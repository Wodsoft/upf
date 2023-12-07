﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    [TypeConverter(typeof(BrushConverter))]
    public abstract class Brush : Freezable
    {
        static Brush()
        {

        }

        #region Methods

        internal static Brush? Parse(string value, ITypeDescriptorContext? context)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            Brush brush = Parsers.ParseBrush(value, CultureInfo.InvariantCulture, context);
            if (brush.CanFreeze)
                brush.Freeze();
            return brush;
        }


        #endregion

        #region Properties

        public static readonly DependencyProperty OpacityProperty =
                  DependencyProperty.Register("Opacity",
                                              typeof(double),
                                              typeof(Brush),
                                              new PropertyMetadata(1.0f));
        public float Opacity { get { return (float)GetValue(OpacityProperty)!; } set { SetValue(OpacityProperty, value); } }

        #endregion
    }
}
