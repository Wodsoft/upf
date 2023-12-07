using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class RowDefinition : DefinitionBase
    {
        #region Properties

        public float ActualHeight { get; internal set; }

        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register(
                "Height",
                typeof(GridLength),
                typeof(RowDefinition),
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
        public GridLength Height { get => (GridLength)GetValue(HeightProperty)!; set => SetValue(HeightProperty, value); }

        public static readonly DependencyProperty MinHeightProperty =
            DependencyProperty.Register(
                "MinHeight",
                typeof(float),
                typeof(RowDefinition),
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
        public float MinHeight { get => (float)GetValue(MinHeightProperty)!; set => SetValue(MinHeightProperty, value); }

        public static readonly DependencyProperty MaxHeightProperty =
            DependencyProperty.Register(
                "MaxHeight",
                typeof(float),
                typeof(RowDefinition),
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
        public float MaxHeight { get => (float)GetValue(MaxHeightProperty)!; set => SetValue(MaxHeightProperty, value); }

        internal static void OnSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DefinitionBase definition = (DefinitionBase)d;
            if (definition.Grid != null)
            {
                if (!definition.Grid.IsInitPending)
                    definition.Grid.CalculateRow();
                definition.Grid.InvalidateMeasure();
            }
        }

        #endregion
    }
}
