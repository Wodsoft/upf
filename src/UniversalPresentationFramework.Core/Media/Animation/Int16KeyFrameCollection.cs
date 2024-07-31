using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class Int16KeyFrameCollection : KeyFrameCollection<Int16KeyFrame>, IHaveEmpty<Int16KeyFrameCollection>
    {
        #region Data

        private static Int16KeyFrameCollection? _EmptyCollection;

        #endregion

        public Int16KeyFrameCollection()
        {
        }

        protected Int16KeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty Int16KeyFrameCollection.
        /// </summary>
        public static Int16KeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    Int16KeyFrameCollection emptyCollection = new Int16KeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this Int16KeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new Int16KeyFrameCollection Clone()
        {
            return (Int16KeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new Int16KeyFrameCollection();
        }
    }
}
