using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public class RadialGradientBrush : GradientBrush
    {
        #region Properties

        public static readonly DependencyProperty CenterProperty = RegisterProperty("Center",
                                   typeof(Point),
                                   typeof(RadialGradientBrush),
                                   new Point(0.5f, 0.5f),
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
        public Point Center
        {
            get { return (Point)GetValue(CenterProperty)!; }
            set { SetValue(CenterProperty, value); }
        }

        public static readonly DependencyProperty RadiusXProperty = RegisterProperty("RadiusX",
                                   typeof(float),
                                   typeof(RadialGradientBrush),
                                   0.5f,
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
        public float RadiusX
        {
            get { return (float)GetValue(RadiusXProperty)!; }
            set { SetValue(RadiusXProperty, value); }
        }

        public static readonly DependencyProperty RadiusYProperty = RegisterProperty("RadiusY",
                                   typeof(float),
                                   typeof(RadialGradientBrush),
                                   0.5f,
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
        public float RadiusY
        {
            get { return (float)GetValue(RadiusYProperty)!; }
            set { SetValue(RadiusYProperty, value); }
        }

        public static readonly DependencyProperty GradientOriginProperty = RegisterProperty("GradientOrigin",
                                   typeof(Point),
                                   typeof(RadialGradientBrush),
                                   new Point(0.5f, 0.5f),
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
        public Point GradientOrigin
        {
            get { return (Point)GetValue(GradientOriginProperty)!; }
            set { SetValue(GradientOriginProperty, value); }
        }

        #endregion

        #region Clone

        /// <summary>
        ///     Shadows inherited Clone() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new RadialGradientBrush Clone()
        {
            return (RadialGradientBrush)base.Clone();
        }

        /// <summary>
        ///     Shadows inherited CloneCurrentValue() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new RadialGradientBrush CloneCurrentValue()
        {
            return (RadialGradientBrush)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new RadialGradientBrush();
        }

        #endregion
    }
}
