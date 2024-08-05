using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public class ImageBrush : TileBrush
    {
        #region Constructors

        /// <summary>
        /// Default constructor for ImageBrush.  The resulting Brush has no content.
        /// </summary>
        public ImageBrush()
        {
            // We do this so that the property, when read, is consistent - not that
            // this will every actually affect drawing.
        }

        /// <summary>
        /// ImageBrush Constructor where the image is set to the parameter's value
        /// </summary>
        /// <param name="image"> The image source. </param>
        public ImageBrush(ImageSource image)
        {
            ImageSource = image;
        }

        #endregion Constructors

        #region Clone

        /// <summary>
        ///     Shadows inherited Clone() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new ImageBrush Clone()
        {
            return (ImageBrush)base.Clone();
        }

        /// <summary>
        ///     Shadows inherited CloneCurrentValue() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new ImageBrush CloneCurrentValue()
        {
            return (ImageBrush)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new ImageBrush();
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty ImageSourceProperty = RegisterProperty("ImageSource",
                                   typeof(ImageSource),
                                   typeof(ImageBrush),
                                   null,
                                   new PropertyChangedCallback(ImageSourcePropertyChanged),
                                   null,
                                   /* isIndependentlyAnimated  = */ false,
                                   /* coerceValueCallback */ null);
        private static void ImageSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        public ImageSource? ImageSource
        {
            get { return (ImageSource?)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        #endregion

        #region Methods

        ///// <summary>
        ///// Obtains the current bounds of the brush's content
        ///// </summary>
        ///// <param name="contentBounds"> Output bounds of content </param>            
        //protected override void GetContentBounds(out Rect contentBounds)
        //{
        //    contentBounds = Rect.Empty;
        //    DrawingImage di = ImageSource as DrawingImage;
        //    if (di != null)
        //    {
        //        Drawing drawing = di.Drawing;
        //        if (drawing != null)
        //        {
        //            contentBounds = drawing.Bounds;
        //        }
        //    }
        //}

        #endregion      
    }
}
