using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class CharKeyFrameCollection : KeyFrameCollection<CharKeyFrame>, IHaveEmpty<CharKeyFrameCollection>
    {
        #region Data

        private static CharKeyFrameCollection? _EmptyCollection;

        #endregion

        public CharKeyFrameCollection()
        {
        }

        private CharKeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty CharKeyFrameCollection.
        /// </summary>
        public static CharKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    CharKeyFrameCollection emptyCollection = new CharKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this CharKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new CharKeyFrameCollection Clone()
        {
            return (CharKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new CharKeyFrameCollection();
        }
    }
}
