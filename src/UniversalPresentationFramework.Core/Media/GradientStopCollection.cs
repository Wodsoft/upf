using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI.Media
{
    public class GradientStopCollection : AnimatableCollection<GradientStop>, IHaveEmpty<GradientStopCollection>
    {
        /// <summary>
        ///     Shadows inherited Clone() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new GradientStopCollection Clone()
        {
            return (GradientStopCollection)base.Clone();
        }

        /// <summary>
        ///     Shadows inherited CloneCurrentValue() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new GradientStopCollection CloneCurrentValue()
        {
            return (GradientStopCollection)base.CloneCurrentValue();
        }

        protected override Freezable CreateInstanceCore()
        {
            return new GradientStopCollection();
        }

        private static GradientStopCollection? _Empty;
        public static GradientStopCollection Empty
        {
            get
            {
                if (_Empty == null)
                {
                    GradientStopCollection collection = new GradientStopCollection();
                    collection.Freeze();
                    _Empty = collection;
                }
                return _Empty;
            }
        }
    }
}
