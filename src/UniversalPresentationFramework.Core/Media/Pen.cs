using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI.Media
{
    public class Pen : Animatable
    {
        #region Constructors

        /// <summary>
        ///
        /// </summary>
        public Pen()
        {
        }

        /// <summary>
        /// Pen - Initializes the pen from the given Brush and thickness.
        /// All other values as set to default.
        /// </summary>
        /// <param name="brush"> The Brush for this Pen. </param>
        /// <param name="thickness"> The thickness of the Pen. </param>
        public Pen(Brush brush,
                   float thickness)
        {
            Brush = brush;
            Thickness = thickness;
        }

        /// <summary>
        /// Pen - Initializes the brush from the parameters.
        /// </summary>
        /// <param name="brush"> The Pen's Brush. </param>
        /// <param name="thickness"> The Pen's thickness. </param>
        /// <param name="startLineCap"> The PenLineCap which applies to the start of the . </param>
        /// <param name="endLineCap"> The PenLineCap which applies to the end of the . </param>
        /// <param name="dashCap"> The PenDashCap which applies to the ends of each dash. </param>
        /// <param name="lineJoin"> The PenLineJoin. </param>
        /// <param name="miterLimit"> The miter limit. </param>
        /// <param name="dashStyle"> The dash style. </param>
        public Pen(
            Brush brush,
            float thickness,
            PenLineCap startLineCap,
            PenLineCap endLineCap,
            PenLineCap dashCap,
            PenLineJoin lineJoin,
            float miterLimit,
            DashStyle dashStyle)
        {
            Thickness = thickness;
            StartLineCap = startLineCap;
            EndLineCap = endLineCap;
            DashCap = dashCap;
            LineJoin = lineJoin;
            MiterLimit = miterLimit;

            Brush = brush;
            DashStyle = dashStyle;
        }

        #endregion

        #region Properties


        public static readonly DependencyProperty BrushProperty =
                  DependencyProperty.Register("Brush",
                                   typeof(Brush),
                                   typeof(Pen));
        public Brush? Brush { get { return (Brush?)GetValue(BrushProperty); } set { SetValue(BrushProperty, value); } }


        public static readonly DependencyProperty DashStyleProperty =
                  DependencyProperty.Register("DashStyle",
                                   typeof(DashStyle),
                                   typeof(Pen),
                                   new PropertyMetadata(DashStyles.Solid));
        public DashStyle DashStyle { get { return (DashStyle)GetValue(DashStyleProperty)!; } set { SetValue(DashStyleProperty, value); } }


        public static readonly DependencyProperty MiterLimitProperty =
                DependencyProperty.Register(
                        "MiterLimit",
                        typeof(float),
                        typeof(Pen),
                        new PropertyMetadata(10.0));
        public float MiterLimit { get { return (float)GetValue(MiterLimitProperty)!; } set { SetValue(MiterLimitProperty, value); } }

        public static readonly DependencyProperty LineJoinProperty =
        DependencyProperty.Register(
                "LineJoin",
                typeof(PenLineJoin),
                typeof(Pen),
                new PropertyMetadata(PenLineJoin.Miter));
        public PenLineJoin LineJoin { get { return (PenLineJoin)GetValue(LineJoinProperty)!; } set { SetValue(LineJoinProperty, value); } }

        public static readonly DependencyProperty DashCapProperty =
                DependencyProperty.Register(
                        "DashCap",
                        typeof(PenLineCap),
                        typeof(Pen),
                        new PropertyMetadata(PenLineCap.Flat));
        public PenLineCap DashCap { get { return (PenLineCap)GetValue(DashCapProperty)!; } set { SetValue(DashCapProperty, value); } }


        public static readonly DependencyProperty EndLineCapProperty =
                DependencyProperty.Register(
                        "EndLineCap",
                        typeof(PenLineCap),
                        typeof(Pen),
                        new PropertyMetadata(PenLineCap.Flat));
        public PenLineCap EndLineCap { get { return (PenLineCap)GetValue(EndLineCapProperty)!; } set { SetValue(EndLineCapProperty, value); } }

        public static readonly DependencyProperty StartLineCapProperty =
                DependencyProperty.Register(
                        "StartLineCap",
                        typeof(PenLineCap),
                        typeof(Pen),
                        new PropertyMetadata(PenLineCap.Flat));
        public PenLineCap StartLineCap { get { return (PenLineCap)GetValue(StartLineCapProperty)!; } set { SetValue(StartLineCapProperty, value); } }

        public static readonly DependencyProperty ThicknessProperty =
                DependencyProperty.Register(
                        "Thickness",
                        typeof(float),
                        typeof(Pen),
                        new PropertyMetadata(1.0f));

        public float Thickness { get { return (float)GetValue(ThicknessProperty)!; } set { SetValue(ThicknessProperty, value); } }

        #endregion

        #region Clone

        protected override Freezable CreateInstanceCore()
        {
            return new Pen();
        }

        #endregion
    }
}
