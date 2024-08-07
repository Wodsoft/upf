﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    /// <summary>
    ///     This interface represents a transformation of normalizedTime.  Animations use it to 
    ///     transform their progress before computing an interpolation.  Classes that implement
    ///     this interface can control the pace at which an animation is performed.
    /// </summary>
    public interface IEasingFunction
    {
        /// <summary>
        ///     Transforms normalized time to control the pace of an animation.
        /// </summary>
        /// <param name="normalizedTime">normalized time (progress) of the animation</param>
        /// <returns>transformed progress</returns>
        float Ease(float normalizedTime);
    }
}
