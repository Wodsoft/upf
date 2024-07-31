using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class MatrixKeyFrameCollection : KeyFrameCollection<MatrixKeyFrame>, IHaveEmpty<MatrixKeyFrameCollection>
    {
        #region Data

        private static MatrixKeyFrameCollection? _EmptyCollection;

        #endregion

        public MatrixKeyFrameCollection()
        {
        }

        protected MatrixKeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty MatrixKeyFrameCollection.
        /// </summary>
        public static MatrixKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    MatrixKeyFrameCollection emptyCollection = new MatrixKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this MatrixKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new MatrixKeyFrameCollection Clone()
        {
            return (MatrixKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new MatrixKeyFrameCollection();
        }
    }
}
