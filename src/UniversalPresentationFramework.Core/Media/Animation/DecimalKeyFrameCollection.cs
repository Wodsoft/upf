using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class DecimalKeyFrameCollection : KeyFrameCollection<DecimalKeyFrame>, IHaveEmpty<DecimalKeyFrameCollection>
    {
        #region Data

        private static DecimalKeyFrameCollection? _EmptyCollection;

        #endregion

        public DecimalKeyFrameCollection()
        {
        }

        protected DecimalKeyFrameCollection(int capacity) : base(capacity)
        {
        }

        #region Static Methods

        /// <summary>
        /// An empty DecimalKeyFrameCollection.
        /// </summary>
        public static DecimalKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    DecimalKeyFrameCollection emptyCollection = new DecimalKeyFrameCollection(0);
                    emptyCollection.Freeze();

                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        /// <summary>
        /// Creates a freezable copy of this DecimalKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new DecimalKeyFrameCollection Clone()
        {
            return (DecimalKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new DecimalKeyFrameCollection();
        }
    }
}
