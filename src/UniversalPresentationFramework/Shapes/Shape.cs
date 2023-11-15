using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Shapes
{
    public abstract class Shape : FrameworkElement
    {
        #region Properties

        public static readonly DependencyProperty FillProperty =
        DependencyProperty.Register(
                "Fill",
                typeof(Brush),
                typeof(Shape),
                new FrameworkPropertyMetadata(
                        null,
                        FrameworkPropertyMetadataOptions.AffectsRender |
                        FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));
        public Brush Fill { get { return (Brush)GetValue(FillProperty)!; } set { SetValue(FillProperty, value); } }

        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(
            "Stretch",                  // Property name
            typeof(Stretch),            // Property type
            typeof(Shape),              // Property owner
            new FrameworkPropertyMetadata(Stretch.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public Stretch Stretch { get { return (Stretch)GetValue(StretchProperty)!; } set { SetValue(StretchProperty, value); } }

        public static readonly DependencyProperty StrokeProperty =
        DependencyProperty.Register(
                "Stroke",
                typeof(Brush),
                typeof(Shape),
                new FrameworkPropertyMetadata(
                        null,
                        FrameworkPropertyMetadataOptions.AffectsMeasure |
                        FrameworkPropertyMetadataOptions.AffectsRender |
                        FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));
        public Brush Stroke { get { return (Brush)GetValue(StrokeProperty)!; } set { SetValue(StrokeProperty, value); } }


        public static readonly DependencyProperty StrokeDashOffsetProperty =
                DependencyProperty.Register(
                        "StrokeDashOffset",
                        typeof(float),
                        typeof(Shape),
                        new FrameworkPropertyMetadata(
                                0.0f,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public float StrokeDashOffset { get { return (float)GetValue(StrokeDashOffsetProperty)!; } set { SetValue(StrokeDashOffsetProperty, value); } }


        public static readonly DependencyProperty StrokeMiterLimitProperty =
                DependencyProperty.Register(
                        "StrokeMiterLimit",
                        typeof(float),
                        typeof(Shape),
                        new FrameworkPropertyMetadata(
                                10.0f,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public float StrokeMiterLimit { get { return (float)GetValue(StrokeMiterLimitProperty)!; } set { SetValue(StrokeMiterLimitProperty, value); } }

        public static readonly DependencyProperty StrokeLineJoinProperty =
        DependencyProperty.Register(
                "StrokeLineJoin",
                typeof(PenLineJoin),
                typeof(Shape),
                new FrameworkPropertyMetadata(
                        PenLineJoin.Miter,
                        FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public PenLineJoin StrokeLineJoin { get { return (PenLineJoin)GetValue(StrokeLineJoinProperty)!; } set { SetValue(StrokeLineJoinProperty, value); } }

        public static readonly DependencyProperty StrokeDashCapProperty =
                DependencyProperty.Register(
                        "StrokeDashCap",
                        typeof(PenLineCap),
                        typeof(Shape),
                        new FrameworkPropertyMetadata(
                                PenLineCap.Flat,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public PenLineCap StrokeDashCap { get { return (PenLineCap)GetValue(StrokeDashCapProperty)!; } set { SetValue(StrokeDashCapProperty, value); } }


        public static readonly DependencyProperty StrokeEndLineCapProperty =
                DependencyProperty.Register(
                        "StrokeEndLineCap",
                        typeof(PenLineCap),
                        typeof(Shape),
                        new FrameworkPropertyMetadata(
                                PenLineCap.Flat,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public PenLineCap StrokeEndLineCap { get { return (PenLineCap)GetValue(StrokeEndLineCapProperty)!; } set { SetValue(StrokeEndLineCapProperty, value); } }

        public static readonly DependencyProperty StrokeStartLineCapProperty =
                DependencyProperty.Register(
                        "StrokeStartLineCap",
                        typeof(PenLineCap),
                        typeof(Shape),
                        new FrameworkPropertyMetadata(
                                PenLineCap.Flat,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public PenLineCap StrokeStartLineCap { get { return (PenLineCap)GetValue(StrokeStartLineCapProperty)!; } set { SetValue(StrokeStartLineCapProperty, value); } }

        public static readonly DependencyProperty StrokeThicknessProperty =
                DependencyProperty.Register(
                        "StrokeThickness",
                        typeof(float),
                        typeof(Shape),
                        new FrameworkPropertyMetadata(
                                1.0f,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        [TypeConverter(typeof(LengthConverter))]
        public float StrokeThickness { get { return (float)GetValue(StrokeThicknessProperty)!; } set { SetValue(StrokeThicknessProperty, value); } }


        public static readonly DependencyProperty StrokeDashArrayProperty =
                DependencyProperty.Register(
                        "StrokeDashArray",
                        typeof(FloatCollection),
                        typeof(Shape),
                        new FrameworkPropertyMetadata(
                                FloatCollection.Empty,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public FloatCollection StrokeDashArray { get { return (FloatCollection)GetValue(StrokeDashArrayProperty)!; } set { SetValue(StrokeDashArrayProperty, value); } }

        #endregion

    }
}
