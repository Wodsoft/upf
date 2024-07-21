using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Animation;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Media
{
    /// <summary>
    /// A collection of Transform objects.
    /// </summary>
    public sealed class TransformCollection : AnimatableCollection<Transform>
    {
        #region Empty

        private static TransformCollection? _Empty;
        public static TransformCollection Empty
        {
            get
            {
                if (_Empty == null)
                {
                    _Empty = new TransformCollection();
                    _Empty.Freeze();
                }
                return _Empty;
            }
        }

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new TransformCollection();
        }

        #endregion
    }
}
