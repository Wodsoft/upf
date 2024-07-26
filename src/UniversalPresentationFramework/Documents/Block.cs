using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    public abstract class Block : TextElement
    {
        #region Properties

        /// <summary>
        /// DependencyProperty for <see cref="LineHeight" /> property.
        /// </summary>
        public static readonly DependencyProperty LineHeightProperty =
                DependencyProperty.RegisterAttached(
                        "LineHeight",
                        typeof(float),
                        typeof(Block),
                        new FrameworkPropertyMetadata(
                                float.NaN,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits),
                        new ValidateValueCallback(IsValidLineHeight));
        private static bool IsValidLineHeight(object? o)
        {
            float lineHeight = (float)o!;
            float minLineHeight = 1;
            float maxLineHeight = 160000;

            if (float.IsNaN(lineHeight))
                return true;
            if (lineHeight < minLineHeight)
                return false;
            if (lineHeight > maxLineHeight)
                return false;
            return true;
        }
        /// <summary>
        /// The LineHeight property specifies the height of each generated line box.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public float LineHeight
        {
            get { return (float)GetValue(LineHeightProperty)!; }
            set { SetValue(LineHeightProperty, value); }
        }
        /// <summary>
        /// DependencyProperty setter for <see cref="LineHeight" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetLineHeight(DependencyObject element, float value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(LineHeightProperty, value);
        }
        /// <summary>
        /// DependencyProperty getter for <see cref="LineHeight" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        [TypeConverter(typeof(LengthConverter))]
        public static float GetLineHeight(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (float)element.GetValue(LineHeightProperty)!;
        }

        /// <summary>
        /// DependencyProperty for <see cref="LineStackingStrategy" /> property.
        /// </summary>
        public static readonly DependencyProperty LineStackingStrategyProperty =
                DependencyProperty.RegisterAttached(
                        "LineStackingStrategy",
                        typeof(LineStackingStrategy),
                        typeof(Block),
                        new FrameworkPropertyMetadata(
                                LineStackingStrategy.MaxHeight,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits),
                        new ValidateValueCallback(IsValidLineStackingStrategy));
        private static bool IsValidLineStackingStrategy(object? o)
        {
            LineStackingStrategy value = (LineStackingStrategy)o!;
            return (value == LineStackingStrategy.MaxHeight
                    || value == LineStackingStrategy.BlockLineHeight);
        }
        /// <summary>
        /// The LineStackingStrategy property specifies how lines are placed
        /// </summary>
        public LineStackingStrategy LineStackingStrategy
        {
            get { return (LineStackingStrategy)GetValue(LineStackingStrategyProperty)!; }
            set { SetValue(LineStackingStrategyProperty, value); }
        }

        /// <summary>
        /// DependencyProperty setter for <see cref="LineStackingStrategy" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetLineStackingStrategy(DependencyObject element, LineStackingStrategy value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(LineStackingStrategyProperty, value);
        }

        /// <summary>
        /// DependencyProperty getter for <see cref="LineStackingStrategy" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        public static LineStackingStrategy GetLineStackingStrategy(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (LineStackingStrategy)element.GetValue(LineStackingStrategyProperty)!;
        }

        /// <summary>
        /// DependencyProperty for <see cref="Margin" /> property.
        /// </summary>
        public static readonly DependencyProperty MarginProperty =
                DependencyProperty.Register(
                        "Margin",
                        typeof(Thickness),
                        typeof(Block),
                        new FrameworkPropertyMetadata(
                                new Thickness(),
                                FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(IsValidMargin));
        internal static bool IsValidMargin(object? o)
        {
            Thickness t = (Thickness)o!;
            return IsValidThickness(t, /*allow NaN*/true);
        }
        /// <summary>
        /// The Margin property specifies the margin of the element.
        /// </summary>
        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty)!; }
            set { SetValue(MarginProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="Padding" /> property.
        /// </summary>
        public static readonly DependencyProperty PaddingProperty =
                DependencyProperty.Register(
                        "Padding",
                        typeof(Thickness),
                        typeof(Block),
                        new FrameworkPropertyMetadata(
                                new Thickness(),
                                FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(IsValidPadding));
        internal static bool IsValidPadding(object? o)
        {
            Thickness t = (Thickness)o!;
            return IsValidThickness(t, /*allow NaN*/true);
        }
        /// <summary>
        /// The Padding property specifies the padding of the element.
        /// </summary>
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty)!; }
            set { SetValue(PaddingProperty, value); }
        }


        /// <summary>
        /// DependencyProperty for <see cref="TextAlignment" /> property.
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty =
                DependencyProperty.RegisterAttached(
                        "TextAlignment",
                        typeof(TextAlignment),
                        typeof(Block),
                        new FrameworkPropertyMetadata(
                                TextAlignment.Left,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits),
                        new ValidateValueCallback(IsValidTextAlignment));
        private static bool IsValidTextAlignment(object? o)
        {
            TextAlignment value = (TextAlignment)o!;
            return value == TextAlignment.Center
                || value == TextAlignment.Justify
                || value == TextAlignment.Left
                || value == TextAlignment.Right;
        }
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty)!; }
            set { SetValue(TextAlignmentProperty, value); }
        }
        /// <summary>
        /// DependencyProperty setter for <see cref="TextAlignment" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetTextAlignment(DependencyObject element, TextAlignment value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(TextAlignmentProperty, value);
        }
        /// <summary>
        /// DependencyProperty getter for <see cref="TextAlignment" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        public static TextAlignment GetTextAlignment(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (TextAlignment)element.GetValue(TextAlignmentProperty)!;
        }


        /// <summary>
        /// DependencyProperty for hyphenation property.
        /// </summary>
        public static readonly DependencyProperty IsHyphenationEnabledProperty =
                DependencyProperty.RegisterAttached(
                        "IsHyphenationEnabled",
                        typeof(bool),
                        typeof(Block),
                        new FrameworkPropertyMetadata(
                                false,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
        /// <summary>
        /// CLR property for hyphenation
        /// </summary>
        public bool IsHyphenationEnabled
        {
            get { return (bool)GetValue(IsHyphenationEnabledProperty)!; }
            set { SetValue(IsHyphenationEnabledProperty, value); }
        }
        /// <summary>
        /// DependencyProperty setter for <see cref="IsHyphenationEnabled" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetIsHyphenationEnabled(DependencyObject element, bool value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(IsHyphenationEnabledProperty, value);
        }
        /// <summary>
        /// DependencyProperty getter for <see cref="IsHyphenationEnabled" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        public static bool GetIsHyphenationEnabled(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (bool)element.GetValue(IsHyphenationEnabledProperty)!;
        }

        internal static bool IsValidThickness(Thickness t, bool allowNaN)
        {
            float maxThickness = 16000;
            if (!allowNaN)
            {
                if (float.IsNaN(t.Left) || float.IsNaN(t.Right) || float.IsNaN(t.Top) || float.IsNaN(t.Bottom))
                {
                    return false;
                }
            }
            if (!float.IsNaN(t.Left) && (t.Left < 0 || t.Left > maxThickness))
            {
                return false;
            }
            if (!float.IsNaN(t.Right) && (t.Right < 0 || t.Right > maxThickness))
            {
                return false;
            }
            if (!float.IsNaN(t.Top) && (t.Top < 0 || t.Top > maxThickness))
            {
                return false;
            }
            if (!float.IsNaN(t.Bottom) && (t.Bottom < 0 || t.Bottom > maxThickness))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a Block immediately following this one
        /// on the same level of siblings
        /// </summary>
        public Block? NextBlock
        {
            get
            {
                return NextElement as Block;
            }
        }

        /// <summary>
        /// Returns a block immediately preceding this one
        /// on the same level of siblings
        /// </summary>
        public Block? PreviousBlock
        {
            get
            {
                return PreviousElement as Block;
            }
        }

        public static readonly DependencyProperty BorderThicknessProperty =
                DependencyProperty.Register(
                        "BorderThickness",
                        typeof(Thickness),
                        typeof(Block),
                        new FrameworkPropertyMetadata(
                                new Thickness(),
                                FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(IsValidBorderThickness));
        private static bool IsValidBorderThickness(object? o)
        {
            Thickness t = (Thickness)o!;
            return IsValidThickness(t, /*allow NaN*/false);
        }
        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty)!; }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty BorderBrushProperty =
                DependencyProperty.Register(
                        "BorderBrush",
                        typeof(Brush),
                        typeof(Block),
                        new FrameworkPropertyMetadata(
                                null,
                                FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush? BorderBrush
        {
            get { return (Brush?)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }


        public static readonly DependencyProperty FlowDirectionProperty = FrameworkElement.FlowDirectionProperty.AddOwner(typeof(Block));
        public FlowDirection FlowDirection
        {
            get { return (FlowDirection)GetValue(FlowDirectionProperty)!; }
            set { SetValue(FlowDirectionProperty, value); }
        }

        #endregion

        #region Layout

        public abstract IBlockLayout Layout { get; }

        #endregion
    }
}
