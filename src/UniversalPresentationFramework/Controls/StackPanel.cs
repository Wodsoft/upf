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
                    desiredSize.Width += elementSize.Width;
                    if (elementSize.Height > desiredSize.Height)
                        desiredSize.Height = elementSize.Height;
                }
                else
                {
                    desiredSize.Height += elementSize.Height;
                    if (elementSize.Width > desiredSize.Width)
                        desiredSize.Width = elementSize.Width;
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
                child.Arrange(childRect);
                if (isHorizontal)
                {
                    var value = child.DesiredSize.Width;
                    childRect.X += value;
                    if (childRect.Width > value)
                        childRect.Width -= value;
                    else
                        childRect.Width = 0;
                }
                else
                {
                    var value = child.DesiredSize.Width;
                    childRect.Y += value;
                    if (childRect.Height > value)
                        childRect.Height -= value;
                    else
                        childRect.Height = 0;
                }
            }
            return finalSize;
        }
    }
}
