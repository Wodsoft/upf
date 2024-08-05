using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public abstract class GradientBrush : Brush
    {
        #region Constructors

        /// <summary>
        /// Protected constructor for GradientBrush
        /// </summary>
        protected GradientBrush()
        {
        }

        /// <summary>
        /// Protected constructor for GradientBrush
        /// Sets all the values of the GradientStopCollection, all other values are left as default.
        /// </summary>
        protected GradientBrush(GradientStopCollection gradientStopCollection)
        {
            GradientStops = gradientStopCollection;
        }

        #endregion Constructors

        #region Properties

        public static readonly DependencyProperty ColorInterpolationModeProperty = DependencyProperty.Register("ColorInterpolationMode",
                                   typeof(ColorInterpolationMode),
                                   typeof(GradientBrush),
                                   new PropertyMetadata(ColorInterpolationMode.SRgbLinearInterpolation));
        public ColorInterpolationMode ColorInterpolationMode
        {
            get { return (ColorInterpolationMode)GetValue(ColorInterpolationModeProperty)!; }
            set { SetValue(ColorInterpolationModeProperty, value); }
        }

        public static readonly DependencyProperty MappingModeProperty = RegisterProperty("MappingMode",
                                   typeof(BrushMappingMode),
                                   typeof(GradientBrush),
                                   BrushMappingMode.RelativeToBoundingBox,
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ false,
                                   /* coerceValueCallback */ null);
        public BrushMappingMode MappingMode
        {
            get { return (BrushMappingMode)GetValue(MappingModeProperty)!; }
            set { SetValue(MappingModeProperty, value); }
        }

        public static readonly DependencyProperty SpreadMethodProperty = RegisterProperty("SpreadMethod",
                                   typeof(GradientSpreadMethod),
                                   typeof(GradientBrush),
                                   GradientSpreadMethod.Pad,
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ false,
                                   /* coerceValueCallback */ null);
        public GradientSpreadMethod SpreadMethod
        {
            get { return (GradientSpreadMethod)GetValue(SpreadMethodProperty)!; }
            set { SetValue(SpreadMethodProperty, value); }
        }

        public static readonly DependencyProperty GradientStopsProperty = RegisterProperty("GradientStops",
                                   typeof(GradientStopCollection),
                                   typeof(GradientBrush),
                                   new FreezableDefaultValueFactory(GradientStopCollection.Empty),
                                   null,
                                   IsValidGradientStops,
                                   /* isIndependentlyAnimated  = */ false,
                                   /* coerceValueCallback */ null);
        private static bool IsValidGradientStops(object? value)
        {
            return value is not null;
        }
        public GradientStopCollection GradientStops
        {
            get { return (GradientStopCollection)GetValue(GradientStopsProperty)!; }
            set { SetValue(GradientStopsProperty, value); }
        }

        protected override int EffectiveValuesInitialSize => 1;

        #endregion
    }
}
