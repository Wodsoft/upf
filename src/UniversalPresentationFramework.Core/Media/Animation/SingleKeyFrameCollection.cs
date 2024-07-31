using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class SingleKeyFrameCollection : KeyFrameCollection<SingleKeyFrame>, IHaveEmpty<SingleKeyFrameCollection>
    {
        #region Data

        private static SingleKeyFrameCollection? _EmptyCollection;

        #endregion

        public SingleKeyFrameCollection()
        {
        }

        public SingleKeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty SingleKeyFrameCollection.
        /// </summary>
        public static SingleKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    SingleKeyFrameCollection emptyCollection = new SingleKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this SingleKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new SingleKeyFrameCollection Clone()
        {
            return (SingleKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new SingleKeyFrameCollection();
        }
    }
}
