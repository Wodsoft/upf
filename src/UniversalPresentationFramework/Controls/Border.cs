using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    public class Border : Decorator
    {
        #region Properties

        public static readonly DependencyProperty BorderThicknessProperty
            = DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(Border),
                                          new FrameworkPropertyMetadata(
                                                new Thickness(),
                                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                                                new PropertyChangedCallback(OnClearPenCache)), new ValidateValueCallback(IsThicknessValid));
        private static void OnClearPenCache(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Border border = (Border)d;
            border._leftPenCache = null;
            border._rightPenCache = null;
            border._topPenCache = null;
            border._bottomPenCache = null;
        }
        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty)!; }
            set { SetValue(BorderThicknessProperty, value); }
        }

        private static bool IsThicknessValid(object? value)
        {
            Thickness t = (Thickness)value!;
            return t.IsValid(false, false, false, false);
        }

        public static readonly DependencyProperty PaddingProperty
            = DependencyProperty.Register("Padding", typeof(Thickness), typeof(Border),
                                          new FrameworkPropertyMetadata(
                                                new Thickness(),
                                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender),
                                          new ValidateValueCallback(IsThicknessValid));
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty)!; }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty
            = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Border),
                                          new FrameworkPropertyMetadata(
                                                new CornerRadius(),
                                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender),
                                          new ValidateValueCallback(IsCornerRadiusValid));
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty)!; }
            set { SetValue(CornerRadiusProperty, value); }
        }

        private static bool IsCornerRadiusValid(object? value)
        {
            CornerRadius cr = (CornerRadius)value!;
            return (cr.IsValid(false, false, false, false));
        }

        public static readonly DependencyProperty BorderBrushProperty
            = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(Border),
                                          new FrameworkPropertyMetadata(
                                                null,
                                                FrameworkPropertyMetadataOptions.AffectsRender |
                                                FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
                                                new PropertyChangedCallback(OnClearPenCache)));
        public Brush? BorderBrush
        {
            get { return (Brush?)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty =
                Panel.BackgroundProperty.AddOwner(typeof(Border),
                        new FrameworkPropertyMetadata(
                                null,
                                FrameworkPropertyMetadataOptions.AffectsRender |
                                FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));
        public Brush? Background
        {
            get { return (Brush?)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        #endregion

        #region Layout

        private bool _useComplexRenderCodePath;

        protected override Size MeasureOverride(Size constraint)
        {
            UIElement? child = Child;
            Thickness borders = BorderThickness;
            if (this.UseLayoutRounding)
            {
                DpiScale dpi = GetDpi();
                borders = new Thickness(RoundLayoutValue(borders.Left, dpi.DpiScaleX), RoundLayoutValue(borders.Top, dpi.DpiScaleY),
                   RoundLayoutValue(borders.Right, dpi.DpiScaleX), RoundLayoutValue(borders.Bottom, dpi.DpiScaleY));
            }
            // Compute the chrome size added by the various elements
            Size border = HelperCollapseThickness(borders);
            Size padding = HelperCollapseThickness(Padding);

            //If we have a child
            if (child != null)
            {
                // Combine into total decorating size
                Size combined = new Size(border.Width + padding.Width, border.Height + padding.Height);

                // Remove size of border only from child's reference size.
                Size childConstraint = new Size(Math.Max(0.0f, constraint.Width - combined.Width),
                                                Math.Max(0.0f, constraint.Height - combined.Height));


                child.Measure(childConstraint);
                Size childSize = child.DesiredSize;

                // Now use the returned size to drive our size, by adding back the margins, etc.
                return new Size(childSize.Width + combined.Width, childSize.Height + combined.Height);
            }
            else
            {
                // Combine into total decorating size
                return new Size(border.Width + padding.Width, border.Height + padding.Height);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Thickness borders = BorderThickness;
            if (UseLayoutRounding)
            {
                DpiScale dpi = GetDpi();
                borders = new Thickness(RoundLayoutValue(borders.Left, dpi.DpiScaleX), RoundLayoutValue(borders.Top, dpi.DpiScaleY),
                   RoundLayoutValue(borders.Right, dpi.DpiScaleX), RoundLayoutValue(borders.Bottom, dpi.DpiScaleY));
            }
            Rect boundRect = new Rect(finalSize);
            Rect innerRect = HelperDeflateRect(boundRect, borders);

            //  arrange child
            UIElement? child = Child;
            if (child != null)
            {
                Rect childRect = HelperDeflateRect(innerRect, Padding);
                child.Arrange(childRect);
            }

            CornerRadius radii = CornerRadius;
            Brush? borderBrush = BorderBrush;
            bool uniformCorners = AreUniformCorners(radii);

            //  decide which code path to execute. complex (geometry path based) rendering 
            //  is used if one of the following is true:

            //  1. there are non-uniform rounded corners
            _useComplexRenderCodePath = !uniformCorners;

            if (!_useComplexRenderCodePath && borderBrush != null)
            {
                SolidColorBrush? originIndependentBrush = borderBrush as SolidColorBrush;

                bool uniformBorders = borders.IsUniform;

                _useComplexRenderCodePath =
                        //  2. the border brush is origin dependent (the only origin independent brush is a solid color brush)
                        (originIndependentBrush == null)
                //  3. the border brush is semi-transtarent solid color brush AND border thickness is not uniform
                //     (for uniform semi-transparent border Border.OnRender draws rectangle outline - so it works fine)
                    || ((originIndependentBrush.Color.A < 0xff) && !uniformBorders)
                //  4. there are rounded corners AND the border thickness is not uniform
                    || (!FloatUtil.IsZero(radii.TopLeft) && !uniformBorders);
            }

            if (_useComplexRenderCodePath)
            {
                Radii innerRadii = new Radii(radii, borders, false);

                StreamGeometry? backgroundGeometry = null;

                //  calculate border / background rendering geometry
                if (!FloatUtil.IsZero(innerRect.Width) && !FloatUtil.IsZero(innerRect.Height))
                {
                    backgroundGeometry = new StreamGeometry();

                    StreamGeometryContext ctx = backgroundGeometry.Open();
                    GenerateGeometry(ctx, innerRect, innerRadii);

                    backgroundGeometry.Freeze();
                    _backgroundGeometryCache = backgroundGeometry;
                }
                else
                {
                    _backgroundGeometryCache = null;
                }

                if (!FloatUtil.IsZero(boundRect.Width) && !FloatUtil.IsZero(boundRect.Height))
                {
                    Radii outerRadii = new Radii(radii, borders, true);
                    StreamGeometry borderGeometry = new StreamGeometry();

                    StreamGeometryContext ctx = borderGeometry.Open();
                    GenerateGeometry(ctx, boundRect, outerRadii);
                    if (backgroundGeometry != null)
                    {
                        GenerateGeometry(ctx, innerRect, innerRadii);
                    }

                    borderGeometry.Freeze();
                    _borderGeometryCache = borderGeometry;
                }
                else
                {
                    _borderGeometryCache = null;
                }
            }
            else
            {
                _backgroundGeometryCache = null;
                _borderGeometryCache = null;
            }

            return (finalSize);
        }

        private StreamGeometry? _borderGeometryCache, _backgroundGeometryCache;
        private Pen? _leftPenCache, _rightPenCache, _topPenCache, _bottomPenCache;
        protected override void OnRender(DrawingContext dc)
        {
            bool useLayoutRounding = UseLayoutRounding;
            DpiScale dpi = GetDpi();

            if (_useComplexRenderCodePath)
            {
                Brush? brush;
                StreamGeometry? borderGeometry = _borderGeometryCache;
                if (borderGeometry != null
                    && (brush = BorderBrush) != null)
                {
                    dc.DrawGeometry(brush, null, borderGeometry);
                }

                StreamGeometry? backgroundGeometry = _backgroundGeometryCache;
                if (backgroundGeometry != null
                    && (brush = Background) != null)
                {
                    dc.DrawGeometry(brush, null, backgroundGeometry);
                }
            }
            else
            {
                Thickness border = BorderThickness;
                Brush? borderBrush;

                CornerRadius cornerRadius = CornerRadius;
                float outerCornerRadius = cornerRadius.TopLeft; // Already validated that all corners have the same radius
                bool roundedCorners = !FloatUtil.IsZero(outerCornerRadius);

                // If we have a brush with which to draw the border, do so.
                // NB: We float draw corners right now.  Corner handling is tricky (bevelling, &c...) and
                //     we need a firm spec before doing "the right thing."  (greglett, ffortes)
                if (!border.IsZero && (borderBrush = BorderBrush) != null)
                {
                    // Initialize the first pen.  Note that each pen is created via new()
                    // and frozen if possible.  Doing this avoids the pen 
                    // being copied when used in the DrawLine methods.
                    Pen? pen = _leftPenCache;
                    if (pen == null)
                    {
                        pen = new Pen();
                        pen.Brush = borderBrush;

                        if (useLayoutRounding)
                        {
                            pen.Thickness = RoundLayoutValue(border.Left, dpi.DpiScaleX);
                        }
                        else
                        {
                            pen.Thickness = border.Left;
                        }
                        if (borderBrush.IsFrozen)
                        {
                            pen.Freeze();
                        }

                        _leftPenCache = pen;
                    }

                    float halfThickness;
                    if (border.IsUniform)
                    {
                        halfThickness = pen.Thickness * 0.5f;


                        // Create rect w/ border thickness, and round if applying layout rounding.
                        Rect rect = new Rect(new Point(halfThickness, halfThickness),
                                             new Point(RenderSize.Width - halfThickness, RenderSize.Height - halfThickness));

                        if (roundedCorners)
                        {
                            dc.DrawRoundedRectangle(
                                null,
                                pen,
                                rect,
                                outerCornerRadius,
                                outerCornerRadius);
                        }
                        else
                        {
                            dc.DrawRectangle(
                                null,
                                pen,
                                rect);
                        }
                    }
                    else
                    {
                        // Nonuniform border; stroke each edge.
                        if (border.Left >= 0)
                        {
                            halfThickness = pen.Thickness * 0.5f;
                            dc.DrawLine(
                                pen,
                                new Point(halfThickness, 0),
                                new Point(halfThickness, RenderSize.Height));
                        }

                        if (border.Right >= 0)
                        {
                            pen = _rightPenCache;
                            if (pen == null)
                            {
                                pen = new Pen();
                                pen.Brush = borderBrush;

                                //if (useLayoutRounding)
                                //{
                                //    pen.Thickness = RoundLayoutValue(border.Right, dpi.DpiScaleX);
                                //}
                                //else
                                //{
                                pen.Thickness = border.Right;
                                //}

                                if (borderBrush.IsFrozen)
                                {
                                    pen.Freeze();
                                }

                                _rightPenCache = pen;
                            }

                            halfThickness = pen.Thickness * 0.5f;
                            dc.DrawLine(
                                pen,
                                new Point(RenderSize.Width - halfThickness, 0),
                                new Point(RenderSize.Width - halfThickness, RenderSize.Height));
                        }

                        if (border.Top >= 0)
                        {
                            pen = _topPenCache;
                            if (pen == null)
                            {
                                pen = new Pen();
                                pen.Brush = borderBrush;
                                if (useLayoutRounding)
                                {
                                    pen.Thickness = RoundLayoutValue(border.Top, dpi.DpiScaleY);
                                }
                                else
                                {
                                    pen.Thickness = border.Top;
                                }

                                if (borderBrush.IsFrozen)
                                {
                                    pen.Freeze();
                                }

                                _topPenCache = pen;
                            }

                            halfThickness = pen.Thickness * 0.5f;
                            dc.DrawLine(
                                pen,
                                new Point(0, halfThickness),
                                new Point(RenderSize.Width, halfThickness));
                        }

                        if (border.Bottom >= 0)
                        {
                            pen = _bottomPenCache;
                            if (pen == null)
                            {
                                pen = new Pen();
                                pen.Brush = borderBrush;
                                if (useLayoutRounding)
                                {
                                    pen.Thickness = RoundLayoutValue(border.Bottom, dpi.DpiScaleY);
                                }
                                else
                                {
                                    pen.Thickness = border.Bottom;
                                }
                                if (borderBrush.IsFrozen)
                                {
                                    pen.Freeze();
                                }

                                _bottomPenCache = pen;
                            }

                            halfThickness = pen.Thickness * 0.5f;
                            dc.DrawLine(
                                pen,
                                new Point(0, RenderSize.Height - halfThickness),
                                new Point(RenderSize.Width, RenderSize.Height - halfThickness));
                        }
                    }
                }

                // Draw background in rectangle inside border.
                Brush? background = Background;
                if (background != null)
                {
                    // Intialize background 
                    Point ptTL, ptBR;

                    if (useLayoutRounding)
                    {
                        ptTL = new Point(RoundLayoutValue(border.Left, dpi.DpiScaleX),
                                         RoundLayoutValue(border.Top, dpi.DpiScaleY));

                        //if (FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
                        //{
                        //    ptBR = new Point(RoundLayoutValue(RenderSize.Width - border.Right, dpi.DpiScaleX),
                        //                 RoundLayoutValue(RenderSize.Height - border.Bottom, dpi.DpiScaleY));
                        //}
                        //else
                        //{
                        ptBR = new Point(RenderSize.Width - RoundLayoutValue(border.Right, dpi.DpiScaleX),
                                     RenderSize.Height - RoundLayoutValue(border.Bottom, dpi.DpiScaleY));
                        //}
                    }
                    else
                    {
                        ptTL = new Point(border.Left, border.Top);
                        ptBR = new Point(RenderSize.Width - border.Right, RenderSize.Height - border.Bottom);
                    }

                    // Do not draw background if the borders are so large that they overlap.
                    if (ptBR.X > ptTL.X && ptBR.Y > ptTL.Y)
                    {
                        if (roundedCorners)
                        {
                            Radii innerRadii = new Radii(cornerRadius, border, false); // Determine the inner edge radius
                            float innerCornerRadius = innerRadii.TopLeft;  // Already validated that all corners have the same radius
                            dc.DrawRoundedRectangle(background, null, new Rect(ptTL, ptBR), innerCornerRadius, innerCornerRadius);
                        }
                        else
                        {
                            dc.DrawRectangle(background, null, new Rect(ptTL, ptBR));
                        }
                    }
                }
            }
        }

        private static Size HelperCollapseThickness(Thickness th)
        {
            return new Size(th.Left + th.Right, th.Top + th.Bottom);
        }

        private static bool AreUniformCorners(CornerRadius borderRadii)
        {
            float topLeft = borderRadii.TopLeft;
            return FloatUtil.AreClose(topLeft, borderRadii.TopRight) &&
                FloatUtil.AreClose(topLeft, borderRadii.BottomLeft) &&
                FloatUtil.AreClose(topLeft, borderRadii.BottomRight);
        }

        /// Helper to deflate rectangle by thickness
        private static Rect HelperDeflateRect(Rect rt, Thickness thick)
        {
            return new Rect(rt.Left + thick.Left,
                            rt.Top + thick.Top,
                            Math.Max(0.0f, rt.Width - thick.Left - thick.Right),
                            Math.Max(0.0f, rt.Height - thick.Top - thick.Bottom));
        }


        private struct Radii
        {
            internal Radii(CornerRadius radii, Thickness borders, bool outer)
            {
                float left = 0.5f * borders.Left;
                float top = 0.5f * borders.Top;
                float right = 0.5f * borders.Right;
                float bottom = 0.5f * borders.Bottom;

                if (outer)
                {
                    if (FloatUtil.IsZero(radii.TopLeft))
                    {
                        LeftTop = TopLeft = 0.0f;
                    }
                    else
                    {
                        LeftTop = radii.TopLeft + left;
                        TopLeft = radii.TopLeft + top;
                    }
                    if (FloatUtil.IsZero(radii.TopRight))
                    {
                        TopRight = RightTop = 0.0f;
                    }
                    else
                    {
                        TopRight = radii.TopRight + top;
                        RightTop = radii.TopRight + right;
                    }
                    if (FloatUtil.IsZero(radii.BottomRight))
                    {
                        RightBottom = BottomRight = 0.0f;
                    }
                    else
                    {
                        RightBottom = radii.BottomRight + right;
                        BottomRight = radii.BottomRight + bottom;
                    }
                    if (FloatUtil.IsZero(radii.BottomLeft))
                    {
                        BottomLeft = LeftBottom = 0.0f;
                    }
                    else
                    {
                        BottomLeft = radii.BottomLeft + bottom;
                        LeftBottom = radii.BottomLeft + left;
                    }
                }
                else
                {
                    LeftTop = Math.Max(0.0f, radii.TopLeft - left);
                    TopLeft = Math.Max(0.0f, radii.TopLeft - top);
                    TopRight = Math.Max(0.0f, radii.TopRight - top);
                    RightTop = Math.Max(0.0f, radii.TopRight - right);
                    RightBottom = Math.Max(0.0f, radii.BottomRight - right);
                    BottomRight = Math.Max(0.0f, radii.BottomRight - bottom);
                    BottomLeft = Math.Max(0.0f, radii.BottomLeft - bottom);
                    LeftBottom = Math.Max(0.0f, radii.BottomLeft - left);
                }
            }

            internal float LeftTop;
            internal float TopLeft;
            internal float TopRight;
            internal float RightTop;
            internal float RightBottom;
            internal float BottomRight;
            internal float BottomLeft;
            internal float LeftBottom;
        }

        private static void GenerateGeometry(StreamGeometryContext ctx, Rect rect, Radii radii)
        {
            //
            //  compute the coordinates of the key points
            //

            Point topLeft = new Point(radii.LeftTop, 0);
            Point topRight = new Point(rect.Width - radii.RightTop, 0);
            Point rightTop = new Point(rect.Width, radii.TopRight);
            Point rightBottom = new Point(rect.Width, rect.Height - radii.BottomRight);
            Point bottomRight = new Point(rect.Width - radii.RightBottom, rect.Height);
            Point bottomLeft = new Point(radii.LeftBottom, rect.Height);
            Point leftBottom = new Point(0, rect.Height - radii.BottomLeft);
            Point leftTop = new Point(0, radii.TopLeft);

            //
            //  check keypoints for overlap and resolve by partitioning radii according to
            //  the percentage of each one.  
            //

            //  top edge is handled here
            if (topLeft.X > topRight.X)
            {
                float v = (radii.LeftTop) / (radii.LeftTop + radii.RightTop) * rect.Width;
                topLeft.X = v;
                topRight.X = v;
            }

            //  right edge
            if (rightTop.Y > rightBottom.Y)
            {
                float v = (radii.TopRight) / (radii.TopRight + radii.BottomRight) * rect.Height;
                rightTop.Y = v;
                rightBottom.Y = v;
            }

            //  bottom edge
            if (bottomRight.X < bottomLeft.X)
            {
                float v = (radii.LeftBottom) / (radii.LeftBottom + radii.RightBottom) * rect.Width;
                bottomRight.X = v;
                bottomLeft.X = v;
            }

            // left edge
            if (leftBottom.Y < leftTop.Y)
            {
                float v = (radii.TopLeft) / (radii.TopLeft + radii.BottomLeft) * rect.Height;
                leftBottom.Y = v;
                leftTop.Y = v;
            }

            //
            //  add on offsets
            //

            Vector2 offset = new Vector2(rect.TopLeft.X, rect.TopLeft.Y);
            topLeft += offset;
            topRight += offset;
            rightTop += offset;
            rightBottom += offset;
            bottomRight += offset;
            bottomLeft += offset;
            leftBottom += offset;
            leftTop += offset;

            //
            //  create the border geometry
            //
            ctx.BeginFigure(topLeft, true /* is filled */, true /* is closed */);

            // Top line
            ctx.LineTo(topRight, true /* is stroked */, false /* is smooth join */);

            // Upper-right corner
            float radiusX = rect.TopRight.X - topRight.X;
            float radiusY = rightTop.Y - rect.TopRight.Y;
            if (!FloatUtil.IsZero(radiusX)
                || !FloatUtil.IsZero(radiusY))
            {
                ctx.ArcTo(rightTop, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }

            // Right line
            ctx.LineTo(rightBottom, true /* is stroked */, false /* is smooth join */);

            // Lower-right corner
            radiusX = rect.BottomRight.X - bottomRight.X;
            radiusY = rect.BottomRight.Y - rightBottom.Y;
            if (!FloatUtil.IsZero(radiusX)
                || !FloatUtil.IsZero(radiusY))
            {
                ctx.ArcTo(bottomRight, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }

            // Bottom line
            ctx.LineTo(bottomLeft, true /* is stroked */, false /* is smooth join */);

            // Lower-left corner
            radiusX = bottomLeft.X - rect.BottomLeft.X;
            radiusY = rect.BottomLeft.Y - leftBottom.Y;
            if (!FloatUtil.IsZero(radiusX)
                || !FloatUtil.IsZero(radiusY))
            {
                ctx.ArcTo(leftBottom, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }

            // Left line
            ctx.LineTo(leftTop, true /* is stroked */, false /* is smooth join */);

            // Upper-left corner
            radiusX = topLeft.X - rect.TopLeft.X;
            radiusY = leftTop.Y - rect.TopLeft.Y;
            if (!FloatUtil.IsZero(radiusX)
                || !FloatUtil.IsZero(radiusY))
            {
                ctx.ArcTo(topLeft, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise, true, false);
            }
        }

        protected override int EffectiveValuesInitialSize => 9;

        #endregion
    }
}
