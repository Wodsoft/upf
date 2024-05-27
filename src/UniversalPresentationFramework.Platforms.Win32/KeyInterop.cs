using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Input;
using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace Wodsoft.UI.Platforms.Win32
{
    /// <summary>
    ///     Provides static methods to convert between Win32 VirtualKeys
    ///     and our Key enum.
    /// </summary>
    public static class KeyInterop
    {
        /// <summary>
        ///     Convert a Win32 VirtualKey into our Key enum.
        /// </summary>
        public static Key KeyFromVirtualKey(int virtualKey)
        {
            Key key = Key.None;
            switch ((VIRTUAL_KEY)virtualKey)
            {
                case VIRTUAL_KEY.VK_CANCEL:
                    key = Key.Cancel;
                    break;

                case VIRTUAL_KEY.VK_BACK:
                    key = Key.Back;
                    break;

                case VIRTUAL_KEY.VK_TAB:
                    key = Key.Tab;
                    break;

                case VIRTUAL_KEY.VK_CLEAR:
                    key = Key.Clear;
                    break;

                case VIRTUAL_KEY.VK_RETURN:
                    key = Key.Return;
                    break;

                case VIRTUAL_KEY.VK_PAUSE:
                    key = Key.Pause;
                    break;

                case VIRTUAL_KEY.VK_CAPITAL:
                    key = Key.Capital;
                    break;

                case VIRTUAL_KEY.VK_KANA:
                    key = Key.KanaMode;
                    break;

                case VIRTUAL_KEY.VK_JUNJA:
                    key = Key.JunjaMode;
                    break;

                case VIRTUAL_KEY.VK_FINAL:
                    key = Key.FinalMode;
                    break;

                case VIRTUAL_KEY.VK_KANJI:
                    key = Key.KanjiMode;
                    break;

                case VIRTUAL_KEY.VK_ESCAPE:
                    key = Key.Escape;
                    break;

                case VIRTUAL_KEY.VK_CONVERT:
                    key = Key.ImeConvert;
                    break;

                case VIRTUAL_KEY.VK_NONCONVERT:
                    key = Key.ImeNonConvert;
                    break;

                case VIRTUAL_KEY.VK_ACCEPT:
                    key = Key.ImeAccept;
                    break;

                case VIRTUAL_KEY.VK_MODECHANGE:
                    key = Key.ImeModeChange;
                    break;

                case VIRTUAL_KEY.VK_SPACE:
                    key = Key.Space;
                    break;

                case VIRTUAL_KEY.VK_PRIOR:
                    key = Key.Prior;
                    break;

                case VIRTUAL_KEY.VK_NEXT:
                    key = Key.Next;
                    break;

                case VIRTUAL_KEY.VK_END:
                    key = Key.End;
                    break;

                case VIRTUAL_KEY.VK_HOME:
                    key = Key.Home;
                    break;

                case VIRTUAL_KEY.VK_LEFT:
                    key = Key.Left;
                    break;

                case VIRTUAL_KEY.VK_UP:
                    key = Key.Up;
                    break;

                case VIRTUAL_KEY.VK_RIGHT:
                    key = Key.Right;
                    break;

                case VIRTUAL_KEY.VK_DOWN:
                    key = Key.Down;
                    break;

                case VIRTUAL_KEY.VK_SELECT:
                    key = Key.Select;
                    break;

                case VIRTUAL_KEY.VK_PRINT:
                    key = Key.Print;
                    break;

                case VIRTUAL_KEY.VK_EXECUTE:
                    key = Key.Execute;
                    break;

                case VIRTUAL_KEY.VK_SNAPSHOT:
                    key = Key.Snapshot;
                    break;

                case VIRTUAL_KEY.VK_INSERT:
                    key = Key.Insert;
                    break;

                case VIRTUAL_KEY.VK_DELETE:
                    key = Key.Delete;
                    break;

                case VIRTUAL_KEY.VK_HELP:
                    key = Key.Help;
                    break;

                case VIRTUAL_KEY.VK_0:
                    key = Key.D0;
                    break;

                case VIRTUAL_KEY.VK_1:
                    key = Key.D1;
                    break;

                case VIRTUAL_KEY.VK_2:
                    key = Key.D2;
                    break;

                case VIRTUAL_KEY.VK_3:
                    key = Key.D3;
                    break;

                case VIRTUAL_KEY.VK_4:
                    key = Key.D4;
                    break;

                case VIRTUAL_KEY.VK_5:
                    key = Key.D5;
                    break;

                case VIRTUAL_KEY.VK_6:
                    key = Key.D6;
                    break;

                case VIRTUAL_KEY.VK_7:
                    key = Key.D7;
                    break;

                case VIRTUAL_KEY.VK_8:
                    key = Key.D8;
                    break;

                case VIRTUAL_KEY.VK_9:
                    key = Key.D9;
                    break;

                case VIRTUAL_KEY.VK_A:
                    key = Key.A;
                    break;

                case VIRTUAL_KEY.VK_B:
                    key = Key.B;
                    break;

                case VIRTUAL_KEY.VK_C:
                    key = Key.C;
                    break;

                case VIRTUAL_KEY.VK_D:
                    key = Key.D;
                    break;

                case VIRTUAL_KEY.VK_E:
                    key = Key.E;
                    break;

                case VIRTUAL_KEY.VK_F:
                    key = Key.F;
                    break;

                case VIRTUAL_KEY.VK_G:
                    key = Key.G;
                    break;

                case VIRTUAL_KEY.VK_H:
                    key = Key.H;
                    break;

                case VIRTUAL_KEY.VK_I:
                    key = Key.I;
                    break;

                case VIRTUAL_KEY.VK_J:
                    key = Key.J;
                    break;

                case VIRTUAL_KEY.VK_K:
                    key = Key.K;
                    break;

                case VIRTUAL_KEY.VK_L:
                    key = Key.L;
                    break;

                case VIRTUAL_KEY.VK_M:
                    key = Key.M;
                    break;

                case VIRTUAL_KEY.VK_N:
                    key = Key.N;
                    break;

                case VIRTUAL_KEY.VK_O:
                    key = Key.O;
                    break;

                case VIRTUAL_KEY.VK_P:
                    key = Key.P;
                    break;

                case VIRTUAL_KEY.VK_Q:
                    key = Key.Q;
                    break;

                case VIRTUAL_KEY.VK_R:
                    key = Key.R;
                    break;

                case VIRTUAL_KEY.VK_S:
                    key = Key.S;
                    break;

                case VIRTUAL_KEY.VK_T:
                    key = Key.T;
                    break;

                case VIRTUAL_KEY.VK_U:
                    key = Key.U;
                    break;

                case VIRTUAL_KEY.VK_V:
                    key = Key.V;
                    break;

                case VIRTUAL_KEY.VK_W:
                    key = Key.W;
                    break;

                case VIRTUAL_KEY.VK_X:
                    key = Key.X;
                    break;

                case VIRTUAL_KEY.VK_Y:
                    key = Key.Y;
                    break;

                case VIRTUAL_KEY.VK_Z:
                    key = Key.Z;
                    break;

                case VIRTUAL_KEY.VK_LWIN:
                    key = Key.LWin;
                    break;

                case VIRTUAL_KEY.VK_RWIN:
                    key = Key.RWin;
                    break;

                case VIRTUAL_KEY.VK_APPS:
                    key = Key.Apps;
                    break;

                case VIRTUAL_KEY.VK_SLEEP:
                    key = Key.Sleep;
                    break;

                case VIRTUAL_KEY.VK_NUMPAD0:
                    key = Key.NumPad0;
                    break;

                case VIRTUAL_KEY.VK_NUMPAD1:
                    key = Key.NumPad1;
                    break;

                case VIRTUAL_KEY.VK_NUMPAD2:
                    key = Key.NumPad2;
                    break;

                case VIRTUAL_KEY.VK_NUMPAD3:
                    key = Key.NumPad3;
                    break;

                case VIRTUAL_KEY.VK_NUMPAD4:
                    key = Key.NumPad4;
                    break;

                case VIRTUAL_KEY.VK_NUMPAD5:
                    key = Key.NumPad5;
                    break;

                case VIRTUAL_KEY.VK_NUMPAD6:
                    key = Key.NumPad6;
                    break;

                case VIRTUAL_KEY.VK_NUMPAD7:
                    key = Key.NumPad7;
                    break;

                case VIRTUAL_KEY.VK_NUMPAD8:
                    key = Key.NumPad8;
                    break;

                case VIRTUAL_KEY.VK_NUMPAD9:
                    key = Key.NumPad9;
                    break;

                case VIRTUAL_KEY.VK_MULTIPLY:
                    key = Key.Multiply;
                    break;

                case VIRTUAL_KEY.VK_ADD:
                    key = Key.Add;
                    break;

                case VIRTUAL_KEY.VK_SEPARATOR:
                    key = Key.Separator;
                    break;

                case VIRTUAL_KEY.VK_SUBTRACT:
                    key = Key.Subtract;
                    break;

                case VIRTUAL_KEY.VK_DECIMAL:
                    key = Key.Decimal;
                    break;

                case VIRTUAL_KEY.VK_DIVIDE:
                    key = Key.Divide;
                    break;

                case VIRTUAL_KEY.VK_F1:
                    key = Key.F1;
                    break;

                case VIRTUAL_KEY.VK_F2:
                    key = Key.F2;
                    break;

                case VIRTUAL_KEY.VK_F3:
                    key = Key.F3;
                    break;

                case VIRTUAL_KEY.VK_F4:
                    key = Key.F4;
                    break;

                case VIRTUAL_KEY.VK_F5:
                    key = Key.F5;
                    break;

                case VIRTUAL_KEY.VK_F6:
                    key = Key.F6;
                    break;

                case VIRTUAL_KEY.VK_F7:
                    key = Key.F7;
                    break;

                case VIRTUAL_KEY.VK_F8:
                    key = Key.F8;
                    break;

                case VIRTUAL_KEY.VK_F9:
                    key = Key.F9;
                    break;

                case VIRTUAL_KEY.VK_F10:
                    key = Key.F10;
                    break;

                case VIRTUAL_KEY.VK_F11:
                    key = Key.F11;
                    break;

                case VIRTUAL_KEY.VK_F12:
                    key = Key.F12;
                    break;

                case VIRTUAL_KEY.VK_F13:
                    key = Key.F13;
                    break;

                case VIRTUAL_KEY.VK_F14:
                    key = Key.F14;
                    break;

                case VIRTUAL_KEY.VK_F15:
                    key = Key.F15;
                    break;

                case VIRTUAL_KEY.VK_F16:
                    key = Key.F16;
                    break;

                case VIRTUAL_KEY.VK_F17:
                    key = Key.F17;
                    break;

                case VIRTUAL_KEY.VK_F18:
                    key = Key.F18;
                    break;

                case VIRTUAL_KEY.VK_F19:
                    key = Key.F19;
                    break;

                case VIRTUAL_KEY.VK_F20:
                    key = Key.F20;
                    break;

                case VIRTUAL_KEY.VK_F21:
                    key = Key.F21;
                    break;

                case VIRTUAL_KEY.VK_F22:
                    key = Key.F22;
                    break;

                case VIRTUAL_KEY.VK_F23:
                    key = Key.F23;
                    break;

                case VIRTUAL_KEY.VK_F24:
                    key = Key.F24;
                    break;

                case VIRTUAL_KEY.VK_NUMLOCK:
                    key = Key.NumLock;
                    break;

                case VIRTUAL_KEY.VK_SCROLL:
                    key = Key.Scroll;
                    break;

                case VIRTUAL_KEY.VK_SHIFT:
                case VIRTUAL_KEY.VK_LSHIFT:
                    key = Key.LeftShift;
                    break;

                case VIRTUAL_KEY.VK_RSHIFT:
                    key = Key.RightShift;
                    break;

                case VIRTUAL_KEY.VK_CONTROL:
                case VIRTUAL_KEY.VK_LCONTROL:
                    key = Key.LeftCtrl;
                    break;

                case VIRTUAL_KEY.VK_RCONTROL:
                    key = Key.RightCtrl;
                    break;

                case VIRTUAL_KEY.VK_MENU:
                case VIRTUAL_KEY.VK_LMENU:
                    key = Key.LeftAlt;
                    break;

                case VIRTUAL_KEY.VK_RMENU:
                    key = Key.RightAlt;
                    break;

                case VIRTUAL_KEY.VK_BROWSER_BACK:
                    key = Key.BrowserBack;
                    break;

                case VIRTUAL_KEY.VK_BROWSER_FORWARD:
                    key = Key.BrowserForward;
                    break;

                case VIRTUAL_KEY.VK_BROWSER_REFRESH:
                    key = Key.BrowserRefresh;
                    break;

                case VIRTUAL_KEY.VK_BROWSER_STOP:
                    key = Key.BrowserStop;
                    break;

                case VIRTUAL_KEY.VK_BROWSER_SEARCH:
                    key = Key.BrowserSearch;
                    break;

                case VIRTUAL_KEY.VK_BROWSER_FAVORITES:
                    key = Key.BrowserFavorites;
                    break;

                case VIRTUAL_KEY.VK_BROWSER_HOME:
                    key = Key.BrowserHome;
                    break;

                case VIRTUAL_KEY.VK_VOLUME_MUTE:
                    key = Key.VolumeMute;
                    break;

                case VIRTUAL_KEY.VK_VOLUME_DOWN:
                    key = Key.VolumeDown;
                    break;

                case VIRTUAL_KEY.VK_VOLUME_UP:
                    key = Key.VolumeUp;
                    break;

                case VIRTUAL_KEY.VK_MEDIA_NEXT_TRACK:
                    key = Key.MediaNextTrack;
                    break;

                case VIRTUAL_KEY.VK_MEDIA_PREV_TRACK:
                    key = Key.MediaPreviousTrack;
                    break;

                case VIRTUAL_KEY.VK_MEDIA_STOP:
                    key = Key.MediaStop;
                    break;

                case VIRTUAL_KEY.VK_MEDIA_PLAY_PAUSE:
                    key = Key.MediaPlayPause;
                    break;

                case VIRTUAL_KEY.VK_LAUNCH_MAIL:
                    key = Key.LaunchMail;
                    break;

                case VIRTUAL_KEY.VK_LAUNCH_MEDIA_SELECT:
                    key = Key.SelectMedia;
                    break;

                case VIRTUAL_KEY.VK_LAUNCH_APP1:
                    key = Key.LaunchApplication1;
                    break;

                case VIRTUAL_KEY.VK_LAUNCH_APP2:
                    key = Key.LaunchApplication2;
                    break;

                case VIRTUAL_KEY.VK_OEM_1:
                    key = Key.OemSemicolon;
                    break;

                case VIRTUAL_KEY.VK_OEM_PLUS:
                    key = Key.OemPlus;
                    break;

                case VIRTUAL_KEY.VK_OEM_COMMA:
                    key = Key.OemComma;
                    break;

                case VIRTUAL_KEY.VK_OEM_MINUS:
                    key = Key.OemMinus;
                    break;

                case VIRTUAL_KEY.VK_OEM_PERIOD:
                    key = Key.OemPeriod;
                    break;

                case VIRTUAL_KEY.VK_OEM_2:
                    key = Key.OemQuestion;
                    break;

                case VIRTUAL_KEY.VK_OEM_3:
                    key = Key.OemTilde;
                    break;

                case VIRTUAL_KEY.VK_ABNT_C1:
                    key = Key.AbntC1;
                    break;

                case VIRTUAL_KEY.VK_ABNT_C2:
                    key = Key.AbntC2;
                    break;

                case VIRTUAL_KEY.VK_OEM_4:
                    key = Key.OemOpenBrackets;
                    break;

                case VIRTUAL_KEY.VK_OEM_5:
                    key = Key.OemPipe;
                    break;

                case VIRTUAL_KEY.VK_OEM_6:
                    key = Key.OemCloseBrackets;
                    break;

                case VIRTUAL_KEY.VK_OEM_7:
                    key = Key.OemQuotes;
                    break;

                case VIRTUAL_KEY.VK_OEM_8:
                    key = Key.Oem8;
                    break;

                case VIRTUAL_KEY.VK_OEM_102:
                    key = Key.OemBackslash;
                    break;

                case VIRTUAL_KEY.VK_PROCESSKEY:
                    key = Key.ImeProcessed;
                    break;

                case VIRTUAL_KEY.VK_OEM_ATTN: // VK_DBE_ALPHANUMERIC
                    key = Key.OemAttn;          // DbeAlphanumeric
                    break;

                case VIRTUAL_KEY.VK_OEM_FINISH: // VK_DBE_KATAKANA
                    key = Key.OemFinish;          // DbeKatakana
                    break;

                case VIRTUAL_KEY.VK_OEM_COPY: // VK_DBE_HIRAGANA
                    key = Key.OemCopy;          // DbeHiragana
                    break;

                case VIRTUAL_KEY.VK_OEM_AUTO: // VK_DBE_SBCSCHAR
                    key = Key.OemAuto;          // DbeSbcsChar
                    break;

                case VIRTUAL_KEY.VK_OEM_ENLW: // VK_DBE_DBCSCHAR
                    key = Key.OemEnlw;          // DbeDbcsChar
                    break;

                case VIRTUAL_KEY.VK_OEM_BACKTAB: // VK_DBE_ROMAN
                    key = Key.OemBackTab;          // DbeRoman
                    break;

                case VIRTUAL_KEY.VK_ATTN: // VK_DBE_NOROMAN
                    key = Key.Attn;         // DbeNoRoman
                    break;

                case VIRTUAL_KEY.VK_CRSEL: // VK_DBE_ENTERWORDREGISTERMODE
                    key = Key.CrSel;         // DbeEnterWordRegisterMode
                    break;

                case VIRTUAL_KEY.VK_EXSEL: // VK_DBE_ENTERIMECONFIGMODE
                    key = Key.ExSel;         // DbeEnterImeConfigMode
                    break;

                case VIRTUAL_KEY.VK_EREOF: // VK_DBE_FLUSHSTRING
                    key = Key.EraseEof;      // DbeFlushString
                    break;

                case VIRTUAL_KEY.VK_PLAY: // VK_DBE_CODEINPUT
                    key = Key.Play;         // DbeCodeInput
                    break;

                case VIRTUAL_KEY.VK_ZOOM: // VK_DBE_NOCODEINPUT
                    key = Key.Zoom;         // DbeNoCodeInput
                    break;

                case VIRTUAL_KEY.VK_NONAME: // VK_DBE_DETERMINESTRING
                    key = Key.NoName;         // DbeDetermineString
                    break;

                case VIRTUAL_KEY.VK_PA1: // VK_DBE_ENTERDLGCONVERSIONMODE
                    key = Key.Pa1;         // DbeEnterDlgConversionMode
                    break;

                case VIRTUAL_KEY.VK_OEM_CLEAR:
                    key = Key.OemClear;
                    break;

                default:
                    key = Key.None;
                    break;
            }

            return key;
        }

        /// <summary>
        ///     Convert our Key enum into a Win32 VirtualKey.
        /// </summary>
        public static int VirtualKeyFromKey(Key key)
        {
            VIRTUAL_KEY virtualKey = 0;

            switch (key)
            {
                case Key.Cancel:
                    virtualKey = VIRTUAL_KEY.VK_CANCEL;
                    break;

                case Key.Back:
                    virtualKey = VIRTUAL_KEY.VK_BACK;
                    break;

                case Key.Tab:
                    virtualKey = VIRTUAL_KEY.VK_TAB;
                    break;

                case Key.Clear:
                    virtualKey = VIRTUAL_KEY.VK_CLEAR;
                    break;

                case Key.Return:
                    virtualKey = VIRTUAL_KEY.VK_RETURN;
                    break;

                case Key.Pause:
                    virtualKey = VIRTUAL_KEY.VK_PAUSE;
                    break;

                case Key.Capital:
                    virtualKey = VIRTUAL_KEY.VK_CAPITAL;
                    break;

                case Key.KanaMode:
                    virtualKey = VIRTUAL_KEY.VK_KANA;
                    break;

                case Key.JunjaMode:
                    virtualKey = VIRTUAL_KEY.VK_JUNJA;
                    break;

                case Key.FinalMode:
                    virtualKey = VIRTUAL_KEY.VK_FINAL;
                    break;

                case Key.KanjiMode:
                    virtualKey = VIRTUAL_KEY.VK_KANJI;
                    break;

                case Key.Escape:
                    virtualKey = VIRTUAL_KEY.VK_ESCAPE;
                    break;

                case Key.ImeConvert:
                    virtualKey = VIRTUAL_KEY.VK_CONVERT;
                    break;

                case Key.ImeNonConvert:
                    virtualKey = VIRTUAL_KEY.VK_NONCONVERT;
                    break;

                case Key.ImeAccept:
                    virtualKey = VIRTUAL_KEY.VK_ACCEPT;
                    break;

                case Key.ImeModeChange:
                    virtualKey = VIRTUAL_KEY.VK_MODECHANGE;
                    break;

                case Key.Space:
                    virtualKey = VIRTUAL_KEY.VK_SPACE;
                    break;

                case Key.Prior:
                    virtualKey = VIRTUAL_KEY.VK_PRIOR;
                    break;

                case Key.Next:
                    virtualKey = VIRTUAL_KEY.VK_NEXT;
                    break;

                case Key.End:
                    virtualKey = VIRTUAL_KEY.VK_END;
                    break;

                case Key.Home:
                    virtualKey = VIRTUAL_KEY.VK_HOME;
                    break;

                case Key.Left:
                    virtualKey = VIRTUAL_KEY.VK_LEFT;
                    break;

                case Key.Up:
                    virtualKey = VIRTUAL_KEY.VK_UP;
                    break;

                case Key.Right:
                    virtualKey = VIRTUAL_KEY.VK_RIGHT;
                    break;

                case Key.Down:
                    virtualKey = VIRTUAL_KEY.VK_DOWN;
                    break;

                case Key.Select:
                    virtualKey = VIRTUAL_KEY.VK_SELECT;
                    break;

                case Key.Print:
                    virtualKey = VIRTUAL_KEY.VK_PRINT;
                    break;

                case Key.Execute:
                    virtualKey = VIRTUAL_KEY.VK_EXECUTE;
                    break;

                case Key.Snapshot:
                    virtualKey = VIRTUAL_KEY.VK_SNAPSHOT;
                    break;

                case Key.Insert:
                    virtualKey = VIRTUAL_KEY.VK_INSERT;
                    break;

                case Key.Delete:
                    virtualKey = VIRTUAL_KEY.VK_DELETE;
                    break;

                case Key.Help:
                    virtualKey = VIRTUAL_KEY.VK_HELP;
                    break;

                case Key.D0:
                    virtualKey = VIRTUAL_KEY.VK_0;
                    break;

                case Key.D1:
                    virtualKey = VIRTUAL_KEY.VK_1;
                    break;

                case Key.D2:
                    virtualKey = VIRTUAL_KEY.VK_2;
                    break;

                case Key.D3:
                    virtualKey = VIRTUAL_KEY.VK_3;
                    break;

                case Key.D4:
                    virtualKey = VIRTUAL_KEY.VK_4;
                    break;

                case Key.D5:
                    virtualKey = VIRTUAL_KEY.VK_5;
                    break;

                case Key.D6:
                    virtualKey = VIRTUAL_KEY.VK_6;
                    break;

                case Key.D7:
                    virtualKey = VIRTUAL_KEY.VK_7;
                    break;

                case Key.D8:
                    virtualKey = VIRTUAL_KEY.VK_8;
                    break;

                case Key.D9:
                    virtualKey = VIRTUAL_KEY.VK_9;
                    break;

                case Key.A:
                    virtualKey = VIRTUAL_KEY.VK_A;
                    break;

                case Key.B:
                    virtualKey = VIRTUAL_KEY.VK_B;
                    break;

                case Key.C:
                    virtualKey = VIRTUAL_KEY.VK_C;
                    break;

                case Key.D:
                    virtualKey = VIRTUAL_KEY.VK_D;
                    break;

                case Key.E:
                    virtualKey = VIRTUAL_KEY.VK_E;
                    break;

                case Key.F:
                    virtualKey = VIRTUAL_KEY.VK_F;
                    break;

                case Key.G:
                    virtualKey = VIRTUAL_KEY.VK_G;
                    break;

                case Key.H:
                    virtualKey = VIRTUAL_KEY.VK_H;
                    break;

                case Key.I:
                    virtualKey = VIRTUAL_KEY.VK_I;
                    break;

                case Key.J:
                    virtualKey = VIRTUAL_KEY.VK_J;
                    break;

                case Key.K:
                    virtualKey = VIRTUAL_KEY.VK_K;
                    break;

                case Key.L:
                    virtualKey = VIRTUAL_KEY.VK_L;
                    break;

                case Key.M:
                    virtualKey = VIRTUAL_KEY.VK_M;
                    break;

                case Key.N:
                    virtualKey = VIRTUAL_KEY.VK_N;
                    break;

                case Key.O:
                    virtualKey = VIRTUAL_KEY.VK_O;
                    break;

                case Key.P:
                    virtualKey = VIRTUAL_KEY.VK_P;
                    break;

                case Key.Q:
                    virtualKey = VIRTUAL_KEY.VK_Q;
                    break;

                case Key.R:
                    virtualKey = VIRTUAL_KEY.VK_R;
                    break;

                case Key.S:
                    virtualKey = VIRTUAL_KEY.VK_S;
                    break;

                case Key.T:
                    virtualKey = VIRTUAL_KEY.VK_T;
                    break;

                case Key.U:
                    virtualKey = VIRTUAL_KEY.VK_U;
                    break;

                case Key.V:
                    virtualKey = VIRTUAL_KEY.VK_V;
                    break;

                case Key.W:
                    virtualKey = VIRTUAL_KEY.VK_W;
                    break;

                case Key.X:
                    virtualKey = VIRTUAL_KEY.VK_X;
                    break;

                case Key.Y:
                    virtualKey = VIRTUAL_KEY.VK_Y;
                    break;

                case Key.Z:
                    virtualKey = VIRTUAL_KEY.VK_Z;
                    break;

                case Key.LWin:
                    virtualKey = VIRTUAL_KEY.VK_LWIN;
                    break;

                case Key.RWin:
                    virtualKey = VIRTUAL_KEY.VK_RWIN;
                    break;

                case Key.Apps:
                    virtualKey = VIRTUAL_KEY.VK_APPS;
                    break;

                case Key.Sleep:
                    virtualKey = VIRTUAL_KEY.VK_SLEEP;
                    break;

                case Key.NumPad0:
                    virtualKey = VIRTUAL_KEY.VK_NUMPAD0;
                    break;

                case Key.NumPad1:
                    virtualKey = VIRTUAL_KEY.VK_NUMPAD1;
                    break;

                case Key.NumPad2:
                    virtualKey = VIRTUAL_KEY.VK_NUMPAD2;
                    break;

                case Key.NumPad3:
                    virtualKey = VIRTUAL_KEY.VK_NUMPAD3;
                    break;

                case Key.NumPad4:
                    virtualKey = VIRTUAL_KEY.VK_NUMPAD4;
                    break;

                case Key.NumPad5:
                    virtualKey = VIRTUAL_KEY.VK_NUMPAD5;
                    break;

                case Key.NumPad6:
                    virtualKey = VIRTUAL_KEY.VK_NUMPAD6;
                    break;

                case Key.NumPad7:
                    virtualKey = VIRTUAL_KEY.VK_NUMPAD7;
                    break;

                case Key.NumPad8:
                    virtualKey = VIRTUAL_KEY.VK_NUMPAD8;
                    break;

                case Key.NumPad9:
                    virtualKey = VIRTUAL_KEY.VK_NUMPAD9;
                    break;

                case Key.Multiply:
                    virtualKey = VIRTUAL_KEY.VK_MULTIPLY;
                    break;

                case Key.Add:
                    virtualKey = VIRTUAL_KEY.VK_ADD;
                    break;

                case Key.Separator:
                    virtualKey = VIRTUAL_KEY.VK_SEPARATOR;
                    break;

                case Key.Subtract:
                    virtualKey = VIRTUAL_KEY.VK_SUBTRACT;
                    break;

                case Key.Decimal:
                    virtualKey = VIRTUAL_KEY.VK_DECIMAL;
                    break;

                case Key.Divide:
                    virtualKey = VIRTUAL_KEY.VK_DIVIDE;
                    break;

                case Key.F1:
                    virtualKey = VIRTUAL_KEY.VK_F1;
                    break;

                case Key.F2:
                    virtualKey = VIRTUAL_KEY.VK_F2;
                    break;

                case Key.F3:
                    virtualKey = VIRTUAL_KEY.VK_F3;
                    break;

                case Key.F4:
                    virtualKey = VIRTUAL_KEY.VK_F4;
                    break;

                case Key.F5:
                    virtualKey = VIRTUAL_KEY.VK_F5;
                    break;

                case Key.F6:
                    virtualKey = VIRTUAL_KEY.VK_F6;
                    break;

                case Key.F7:
                    virtualKey = VIRTUAL_KEY.VK_F7;
                    break;

                case Key.F8:
                    virtualKey = VIRTUAL_KEY.VK_F8;
                    break;

                case Key.F9:
                    virtualKey = VIRTUAL_KEY.VK_F9;
                    break;

                case Key.F10:
                    virtualKey = VIRTUAL_KEY.VK_F10;
                    break;

                case Key.F11:
                    virtualKey = VIRTUAL_KEY.VK_F11;
                    break;

                case Key.F12:
                    virtualKey = VIRTUAL_KEY.VK_F12;
                    break;

                case Key.F13:
                    virtualKey = VIRTUAL_KEY.VK_F13;
                    break;

                case Key.F14:
                    virtualKey = VIRTUAL_KEY.VK_F14;
                    break;

                case Key.F15:
                    virtualKey = VIRTUAL_KEY.VK_F15;
                    break;

                case Key.F16:
                    virtualKey = VIRTUAL_KEY.VK_F16;
                    break;

                case Key.F17:
                    virtualKey = VIRTUAL_KEY.VK_F17;
                    break;

                case Key.F18:
                    virtualKey = VIRTUAL_KEY.VK_F18;
                    break;

                case Key.F19:
                    virtualKey = VIRTUAL_KEY.VK_F19;
                    break;

                case Key.F20:
                    virtualKey = VIRTUAL_KEY.VK_F20;
                    break;

                case Key.F21:
                    virtualKey = VIRTUAL_KEY.VK_F21;
                    break;

                case Key.F22:
                    virtualKey = VIRTUAL_KEY.VK_F22;
                    break;

                case Key.F23:
                    virtualKey = VIRTUAL_KEY.VK_F23;
                    break;

                case Key.F24:
                    virtualKey = VIRTUAL_KEY.VK_F24;
                    break;

                case Key.NumLock:
                    virtualKey = VIRTUAL_KEY.VK_NUMLOCK;
                    break;

                case Key.Scroll:
                    virtualKey = VIRTUAL_KEY.VK_SCROLL;
                    break;

                case Key.LeftShift:
                    virtualKey = VIRTUAL_KEY.VK_LSHIFT;
                    break;

                case Key.RightShift:
                    virtualKey = VIRTUAL_KEY.VK_RSHIFT;
                    break;

                case Key.LeftCtrl:
                    virtualKey = VIRTUAL_KEY.VK_LCONTROL;
                    break;

                case Key.RightCtrl:
                    virtualKey = VIRTUAL_KEY.VK_RCONTROL;
                    break;

                case Key.LeftAlt:
                    virtualKey = VIRTUAL_KEY.VK_LMENU;
                    break;

                case Key.RightAlt:
                    virtualKey = VIRTUAL_KEY.VK_RMENU;
                    break;

                case Key.BrowserBack:
                    virtualKey = VIRTUAL_KEY.VK_BROWSER_BACK;
                    break;

                case Key.BrowserForward:
                    virtualKey = VIRTUAL_KEY.VK_BROWSER_FORWARD;
                    break;

                case Key.BrowserRefresh:
                    virtualKey = VIRTUAL_KEY.VK_BROWSER_REFRESH;
                    break;

                case Key.BrowserStop:
                    virtualKey = VIRTUAL_KEY.VK_BROWSER_STOP;
                    break;

                case Key.BrowserSearch:
                    virtualKey = VIRTUAL_KEY.VK_BROWSER_SEARCH;
                    break;

                case Key.BrowserFavorites:
                    virtualKey = VIRTUAL_KEY.VK_BROWSER_FAVORITES;
                    break;

                case Key.BrowserHome:
                    virtualKey = VIRTUAL_KEY.VK_BROWSER_HOME;
                    break;

                case Key.VolumeMute:
                    virtualKey = VIRTUAL_KEY.VK_VOLUME_MUTE;
                    break;

                case Key.VolumeDown:
                    virtualKey = VIRTUAL_KEY.VK_VOLUME_DOWN;
                    break;

                case Key.VolumeUp:
                    virtualKey = VIRTUAL_KEY.VK_VOLUME_UP;
                    break;

                case Key.MediaNextTrack:
                    virtualKey = VIRTUAL_KEY.VK_MEDIA_NEXT_TRACK;
                    break;

                case Key.MediaPreviousTrack:
                    virtualKey = VIRTUAL_KEY.VK_MEDIA_PREV_TRACK;
                    break;

                case Key.MediaStop:
                    virtualKey = VIRTUAL_KEY.VK_MEDIA_STOP;
                    break;

                case Key.MediaPlayPause:
                    virtualKey = VIRTUAL_KEY.VK_MEDIA_PLAY_PAUSE;
                    break;

                case Key.LaunchMail:
                    virtualKey = VIRTUAL_KEY.VK_LAUNCH_MAIL;
                    break;

                case Key.SelectMedia:
                    virtualKey = VIRTUAL_KEY.VK_LAUNCH_MEDIA_SELECT;
                    break;

                case Key.LaunchApplication1:
                    virtualKey = VIRTUAL_KEY.VK_LAUNCH_APP1;
                    break;

                case Key.LaunchApplication2:
                    virtualKey = VIRTUAL_KEY.VK_LAUNCH_APP2;
                    break;

                case Key.OemSemicolon:
                    virtualKey = VIRTUAL_KEY.VK_OEM_1;
                    break;

                case Key.OemPlus:
                    virtualKey = VIRTUAL_KEY.VK_OEM_PLUS;
                    break;

                case Key.OemComma:
                    virtualKey = VIRTUAL_KEY.VK_OEM_COMMA;
                    break;

                case Key.OemMinus:
                    virtualKey = VIRTUAL_KEY.VK_OEM_MINUS;
                    break;

                case Key.OemPeriod:
                    virtualKey = VIRTUAL_KEY.VK_OEM_PERIOD;
                    break;

                case Key.OemQuestion:
                    virtualKey = VIRTUAL_KEY.VK_OEM_2;
                    break;

                case Key.OemTilde:
                    virtualKey = VIRTUAL_KEY.VK_OEM_3;
                    break;

                case Key.AbntC1:
                    virtualKey = VIRTUAL_KEY.VK_ABNT_C1;
                    break;

                case Key.AbntC2:
                    virtualKey = VIRTUAL_KEY.VK_ABNT_C2;
                    break;

                case Key.OemOpenBrackets:
                    virtualKey = VIRTUAL_KEY.VK_OEM_4;
                    break;

                case Key.OemPipe:
                    virtualKey = VIRTUAL_KEY.VK_OEM_5;
                    break;

                case Key.OemCloseBrackets:
                    virtualKey = VIRTUAL_KEY.VK_OEM_6;
                    break;

                case Key.OemQuotes:
                    virtualKey = VIRTUAL_KEY.VK_OEM_7;
                    break;

                case Key.Oem8:
                    virtualKey = VIRTUAL_KEY.VK_OEM_8;
                    break;

                case Key.OemBackslash:
                    virtualKey = VIRTUAL_KEY.VK_OEM_102;
                    break;

                case Key.ImeProcessed:
                    virtualKey = VIRTUAL_KEY.VK_PROCESSKEY;
                    break;

                case Key.OemAttn:                           // DbeAlphanumeric
                    virtualKey = VIRTUAL_KEY.VK_OEM_ATTN; // VK_DBE_ALPHANUMERIC
                    break;

                case Key.OemFinish:                           // DbeKatakana
                    virtualKey = VIRTUAL_KEY.VK_OEM_FINISH; // VK_DBE_KATAKANA
                    break;

                case Key.OemCopy:                           // DbeHiragana
                    virtualKey = VIRTUAL_KEY.VK_OEM_COPY; // VK_DBE_HIRAGANA
                    break;

                case Key.OemAuto:                           // DbeSbcsChar
                    virtualKey = VIRTUAL_KEY.VK_OEM_AUTO; // VK_DBE_SBCSCHAR
                    break;

                case Key.OemEnlw:                           // DbeDbcsChar
                    virtualKey = VIRTUAL_KEY.VK_OEM_ENLW; // VK_DBE_DBCSCHAR
                    break;

                case Key.OemBackTab:                           // DbeRoman
                    virtualKey = VIRTUAL_KEY.VK_OEM_BACKTAB; // VK_DBE_ROMAN
                    break;

                case Key.Attn:                          // DbeNoRoman
                    virtualKey = VIRTUAL_KEY.VK_ATTN; // VK_DBE_NOROMAN
                    break;

                case Key.CrSel:                          // DbeEnterWordRegisterMode
                    virtualKey = VIRTUAL_KEY.VK_CRSEL; // VK_DBE_ENTERWORDREGISTERMODE
                    break;

                case Key.ExSel:                          // EnterImeConfigureMode
                    virtualKey = VIRTUAL_KEY.VK_EXSEL; // VK_DBE_ENTERIMECONFIGMODE
                    break;

                case Key.EraseEof:                       // DbeFlushString
                    virtualKey = VIRTUAL_KEY.VK_EREOF; // VK_DBE_FLUSHSTRING
                    break;

                case Key.Play:                           // DbeCodeInput
                    virtualKey = VIRTUAL_KEY.VK_PLAY;  // VK_DBE_CODEINPUT
                    break;

                case Key.Zoom:                           // DbeNoCodeInput
                    virtualKey = VIRTUAL_KEY.VK_ZOOM;  // VK_DBE_NOCODEINPUT
                    break;

                case Key.NoName:                          // DbeDetermineString
                    virtualKey = VIRTUAL_KEY.VK_NONAME; // VK_DBE_DETERMINESTRING
                    break;

                case Key.Pa1:                          // DbeEnterDlgConversionMode
                    virtualKey = VIRTUAL_KEY.VK_PA1; // VK_ENTERDLGCONVERSIONMODE
                    break;

                case Key.OemClear:
                    virtualKey = VIRTUAL_KEY.VK_OEM_CLEAR;
                    break;

                case Key.DeadCharProcessed:             //This is usused.  It's just here for completeness.
                    virtualKey = 0;                     //There is no Win32 VKey for this.
                    break;

                default:
                    virtualKey = 0;
                    break;
            }

            return (int)virtualKey;
        }
    }
}
