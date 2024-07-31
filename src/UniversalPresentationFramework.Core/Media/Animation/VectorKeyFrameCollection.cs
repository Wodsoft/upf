using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class VectorKeyFrameCollection : KeyFrameCollection<VectorKeyFrame>, IHaveEmpty<VectorKeyFrameCollection>
    {
        #region Data

        private static VectorKeyFrameCollection? _EmptyCollection;

        #endregion

        public VectorKeyFrameCollection()
        {
        }

        protected VectorKeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty VectorKeyFrameCollection.
        /// </summary>
        public static VectorKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    VectorKeyFrameCollection emptyCollection = new VectorKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this VectorKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new VectorKeyFrameCollection Clone()
        {
            return (VectorKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new VectorKeyFrameCollection();
        }
    }
}
