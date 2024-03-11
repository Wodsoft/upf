using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class StackPanel : Panel
    {
        public static readonly DependencyProperty OrientationProperty =
                DependencyProperty.Register(
                        "Orientation",
                        typeof(Orientation),
                        typeof(StackPanel),
                        new FrameworkPropertyMetadata(
                                Orientation.Vertical,
                                FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(ScrollBar.IsValidOrientation));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty)!; }
            set { SetValue(OrientationProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size desiredSize = new Size(), elementSize = new Size();
            bool isHorizontal = Orientation == Orientation.Horizontal;
            if (isHorizontal)
            {
                elementSize.Width = float.PositiveInfinity;
                elementSize.Height = availableSize.Height;
            }
            else
            {
                elementSize.Width = availableSize.Width;
                elementSize.Height = float.PositiveInfinity;
            }
            var children = Children;
            for (int i = 0; i < children.Count; i++)
            {
                UIElement? child = children[i];
                if (child == null)
                    continue;
                child.Measure(elementSize);
                var childSize = child.DesiredSize;
                if (isHorizontal)
                {
                    desiredSize.Width += childSize.Width;
                    if (childSize.Height > desiredSize.Height)
                        desiredSize.Height = childSize.Height;
                }
                else
                {
                    desiredSize.Height += childSize.Height;
                    if (childSize.Width > desiredSize.Width)
                        desiredSize.Width = childSize.Width;
                }
            }
            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var childRect = new Rect(finalSize);
            bool isHorizontal = Orientation == Orientation.Horizontal;
            var children = Children;

            for (int i = 0; i < children.Count; i++)
            {
                UIElement? child = children[i];
                if (child == null)
                    continue;
                float size;
                if (isHorizontal)
                {
                    size = child.DesiredSize.Width;
                    childRect.Width = size;
                    childRect.Height = Math.Max(finalSize.Height, child.DesiredSize.Height);
                }
                else
                {
                    size = child.DesiredSize.Height;
                    childRect.Height = size;
                    childRect.Width = Math.Max(finalSize.Width, child.DesiredSize.Width);
                }
                child.Arrange(childRect);
                if (isHorizontal)
                {
                    childRect.X += size;
                }
                else
                {
                    childRect.Y += size;
                }
            }
            return finalSize;
        }
    }
}
