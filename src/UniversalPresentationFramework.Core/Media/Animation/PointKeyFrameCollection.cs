using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class PointKeyFrameCollection : KeyFrameCollection<PointKeyFrame>, IHaveEmpty<PointKeyFrameCollection>
    {
        #region Data

        private static PointKeyFrameCollection? _EmptyCollection;

        #endregion

        public PointKeyFrameCollection()
        {
        }

        protected PointKeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty PointKeyFrameCollection.
        /// </summary>
        public static PointKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    PointKeyFrameCollection emptyCollection = new PointKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this PointKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new PointKeyFrameCollection Clone()
        {
            return (PointKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new PointKeyFrameCollection();
        }
    }
}
