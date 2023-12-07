using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class ColumnDefinition : DefinitionBase
    {
        #region Properties

        public float ActualWidth { get; internal set; }

        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register(
                "Width",
                typeof(GridLength),
                typeof(ColumnDefinition),
                new FrameworkPropertyMetadata(
                        new GridLength(1.0f, GridUnitType.Star),
                        new PropertyChangedCallback(OnSizePropertyChanged)),
                new ValidateValueCallback(IsUserSizePropertyValueValid));
        private static bool IsUserSizePropertyValueValid(object? value)
        {
            if (value is GridLength v)
                return v.Value >= 0f;
            return false;
        }
        public GridLength Width { get => (GridLength)GetValue(WidthProperty)!; set => SetValue(WidthProperty, value); }

        public static readonly DependencyProperty MinWidthProperty =
            DependencyProperty.Register(
                "MinWidth",
                typeof(float),
                typeof(ColumnDefinition),
                new FrameworkPropertyMetadata(
                        0f,
                        new PropertyChangedCallback(OnSizePropertyChanged)),
                new ValidateValueCallback(IsUserMinSizePropertyValueValid));
        private static bool IsUserMinSizePropertyValueValid(object? value)
        {
            if (value is float v)
                return (!float.IsNaN(v) && v >= 0.0f && !float.IsPositiveInfinity(v));
            return false;
        }
        public float MinWidth { get => (float)GetValue(MinWidthProperty)!; set => SetValue(MinWidthProperty, value); }

        public static readonly DependencyProperty MaxWidthProperty =
            DependencyProperty.Register(
                "MaxWidth",
                typeof(float),
                typeof(ColumnDefinition),
                new FrameworkPropertyMetadata(
                        float.PositiveInfinity,
                        new PropertyChangedCallback(OnSizePropertyChanged)),
                new ValidateValueCallback(IsUserMaxSizePropertyValueValid));
        private static bool IsUserMaxSizePropertyValueValid(object? value)
        {
            if (value is float v)
                return (!float.IsNaN(v) && v >= 0.0f);
            return false;
        }
        public float MaxWidth { get => (float)GetValue(MaxWidthProperty)!; set => SetValue(MaxWidthProperty, value); }

        internal static void OnSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DefinitionBase definition = (DefinitionBase)d;
            if (definition.Grid != null)
            {
                if (!definition.Grid.IsInitPending)
                    definition.Grid.CalculateColumn();
                definition.Grid.InvalidateMeasure();
            }
        }

        #endregion

    }
}
