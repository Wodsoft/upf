using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class ColorKeyFrameCollection : KeyFrameCollection<ColorKeyFrame>, IHaveEmpty<ColorKeyFrameCollection>
    {
        #region Data

        private static ColorKeyFrameCollection? _EmptyCollection;

        #endregion

        public ColorKeyFrameCollection()
        {
        }

        protected ColorKeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty ColorKeyFrameCollection.
        /// </summary>
        public static ColorKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    ColorKeyFrameCollection emptyCollection = new ColorKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this ColorKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new ColorKeyFrameCollection Clone()
        {
            return (ColorKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new ColorKeyFrameCollection();
        }
    }
}
