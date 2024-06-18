using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    /// <summary>
    /// ImeConversionModeValues
    /// </summary>
    [Flags]
    public enum ImeConversionModeValues
    {
        /// <summary>
        /// Native Mode (Hiragana, Hangul, Chinese)
        /// </summary>
        Native = 0x00000001,
        /// <summary>
        /// Japanese Katakana Mode
        /// </summary>
        Katakana = 0x00000002,
        /// <summary>
        /// Full Shape mode
        /// </summary>
        FullShape = 0x00000004,
        /// <summary>
        /// Roman Input Mode
        /// </summary>
        Roman = 0x00000008,
        /// <summary>
        /// Roman Input Mode
        /// </summary>
        CharCode = 0x00000010,
        /// <summary>
        /// No conversion
        /// </summary>
        NoConversion = 0x00000020,
        /// <summary>
        /// EUDC symbol(bopomofo) Mode
        /// </summary>
        Eudc = 0x00000040,
        /// <summary>
        /// Symbol Input Mode
        /// </summary>
        Symbol = 0x00000080,
        /// <summary>
        /// Fixed Input Mode
        /// </summary>
        Fixed = 0x00000100,
        /// <summary>
        /// Alphanumeric mode (Alphanumeric mode was 0x0 in Win32 IMM/Cicero).
        /// </summary>
        Alphanumeric = 0x00000200,

        /// <summary>
        /// Mode is not set. It does not care.
        /// </summary>
        DoNotCare = unchecked((int)0x80000000),
    }
}
