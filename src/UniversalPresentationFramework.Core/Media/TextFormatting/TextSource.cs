using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.TextFormatting
{
    public abstract class TextSource
    {
        /// <summary>
        /// TextFormatter to get a text run started at specified text source position
        /// </summary>
        /// <param name="textSourceCharacterIndex">character index to specify where in the source text the fetch is to start.</param>
        /// <returns>text run corresponding to textSourceCharacterIndex.</returns>
        public abstract TextRun GetTextRun(int textSourceCharacterIndex);

        ///// <summary>
        ///// TextFormatter to map a text source character index to a text effect character index        
        ///// </summary>
        ///// <param name="textSourceCharacterIndex"> text source character index </param>
        ///// <returns> the text effect index corresponding to the text source character index </returns>
        //public abstract int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex);

        public abstract int Length { get; }
    }
}
