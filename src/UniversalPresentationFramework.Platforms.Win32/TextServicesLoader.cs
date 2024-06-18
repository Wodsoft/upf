using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.UI.TextServices;

#pragma warning disable CA1416 // 验证平台兼容性
namespace Wodsoft.UI.Platforms.Win32
{
    internal static class TextServicesLoader
    {
        private static object _ServicesInstalledLock = new object();
        private static InstallState _ServicesInstalled = InstallState.Unknown;

        internal static bool ServicesInstalled
        {
            get
            {
                lock (_ServicesInstalledLock)
                {
                    if (_ServicesInstalled == InstallState.Unknown)
                    {
                        _ServicesInstalled = TIPsWantToRun() ? InstallState.Installed : InstallState.NotInstalled;
                    }
                }

                return (_ServicesInstalled == InstallState.Installed);
            }
        }

        private static bool TIPsWantToRun()
        {
            object? obj;
            RegistryKey? key;
            bool tipsWantToRun = false;

            key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\CTF", false);

            // Is cicero disabled completely for the current user?
            if (key != null)
            {
                obj = key.GetValue("Disable Thread Input Manager");

                if (obj is int && (int)obj != 0)
                    return false;
            }

            // Loop through all the TIP entries for machine and current user.
            tipsWantToRun = IterateSubKeys(Registry.LocalMachine, "SOFTWARE\\Microsoft\\CTF\\TIP", new IterateHandler(SingleTIPWantsToRun), true) == EnableState.Enabled;

            return tipsWantToRun;
        }

        private static EnableState IterateSubKeys(RegistryKey keyBase, string subKey, IterateHandler handler, bool localMachine)
        {
            RegistryKey? key;
            string[] subKeyNames;
            EnableState state;

            key = keyBase.OpenSubKey(subKey, false);

            if (key == null)
                return EnableState.Error;

            subKeyNames = key.GetSubKeyNames();
            state = EnableState.Error;

            foreach (string name in subKeyNames)
            {
                switch (handler(key, name, localMachine))
                {
                    case EnableState.Error:
                        break;
                    case EnableState.None:
                        if (localMachine) // For lm, want to return here right away.
                            return EnableState.None;

                        // For current user, remember that we found no Enable value.
                        if (state == EnableState.Error)
                        {
                            state = EnableState.None;
                        }
                        break;
                    case EnableState.Disabled:
                        state = EnableState.Disabled;
                        break;
                    case EnableState.Enabled:
                        return EnableState.Enabled;
                }
            }

            return state;
        }

        private static EnableState SingleTIPWantsToRun(RegistryKey keyLocalMachine, string subKeyName, bool localMachine)
        {
            EnableState result;

            if (subKeyName.Length != _CLSIDLength)
                return EnableState.Disabled;

            // We want subkey\LanguageProfile key.
            // Loop through all the langid entries for TIP.

            // First, check current user.
            result = IterateSubKeys(Registry.CurrentUser, "SOFTWARE\\Microsoft\\CTF\\TIP\\" + subKeyName + "\\LanguageProfile", new IterateHandler(IsLangidEnabled), false);

            // Any explicit value short circuits the process.
            // Otherwise check local machine.
            if (result == EnableState.None || result == EnableState.Error)
            {
                result = IterateSubKeys(keyLocalMachine, subKeyName + "\\LanguageProfile", new IterateHandler(IsLangidEnabled), true);

                if (result == EnableState.None)
                {
                    result = EnableState.Enabled;
                }
            }

            return result;
        }

        private static EnableState IsLangidEnabled(RegistryKey key, string subKeyName, bool localMachine)
        {
            if (subKeyName.Length != _LANGIDLength)
                return EnableState.Error;

            // Loop through all the assembly entries for the langid
            return IterateSubKeys(key, subKeyName, new IterateHandler(IsAssemblyEnabled), localMachine);
        }

        private static EnableState IsAssemblyEnabled(RegistryKey key, string subKeyName, bool localMachine)
        {
            RegistryKey? subKey;
            object? obj;

            if (subKeyName.Length != _CLSIDLength)
                return EnableState.Error;

            // Open the local machine assembly key.
            subKey = key.OpenSubKey(subKeyName);

            if (subKey == null)
                return EnableState.Error;

            // Try to read the "Enable" value.
            obj = subKey.GetValue("Enable");

            if (obj is int)
            {
                return ((int)obj == 0) ? EnableState.Disabled : EnableState.Enabled;
            }

            return EnableState.None;
        }

        internal static ITfThreadMgr2? Load()
        {
            if (ServicesInstalled)
            {
                // NB: a COMException here means something went wrong initialzing Cicero.
                // Cicero will throw an exception if it doesn't think it should have been
                // loaded (no TIPs to run), you can check that in msctf.dll's NoTipsInstalled
                // which lives in nt\windows\advcore\ctf\lib\immxutil.cpp.  If that's the
                // problem, ServicesInstalled is out of sync with Cicero's thinking.
                if (PInvoke.CoCreateInstance<ITfThreadMgr2>(new Guid(unchecked((int)1385864811u), 25991, 20259, [171, 158, 156, 125, 104, 62, 60, 80]), null, Windows.Win32.System.Com.CLSCTX.CLSCTX_INPROC_SERVER, out var threadManager).Succeeded)
                    return threadManager;
                var error = Marshal.GetLastPInvokeError();
                //if (PInvoke.TF_CreateThreadMgr(out var threadManager) == 0)
                //{
                //    return threadManager;
                //}
            }
            return null;
        }

        private delegate EnableState IterateHandler(RegistryKey key, string subKeyName, bool localMachine);

        private const int _CLSIDLength = 38;  // {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
        private const int _LANGIDLength = 10; // 0x12345678

        private enum EnableState
        {
            Error,      // Invalid entry.
            None,       // No explicit Enable entry on the assembly.
            Enabled,    // Assembly is enabled.
            Disabled    // Assembly is disabled.
        };

        private enum InstallState
        {
            Unknown,        // Haven't checked to see if any TIPs are installed yet.
            Installed,      // Checked and installed.
            NotInstalled    // Checked and not installed.
        }
    }
}
