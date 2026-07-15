using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using System.Xml.Linq;
using Wodsoft.UI.Documents;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    [ContentProperty("Blocks")]
    public partial class FlowDocument : FrameworkContentElement
    {
        private BlockCollection _blocks;
        private FlowDocumentNode _node;

        #region Constructors

        static FlowDocument()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlowDocument), new FrameworkPropertyMetadata(typeof(FlowDocument)));
        }

        public FlowDocument()
        {
            _node = new FlowDocumentNode();
            _blocks = new BlockCollection(this, _node);
        }

        #endregion

        #region Properties

        public BlockCollection Blocks => _blocks;


        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(FlowDocument));
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty)!; }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(FlowDocument));
        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty)!; }
            set { SetValue(FontStyleProperty, value); }
        }

        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(FlowDocument));
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty)!; }
            set { SetValue(FontWeightProperty, value); }
        }

        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(FlowDocument));
        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(FlowDocument));
        [TypeConverter(typeof(FontSizeConverter))]
        public float FontSize
        {
            get { return (float)GetValue(FontSizeProperty)!; }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(FlowDocument));
        public Brush? Foreground
        {
            get { return (Brush?)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(typeof(FlowDocument), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush? Background
        {
            get { return (Brush?)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        //public static readonly DependencyProperty TextEffectsProperty =
        //        TextElement.TextEffectsProperty.AddOwner(
        //                typeof(FlowDocument),
        //                new FrameworkPropertyMetadata(
        //                        new FreezableDefaultValueFactory(TextEffectCollection.Empty),
        //                        FrameworkPropertyMetadataOptions.AffectsRender));
        //public TextEffectCollection TextEffects
        //{
        //    get { return (TextEffectCollection)GetValue(TextEffectsProperty); }
        //    set { SetValue(TextEffectsProperty, value); }
        //}

        public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(FlowDocument));
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty)!; }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public static readonly DependencyProperty FlowDirectionProperty = Block.FlowDirectionProperty.AddOwner(typeof(FlowDocument));
        public FlowDirection FlowDirection
        {
            get { return (FlowDirection)GetValue(FlowDirectionProperty)!; }
            set { SetValue(FlowDirectionProperty, value); }
        }

        public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(FlowDocument));
        [TypeConverter(typeof(LengthConverter))]
        public float LineHeight
        {
            get { return (float)GetValue(LineHeightProperty)!; }
            set { SetValue(LineHeightProperty, value); }
        }

        public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(typeof(FlowDocument));
        public LineStackingStrategy LineStackingStrategy
        {
            get { return (LineStackingStrategy)GetValue(LineStackingStrategyProperty)!; }
            set { SetValue(LineStackingStrategyProperty, value); }
        }

        #endregion
    }
}
