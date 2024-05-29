using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Vulkan;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Wodsoft.UI.Input;
using Wodsoft.UI.Providers;

namespace Wodsoft.UI.Platforms.Win32
{
    public class InputProvider : IInputProvider
    {
        private readonly static HCURSOR[] _Cursors = new HCURSOR[(int)CursorType.Custom];

        public ICursorContext CreateCursorContext(string filename)
        {
            if (Path.GetExtension(filename) == string.Empty)
                filename += ".cur";
            var file = new FileInfo(filename);
            if (!file.Exists)
                throw new FileNotFoundException($"Cursor file not found: \"{filename}\".");
            var cursor = PInvoke.LoadCursorFromFile(filename);
            if (cursor == default)
                throw new Win32Exception(Marshal.GetLastWin32Error(), $"Can not load cursor file: \"{filename}\".");
            return new Win32CursorContext(cursor);
        }

        public ICursorContext CreateCursorContext(Stream stream)
        {
            var tempFile = Path.GetTempFileName();
            using (var file = File.Open(tempFile, new FileStreamOptions
            {
                Access = FileAccess.ReadWrite,
                Mode = FileMode.Create,
                Options = FileOptions.DeleteOnClose,
                Share = FileShare.Read
            }))
            {
                stream.CopyTo(file);
                file.Flush();
                return CreateCursorContext(tempFile);
            }
        }

        void IInputProvider.SetCursor(ICursorContext cursorContext) => SetCursor(cursorContext);

        void IInputProvider.SetCursor(CursorType cursorType) => SetCursor(cursorType);

        public static void SetCursor(ICursorContext cursorContext)
        {
            if (cursorContext is Win32CursorContext win32CursorContext)
                PInvoke.SetCursor(win32CursorContext.Handle);
            else
                throw new InvalidOperationException("Only support set cursor with Win32CursorContext.");
        }

        public static void SetCursor(CursorType cursorType)
        {
            if (cursorType == CursorType.Custom)
                throw new InvalidOperationException("Can't set custom cursor.");
            var index = (int)cursorType;
            HCURSOR cursor = _Cursors[index];
            if (cursor == default)
                _Cursors[index] = cursor = PInvoke.LoadCursor(default, _CursorTypes[index]);
            PInvoke.SetCursor(cursor);
        }

        private unsafe static readonly PCWSTR[] _CursorTypes = {
            default, // None
            PInvoke.IDC_NO,
            PInvoke.IDC_ARROW,
            PInvoke.IDC_APPSTARTING,
            PInvoke.IDC_CROSS,
            PInvoke.IDC_HELP,
            PInvoke.IDC_IBEAM,
            PInvoke.IDC_SIZEALL,
            PInvoke.IDC_SIZENESW,
            PInvoke.IDC_SIZENS,
            PInvoke.IDC_SIZENWSE,
            PInvoke.IDC_SIZEWE,
            PInvoke.IDC_UPARROW,
            PInvoke.IDC_WAIT,
            PInvoke.IDC_HAND,
            new PCWSTR((char*) 32512 + 119), // PenCursor
            new PCWSTR((char*) 32512 + 140), // ScrollNSCursor
            new PCWSTR((char*) 32512 + 141), // ScrollWECursor
            new PCWSTR((char*) 32512 + 142), // ScrollAllCursor
            new PCWSTR((char*) 32512 + 143), // ScrollNCursor
            new PCWSTR((char*) 32512 + 144), // ScrollSCursor
            new PCWSTR((char*) 32512 + 145), // ScrollWCursor
            new PCWSTR((char*) 32512 + 146), // ScrollECursor
            new PCWSTR((char*) 32512 + 147), // ScrollNWCursor
            new PCWSTR((char*) 32512 + 148), // ScrollNECursor
            new PCWSTR((char*) 32512 + 149), // ScrollSWCursor
            new PCWSTR((char*) 32512 + 150), // ScrollSECursor
            new PCWSTR((char*) 32512 + 151) // ArrowCDCursor
       };

        public Win32MouseDevice MouseDevice { get; } = new Win32MouseDevice();

        public Win32KeyboardDevice KeyboardDevice { get; } = new Win32KeyboardDevice();

        MouseDevice IInputProvider.MouseDevice => MouseDevice;

        KeyboardDevice IInputProvider.KeyboardDevice => KeyboardDevice;
    }
}
