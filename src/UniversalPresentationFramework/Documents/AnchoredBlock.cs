using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    [ContentProperty("Blocks")]
    public abstract class AnchoredBlock : Inline
    {
        private BlockCollection _blocks;

        #region Constructors


        public AnchoredBlock()
        {
            _blocks = new BlockCollection(this, TextElementNode);
        }

        public AnchoredBlock(Block block) : this()
        {
            _blocks.Add(block);
        }

        #endregion

        #region Properties

        public BlockCollection Blocks => _blocks;

        public static readonly DependencyProperty MarginProperty =
                Block.MarginProperty.AddOwner(
                        typeof(AnchoredBlock),
                        new FrameworkPropertyMetadata(
                                new Thickness(),
                                FrameworkPropertyMetadataOptions.AffectsMeasure));
        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty)!; }
            set { SetValue(MarginProperty, value); }
        }

        public static readonly DependencyProperty PaddingProperty =
                Block.PaddingProperty.AddOwner(
                        typeof(AnchoredBlock),
                        new FrameworkPropertyMetadata(
                                new Thickness(),
                                FrameworkPropertyMetadataOptions.AffectsMeasure));
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty)!; }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessProperty =
                Block.BorderThicknessProperty.AddOwner(
                        typeof(AnchoredBlock),
                        new FrameworkPropertyMetadata(
                            new Thickness(),
                            FrameworkPropertyMetadataOptions.AffectsMeasure));
        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty)!; }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty BorderBrushProperty =
                Block.BorderBrushProperty.AddOwner(
                        typeof(AnchoredBlock),
                        new FrameworkPropertyMetadata(
                                null,
                                FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush? BorderBrush
        {
            get { return (Brush?)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly DependencyProperty TextAlignmentProperty =
                Block.TextAlignmentProperty.AddOwner(typeof(AnchoredBlock));
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty)!; }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public static readonly DependencyProperty LineHeightProperty =
                Block.LineHeightProperty.AddOwner(typeof(AnchoredBlock));
        [TypeConverter(typeof(LengthConverter))]
        public float LineHeight
        {
            get { return (float)GetValue(LineHeightProperty)!; }
            set { SetValue(LineHeightProperty, value); }
        }

        public static readonly DependencyProperty LineStackingStrategyProperty =
                Block.LineStackingStrategyProperty.AddOwner(typeof(AnchoredBlock));
        public LineStackingStrategy LineStackingStrategy
        {
            get { return (LineStackingStrategy)GetValue(LineStackingStrategyProperty)!; }
            set { SetValue(LineStackingStrategyProperty, value); }
        }

        #endregion
    }
}
