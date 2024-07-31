using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class SizeKeyFrameCollection : KeyFrameCollection<SizeKeyFrame>, IHaveEmpty<SizeKeyFrameCollection>
    {
        #region Data

        private static SizeKeyFrameCollection? _EmptyCollection;

        #endregion

        public SizeKeyFrameCollection()
        {
        }

        protected SizeKeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty SizeKeyFrameCollection.
        /// </summary>
        public static SizeKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    SizeKeyFrameCollection emptyCollection = new SizeKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this SizeKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new SizeKeyFrameCollection Clone()
        {
            return (SizeKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new SizeKeyFrameCollection();
        }
    }
}
