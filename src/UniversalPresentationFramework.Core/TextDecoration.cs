using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI
{
    public class TextDecoration : Animatable
    {
        #region Constructors

        public TextDecoration()
        {
        }

        public TextDecoration(
            TextDecorationLocation location,
            Pen pen,
            float penOffset,
            TextDecorationUnit penOffsetUnit,
            TextDecorationUnit penThicknessUnit
            )
        {
            Location = location;
            Pen = pen;
            PenOffset = penOffset;
            PenOffsetUnit = penOffsetUnit;
            PenThicknessUnit = penThicknessUnit;
        }

        #endregion

        #region Clone

        protected override Freezable CreateInstanceCore()
        {
            return new TextDecoration();
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty PenProperty = DependencyProperty.Register("Pen", typeof(Pen), typeof(TextDecoration));
        /// <summary>
        ///     Pen - Pen.  Default value is null.
        ///     The pen used to draw the text decoration
        /// </summary>
        public Pen? Pen
        {
            get
            {
                return (Pen?)GetValue(PenProperty);
            }
            set
            {
                SetValue(PenProperty, value);
            }
        }

        public static readonly DependencyProperty PenOffsetProperty = DependencyProperty.Register("PenOffset", typeof(float), typeof(TextDecoration), new PropertyMetadata(0f));
        /// <summary>
        ///     PenOffset - double.  Default value is 0.0.
        ///     The offset of the text decoration to the location specified.
        /// </summary>
        public float PenOffset
        {
            get
            {
                return (float)GetValue(PenOffsetProperty)!;
            }
            set
            {
                SetValue(PenOffsetProperty, value);
            }
        }

        public static readonly DependencyProperty PenOffsetUnitProperty = DependencyProperty.Register("PenOffsetUnit", typeof(TextDecorationUnit), typeof(TextDecoration), new PropertyMetadata(TextDecorationUnit.FontRecommended));
        /// <summary>
        ///     PenOffsetUnit - TextDecorationUnit.  Default value is TextDecorationUnit.FontRecommended.
        ///     The unit type we use to interpret the offset value.
        /// </summary>
        public TextDecorationUnit PenOffsetUnit
        {
            get
            {
                return (TextDecorationUnit)GetValue(PenOffsetUnitProperty)!;
            }
            set
            {
                SetValue(PenOffsetUnitProperty, value);
            }
        }

        public static readonly DependencyProperty PenThicknessUnitProperty = DependencyProperty.Register("PenThicknessUnit", typeof(TextDecorationUnit), typeof(TextDecoration), new PropertyMetadata(TextDecorationUnit.FontRecommended));
        /// <summary>
        ///     PenThicknessUnit - TextDecorationUnit.  Default value is TextDecorationUnit.FontRecommended.
        ///     The unit type we use to interpret the thickness value.
        /// </summary>
        public TextDecorationUnit PenThicknessUnit
        {
            get
            {
                return (TextDecorationUnit)GetValue(PenThicknessUnitProperty)!;
            }
            set
            {
                SetValue(PenThicknessUnitProperty, value);
            }
        }

        public static readonly DependencyProperty LocationProperty = DependencyProperty.Register("Location", typeof(TextDecorationLocation), typeof(TextDecoration), new PropertyMetadata(TextDecorationLocation.Underline));
        /// <summary>
        ///     Location - TextDecorationLocation.  Default value is TextDecorationLocation.Underline.
        ///     The Location of the text decorations
        /// </summary>
        public TextDecorationLocation Location
        {
            get
            {
                return (TextDecorationLocation)GetValue(LocationProperty)!;
            }
            set
            {
                SetValue(LocationProperty, value);
            }
        }

        #endregion

        internal bool ValueEquals(TextDecoration textDecoration)
        {
            if (textDecoration == null)
                return false; // o is either null or not a TextDecoration object.

            if (this == textDecoration)
                return true; // reference equality.

            return (
               Location == textDecoration.Location
            && PenOffset == textDecoration.PenOffset
            && PenOffsetUnit == textDecoration.PenOffsetUnit
            && PenThicknessUnit == textDecoration.PenThicknessUnit
            && (Pen == null ? textDecoration.Pen == null : Pen.Equals(textDecoration.Pen))
            );
        }
    }
}
