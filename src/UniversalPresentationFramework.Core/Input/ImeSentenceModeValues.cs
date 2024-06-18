using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    /// <summary>
    /// ImeSentenceModeValues
    /// </summary>
    [Flags]
    public enum ImeSentenceModeValues
    {
        /// <summary>
        /// Non Sentence conversion
        /// </summary>
        None = 0x00000000,
        /// <summary>
        /// PluralClause conversion
        /// </summary>
        PluralClause = 0x00000001,
        /// <summary>
        /// Single Kanji/Hanja conversion
        /// </summary>
        SingleConversion = 0x00000002,
        /// <summary>
        /// automatic conversion mode
        /// </summary>
        Automatic = 0x00000004,
        /// <summary>
        /// phrase prediction mode
        /// </summary>
        PhrasePrediction = 0x00000008,
        /// <summary>
        /// conversation style conversion mode
        /// </summary>
        Conversation = 0x00000010,

        /// <summary>
        /// Mode is not set. It does not care.
        /// </summary>
        DoNotCare = unchecked((int)0x80000000),
    }
}
