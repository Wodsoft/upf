using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class Vector3DKeyFrameCollection : KeyFrameCollection<Vector3DKeyFrame>, IHaveEmpty<Vector3DKeyFrameCollection>
    {
        #region Data

        private static Vector3DKeyFrameCollection? _EmptyCollection;

        #endregion

        public Vector3DKeyFrameCollection()
        {
        }

        protected Vector3DKeyFrameCollection(int capacity) : base(capacity)
        {
        }


        #region Static Methods

        /// <summary>
        /// An empty Vector3DKeyFrameCollection.
        /// </summary>
        public static Vector3DKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    Vector3DKeyFrameCollection emptyCollection = new Vector3DKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this Vector3DKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new Vector3DKeyFrameCollection Clone()
        {
            return (Vector3DKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new Vector3DKeyFrameCollection();
        }
    }
}
