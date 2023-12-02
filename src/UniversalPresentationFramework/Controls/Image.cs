using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Imaging;

namespace Wodsoft.UI.Controls
{
    public class Image : FrameworkElement
    {
        #region Properties

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                    "Source",
                    typeof(ImageSource),
                    typeof(Image),
                    new FrameworkPropertyMetadata(
                            null,
                            FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                            new PropertyChangedCallback(OnSourceChanged)));
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Image image = (Image)d;
            ImageSource? oldValue = (ImageSource?)e.OldValue;
            ImageSource? newValue = (ImageSource?)e.NewValue;

            //image.DetachBitmapSourceEvents();

            //BitmapSource newBitmapSource = newValue as BitmapSource;
            //if (newBitmapSource != null && newBitmapSource.CheckAccess() && !newBitmapSource.IsFrozen)
            //{
            //    image.AttachBitmapSourceEvents(newBitmapSource);
            //}
        }
        public ImageSource? Source { get { return (ImageSource?)GetValue(SourceProperty); } set { SetValue(SourceProperty, value); } }

        public static readonly DependencyProperty StretchProperty = Viewbox.StretchProperty.AddOwner(typeof(Image));
        public Stretch Stretch { get { return (Stretch)GetValue(StretchProperty)!; } set { SetValue(StretchProperty, value); } }

        public static readonly DependencyProperty StretchDirectionProperty = Viewbox.StretchDirectionProperty.AddOwner(typeof(Image));
        public StretchDirection StretchDirection { get { return (StretchDirection)GetValue(StretchDirectionProperty)!; } set { SetValue(StretchDirectionProperty, value); } }

        #endregion

        #region Layout

        protected override Size ArrangeOverride(Size finalSize)
        {
            var source = Source;
            if (source == null)
                return new Size();
            return CalculateSize(source, finalSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var source = Source;
            if (source == null)
                return new Size();
            return CalculateSize(source, availableSize);
        }

        private Size CalculateSize(ImageSource source, Size size)
        {
            var width = source.Width;
            var height = source.Height;
            Stretch stretch = Stretch;
            if ((!float.IsPositiveInfinity(size.Width) || !float.IsPositiveInfinity(size.Height)) && stretch != Stretch.None)
            {
                if (float.IsPositiveInfinity(size.Width))
                    return new Size(size.Height / height * width, size.Height);
                else if (float.IsPositiveInfinity(size.Height))
                    return new Size(size.Width, size.Width / width * height);
                else if (stretch == Stretch.Uniform)
                {
                    var imgAspect = width / height;
                    var sizeAspect = size.Width / size.Height;
                    if (imgAspect > sizeAspect)
                        return new Size(size.Width, size.Width / width * height);
                    else
                        return new Size(size.Height / height * width, size.Height);
                }
                else
                    return size;
            }
            return new Size(width, height);
        }

        #endregion

        protected override void OnRender(DrawingContext drawingContext)
        {
            var renderSize = RenderSize;
            if (renderSize.Width <= 0 || renderSize.Height <= 0)
                return;
            var source = Source;
            if (source == null)
                return;
            var stretch = Stretch;
            switch (stretch)
            {
                case Stretch.UniformToFill:
                    drawingContext.PushClip(new RectangleGeometry(new Rect(0, 0, renderSize.Width, renderSize.Height)));
                    if (source.Width / source.Height > renderSize.Width / renderSize.Height)
                    {
                        drawingContext.DrawImage(source, new Rect(0, 0, renderSize.Height / source.Height * source.Width, renderSize.Height));
                    }
                    else
                    {
                        drawingContext.DrawImage(source, new Rect(0, 0, renderSize.Width, renderSize.Width / source.Width * source.Height));
                    }
                    break;
                case Stretch.None:
                    if (renderSize.Width < source.Width || renderSize.Height < source.Height)
                        drawingContext.PushClip(new RectangleGeometry(new Rect(0, 0, MathF.Min(renderSize.Width, source.Width), MathF.Min(renderSize.Height, source.Height))));
                    drawingContext.DrawImage(source, new Rect(0, 0, source.Width, source.Height));
                    break;
                default:
                        drawingContext.DrawImage(source, new Rect(0, 0, renderSize.Width, renderSize.Height));
                    break;
            }
        }
    }
}
