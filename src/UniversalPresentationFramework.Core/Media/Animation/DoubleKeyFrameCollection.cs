using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class DoubleKeyFrameCollection : KeyFrameCollection<DoubleKeyFrame>, IHaveEmpty<DoubleKeyFrameCollection>
    {
        #region Data

        private static DoubleKeyFrameCollection? _EmptyCollection;

        #endregion

        public DoubleKeyFrameCollection()
        {
        }

        protected DoubleKeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty DoubleKeyFrameCollection.
        /// </summary>
        public static DoubleKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    DoubleKeyFrameCollection emptyCollection = new DoubleKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this DoubleKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new DoubleKeyFrameCollection Clone()
        {
            return (DoubleKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new DoubleKeyFrameCollection();
        }
    }
}
