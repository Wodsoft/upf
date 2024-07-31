using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class QuaternionKeyFrameCollection : KeyFrameCollection<QuaternionKeyFrame>, IHaveEmpty<QuaternionKeyFrameCollection>
    {
        #region Data

        private static QuaternionKeyFrameCollection? _EmptyCollection;

        #endregion

        public QuaternionKeyFrameCollection()
        {
        }

        protected QuaternionKeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty QuaternionKeyFrameCollection.
        /// </summary>
        public static QuaternionKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    QuaternionKeyFrameCollection emptyCollection = new QuaternionKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this QuaternionKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new QuaternionKeyFrameCollection Clone()
        {
            return (QuaternionKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new QuaternionKeyFrameCollection();
        }
    }
}
