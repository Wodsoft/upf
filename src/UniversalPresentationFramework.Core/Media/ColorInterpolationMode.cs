using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    /// <summary>
    ///     ColorInterpolationMode - This determines how the colors in a gradient are 
    ///     interpolated.
    /// </summary>
    public enum ColorInterpolationMode
    {
        /// <summary>
        ///     ScRgbLinearInterpolation - Colors are interpolated in the scRGB color space
        /// </summary>
        ScRgbLinearInterpolation = 0,

        /// <summary>
        ///     SRgbLinearInterpolation - Colors are interpolated in the sRGB color space
        /// </summary>
        SRgbLinearInterpolation = 1,
    }
}
