using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public abstract class TileBrush : Brush
    {
        #region Properties

        public static readonly DependencyProperty ViewportUnitsProperty = RegisterProperty("ViewportUnits",
                                   typeof(BrushMappingMode),
                                   typeof(TileBrush),
                                   BrushMappingMode.RelativeToBoundingBox,
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ false,
                                   /* coerceValueCallback */ null);
        public BrushMappingMode ViewportUnits
        {
            get { return (BrushMappingMode)GetValue(ViewportUnitsProperty)!; }
            set { SetValue(ViewportUnitsProperty, value); }
        }

        public static readonly DependencyProperty ViewboxUnitsProperty = RegisterProperty("ViewboxUnits",
                                   typeof(BrushMappingMode),
                                   typeof(TileBrush),
                                   BrushMappingMode.RelativeToBoundingBox,
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ false,
                                   /* coerceValueCallback */ null);
        public BrushMappingMode ViewboxUnits
        {
            get { return (BrushMappingMode)GetValue(ViewboxUnitsProperty)!; }
            set { SetValue(ViewboxUnitsProperty, value); }
        }

        public static readonly DependencyProperty ViewportProperty = RegisterProperty("Viewport",
                                   typeof(Rect),
                                   typeof(TileBrush),
                                   new Rect(0, 0, 1, 1),
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
        public Rect Viewport
        {
            get { return (Rect)GetValue(ViewportProperty)!; }
            set { SetValue(ViewportProperty, value); }
        }

        public static readonly DependencyProperty ViewboxProperty = RegisterProperty("Viewbox",
                                   typeof(Rect),
                                   typeof(TileBrush),
                                   new Rect(0, 0, 1, 1),
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ true,
                                   /* coerceValueCallback */ null);
        public Rect Viewbox
        {
            get { return (Rect)GetValue(ViewboxProperty)!; }
            set { SetValue(ViewboxProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = RegisterProperty("Stretch",
                                   typeof(Stretch),
                                   typeof(TileBrush),
                                   Stretch.Fill,
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ false,
                                   /* coerceValueCallback */ null);
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty)!; }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty TileModeProperty = RegisterProperty("TileMode",
                                   typeof(TileMode),
                                   typeof(TileBrush),
                                   TileMode.None,
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ false,
                                   /* coerceValueCallback */ null);
        public TileMode TileMode
        {
            get { return (TileMode)GetValue(TileModeProperty)!; }
            set { SetValue(TileModeProperty, value); }
        }

        public static readonly DependencyProperty AlignmentXProperty = RegisterProperty("AlignmentX",
                                   typeof(AlignmentX),
                                   typeof(TileBrush),
                                   AlignmentX.Center,
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ false,
                                   /* coerceValueCallback */ null);
        public AlignmentX AlignmentX
        {
            get { return (AlignmentX)GetValue(AlignmentXProperty)!; }
            set { SetValue(AlignmentXProperty, value); }
        }

        public static readonly DependencyProperty AlignmentYProperty = RegisterProperty("AlignmentY",
                                   typeof(AlignmentY),
                                   typeof(TileBrush),
                                   AlignmentY.Center,
                                   null,
                                   null,
                                   /* isIndependentlyAnimated  = */ false,
                                   /* coerceValueCallback */ null);
        public AlignmentY AlignmentY
        {
            get { return (AlignmentY)GetValue(AlignmentYProperty)!; }
            set { SetValue(AlignmentYProperty, value); }
        }

        #endregion

        #region Methods

        ///// <summary>
        ///// Obtains the current bounds of the brush's content
        ///// </summary>
        ///// <param name="contentBounds"> Output bounds of content </param>            
        //protected abstract void GetContentBounds(out Rect contentBounds);

        #endregion
    }
}
