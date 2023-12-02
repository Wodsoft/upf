using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    public class Viewbox : Decorator
    {
        #region Properties

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch",          // Property name
            typeof(Stretch),    // Property type
            typeof(Viewbox),    // Property owner
            new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure));
        public Stretch Stretch { get { return (Stretch)GetValue(StretchProperty)!; } set { SetValue(StretchProperty, value); } }

        public static readonly DependencyProperty StretchDirectionProperty = DependencyProperty.Register(
            "StretchDirection",         // Property name
            typeof(StretchDirection),   // Property type
            typeof(Viewbox),            // Property owner
            new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure));
        public StretchDirection StretchDirection { get { return (StretchDirection)GetValue(StretchDirectionProperty)!; } set { SetValue(StretchDirectionProperty, value); } }

        #endregion
    }
}
