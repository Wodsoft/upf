using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class ObjectKeyFrameCollection : KeyFrameCollection<ObjectKeyFrame>, IHaveEmpty<ObjectKeyFrameCollection>
    {
        #region Data

        private static ObjectKeyFrameCollection? _EmptyCollection;

        #endregion

        public ObjectKeyFrameCollection()
        {
        }

        protected ObjectKeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty ObjectKeyFrameCollection.
        /// </summary>
        public static ObjectKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    ObjectKeyFrameCollection emptyCollection = new ObjectKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this ObjectKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new ObjectKeyFrameCollection Clone()
        {
            return (ObjectKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new ObjectKeyFrameCollection();
        }
    }
}
