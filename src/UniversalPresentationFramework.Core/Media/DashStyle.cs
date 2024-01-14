using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public class DashStyle : Freezable
    {
        #region Constructors

        /// <summary>
        ///     Default constructor
        /// </summary>
        public DashStyle()
        {
        }

        /// <summary>
        ///     Constructor from an array and offset
        /// <param name="dashes">
        ///     The array of lengths of dashes and gaps, measured in Thickness units.
        ///     If the value of dashes is null then the style will be solid
        /// </param>
        /// <param name="offset">
        ///     Determines where in the dash sequence the stroke will start
        /// </param>
        /// </summary>
        public DashStyle(IEnumerable<float> dashes, float offset)
        {
            Offset = offset;

            if (dashes != null)
            {
                Dashes = new FloatCollection(dashes);
            }
        }


        #endregion Constructors

        #region Properties

        public static readonly DependencyProperty OffsetProperty =
                  DependencyProperty.Register("Offset",
                                   typeof(float),
                                   typeof(DashStyle),
                                   new PropertyMetadata(0f));
        public float Offset { get { return (float)GetValue(OffsetProperty)!; } set { SetValue(OffsetProperty, value); } }

        public static readonly DependencyProperty DashesProperty =
                  DependencyProperty.Register("Dashes",
                                   typeof(FloatCollection),
                                   typeof(DashStyle),
                                   new PropertyMetadata(FloatCollection.Empty));
        public FloatCollection Dashes { get { return (FloatCollection)GetValue(DashesProperty)!; } set { SetValue(DashesProperty, value); } }

        #endregion

        #region Clone

        public new DashStyle Clone()
        {
            return (DashStyle)base.Clone();
        }

        public new DashStyle CloneCurrentValue()
        {
            return (DashStyle)base.CloneCurrentValue();
        }

        protected override Freezable CreateInstanceCore()
        {
            return new DashStyle();
        }

        #endregion
    }
}
