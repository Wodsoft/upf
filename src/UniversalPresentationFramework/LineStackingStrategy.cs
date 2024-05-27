using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    /// <summary>
    /// This property describes mechanism by which a line box is determined for each line.
    /// </summary>
    public enum LineStackingStrategy
    {
        /// <summary>
        /// The stack-height is determined by the block element 'line-height' property value.
        /// </summary>
        BlockLineHeight,

        /// <summary>
        /// The stack-height is the smallest value that contains the extended block progression 
        /// dimension of all the inline elements on that line when those elements are properly aligned.
        /// The 'line-height' property value is taken into account only for the block elements.
        /// </summary>
        MaxHeight,

        ///// <summary>
        ///// The stack-height is the smallest value that contains the extended block progression 
        ///// dimension of all the inline elements on that line when those elements are properly aligned.
        ///// </summary>
        //InlineLineHeight,        

        ///// <summary>
        ///// The stack-height is the smallest multiple of the block element 'line-height' computed value 
        ///// that can contain the block progression of all the inline elements on that line when those 
        ///// elements are properly aligned.
        ///// </summary>
        //GridHeight,
    }
}
