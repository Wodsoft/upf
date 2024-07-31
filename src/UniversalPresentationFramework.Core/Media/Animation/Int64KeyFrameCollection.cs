using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class Int64KeyFrameCollection : KeyFrameCollection<Int64KeyFrame>, IHaveEmpty<Int64KeyFrameCollection>
    {
        #region Data

        private static Int64KeyFrameCollection? _EmptyCollection;

        #endregion

        public Int64KeyFrameCollection()
        {
        }

        protected Int64KeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty Int64KeyFrameCollection.
        /// </summary>
        public static Int64KeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    Int64KeyFrameCollection emptyCollection = new Int64KeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this Int64KeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new Int64KeyFrameCollection Clone()
        {
            return (Int64KeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new Int64KeyFrameCollection();
        }
    }
}
