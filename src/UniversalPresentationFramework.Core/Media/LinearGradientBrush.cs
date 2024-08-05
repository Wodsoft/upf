using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public class LinearGradientBrush : GradientBrush
    {
        #region Properties

        public static readonly DependencyProperty StartPointProperty = RegisterProperty("StartPoint",
                                   typeof(Point),
                                   typeof(LinearGradientBrush),
                                   new Point(0, 0),
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty)!; }
            set { SetValue(StartPointProperty, value); }
        }

        public static readonly DependencyProperty EndPointProperty = RegisterProperty("EndPoint",
                                   typeof(Point),
                                   typeof(LinearGradientBrush),
                                   new Point(1, 1),
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty)!; }
            set { SetValue(EndPointProperty, value); }
        }

        protected override int EffectiveValuesInitialSize => 3;

        #endregion

        #region Clone

        /// <summary>
        ///     Shadows inherited Clone() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new LinearGradientBrush Clone()
        {
            return (LinearGradientBrush)base.Clone();
        }

        /// <summary>
        ///     Shadows inherited CloneCurrentValue() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new LinearGradientBrush CloneCurrentValue()
        {
            return (LinearGradientBrush)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new LinearGradientBrush();
        }

        #endregion
    }
}
