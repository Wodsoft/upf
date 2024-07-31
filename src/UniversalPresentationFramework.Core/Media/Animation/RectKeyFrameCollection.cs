using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class RectKeyFrameCollection : KeyFrameCollection<RectKeyFrame>, IHaveEmpty<RectKeyFrameCollection>
    {
        #region Data

        private static RectKeyFrameCollection? _EmptyCollection;

        #endregion

        public RectKeyFrameCollection()
        {
        }

        protected RectKeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty RectKeyFrameCollection.
        /// </summary>
        public static RectKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    RectKeyFrameCollection emptyCollection = new RectKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this RectKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new RectKeyFrameCollection Clone()
        {
            return (RectKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new RectKeyFrameCollection();
        }
    }
}
