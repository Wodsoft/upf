using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class Int32KeyFrameCollection : KeyFrameCollection<Int32KeyFrame>, IHaveEmpty<Int32KeyFrameCollection>
    {
        #region Data

        private static Int32KeyFrameCollection? _EmptyCollection;

        #endregion

        public Int32KeyFrameCollection()
        {
        }

        protected Int32KeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty Int32KeyFrameCollection.
        /// </summary>
        public static Int32KeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    Int32KeyFrameCollection emptyCollection = new Int32KeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this Int32KeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new Int32KeyFrameCollection Clone()
        {
            return (Int32KeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new Int32KeyFrameCollection();
        }
    }
}
