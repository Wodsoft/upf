using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class ByteKeyFrameCollection : KeyFrameCollection<ByteKeyFrame>, IHaveEmpty<ByteKeyFrameCollection>
    {
        #region Data

        private static ByteKeyFrameCollection? _EmptyCollection;

        #endregion

        #region Constructors

        /// <Summary>
        /// Creates a new ByteKeyFrameCollection.
        /// </Summary>
        public ByteKeyFrameCollection()
            : base()
        {

        }

        protected ByteKeyFrameCollection(int capacity) : base(capacity) { }

        #endregion

        #region Static Methods

        /// <summary>
        /// An empty ByteKeyFrameCollection.
        /// </summary>
        public static ByteKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    ByteKeyFrameCollection emptyCollection = new ByteKeyFrameCollection(0);
                    emptyCollection.Freeze();
                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        #region Freezable

        /// <summary>
        /// Creates a freezable copy of this ByteKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new ByteKeyFrameCollection Clone()
        {
            return (ByteKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new ByteKeyFrameCollection();
        }

        #endregion
    }
}
