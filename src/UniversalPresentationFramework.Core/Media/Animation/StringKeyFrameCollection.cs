using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class StringKeyFrameCollection : KeyFrameCollection<StringKeyFrame>, IHaveEmpty<StringKeyFrameCollection>
    {
        #region Data

        private static StringKeyFrameCollection? _EmptyCollection;

        #endregion

        public StringKeyFrameCollection()
        {
        }

        protected StringKeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty StringKeyFrameCollection.
        /// </summary>
        public static StringKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    StringKeyFrameCollection emptyCollection = new StringKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this StringKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new StringKeyFrameCollection Clone()
        {
            return (StringKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new StringKeyFrameCollection();
        }
    }
}
