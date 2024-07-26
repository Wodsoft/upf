using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    public abstract class Inline : TextElement
    {
        #region Public Properties

        ///// <value>
        ///// A collection of Inlines containing this one in its sequential tree.
        ///// May return null if an element is not inserted into any tree.
        ///// </value>
        //public InlineCollection SiblingInlines
        //{
        //    get
        //    {
        //        if (this.Parent == null)
        //        {
        //            return null;
        //        }

        //        return new InlineCollection(this, /*isOwnerParent*/false);
        //    }
        //}

        /// <summary>
        /// Returns an Inline immediately following this one
        /// on the same level of siblings
        /// </summary>
        public Inline? NextInline
        {
            get
            {
                return NextElement as Inline;
            }
        }

        /// <summary>
        /// Returns an Inline immediately preceding this one
        /// on the same level of siblings
        /// </summary>
        public Inline? PreviousInline
        {
            get
            {
                return PreviousElement as Inline;
            }
        }

        /// <summary>
        /// DependencyProperty for <see cref="BaselineAlignment" /> property.
        /// </summary>
        public static readonly DependencyProperty BaselineAlignmentProperty =
                DependencyProperty.Register(
                        "BaselineAlignment",
                        typeof(BaselineAlignment),
                        typeof(Inline),
                        new FrameworkPropertyMetadata(
                                BaselineAlignment.Baseline,
                                FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        public BaselineAlignment BaselineAlignment
        {
            get { return (BaselineAlignment)GetValue(BaselineAlignmentProperty)!; }
            set { SetValue(BaselineAlignmentProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="TextDecorations" /> property.
        /// </summary>
        public static readonly DependencyProperty TextDecorationsProperty =
                DependencyProperty.Register(
                        "TextDecorations",
                        typeof(TextDecorationCollection),
                        typeof(Inline),
                        new FrameworkPropertyMetadata(TextDecorationCollection.Empty, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
        /// <summary>
        /// The TextDecorations property specifies decorations that are added to the text of an element.
        /// </summary>
        public TextDecorationCollection? TextDecorations
        {
            get { return (TextDecorationCollection?)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="FlowDirection" /> property.
        /// </summary>
        public static readonly DependencyProperty FlowDirectionProperty =
                FrameworkElement.FlowDirectionProperty.AddOwner(typeof(Inline));
        /// <summary>
        /// The FlowDirection property specifies the flow direction of the element.
        /// </summary>
        public FlowDirection FlowDirection
        {
            get { return (FlowDirection)GetValue(FlowDirectionProperty)!; }
            set { SetValue(FlowDirectionProperty, value); }
        }

        #endregion Public Properties

        #region Layout

        public abstract IInlineLayout Layout { get; }

        #endregion
    }
}
