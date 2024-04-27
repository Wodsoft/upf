using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.TextFormatting
{
    public class GenericTextParagraphProperties : TextParagraphProperties
    {
        /// <summary>
        /// Constructing TextParagraphProperties
        /// </summary>
        /// <param name="flowDirection">text flow direction</param>
        /// <param name="textAlignment">logical horizontal alignment</param>
        /// <param name="firstLineInParagraph">true if the paragraph is the first line in the paragraph</param>
        /// <param name="alwaysCollapsible">true if the line is always collapsible</param>
        /// <param name="defaultTextRunProperties">default paragraph's default run properties</param>
        /// <param name="textWrap">text wrap option</param>
        /// <param name="lineHeight">Paragraph line height</param>
        /// <param name="indent">line indentation</param>
        public GenericTextParagraphProperties(
            FlowDirection flowDirection,
            TextAlignment textAlignment,
            bool firstLineInParagraph,
            bool alwaysCollapsible,
            TextRunProperties defaultTextRunProperties,
            TextWrapping textWrap,
            float lineHeight,
            float indent
            )
        {
            _flowDirection = flowDirection;
            _textAlignment = textAlignment;
            _firstLineInParagraph = firstLineInParagraph;
            _alwaysCollapsible = alwaysCollapsible;
            _defaultTextRunProperties = defaultTextRunProperties;
            _textWrap = textWrap;
            _lineHeight = lineHeight;
            _indent = indent;
        }

        /// <summary>
        /// Constructing TextParagraphProperties from another one
        /// </summary>
        /// <param name="textParagraphProperties">source line props</param>
        public GenericTextParagraphProperties(TextParagraphProperties textParagraphProperties)
        {
            _flowDirection = textParagraphProperties.FlowDirection;
            _defaultTextRunProperties = textParagraphProperties.DefaultTextRunProperties;
            _textAlignment = textParagraphProperties.TextAlignment;
            _lineHeight = textParagraphProperties.LineHeight;
            _firstLineInParagraph = textParagraphProperties.FirstLineInParagraph;
            _alwaysCollapsible = textParagraphProperties.AlwaysCollapsible;
            _textWrap = textParagraphProperties.TextWrapping;
            _indent = textParagraphProperties.Indent;
        }



        /// <summary>
        /// This property specifies whether the primary text advance
        /// direction shall be left-to-right, right-to-left, or top-to-bottom.
        /// </summary>
        public override FlowDirection FlowDirection
        {
            get { return _flowDirection; }
        }


        /// <summary>
        /// This property describes how inline content of a block is aligned.
        /// </summary>
        public override TextAlignment TextAlignment
        {
            get { return _textAlignment; }
        }


        /// <summary>
        /// Paragraph's line height
        /// </summary>
        public override float LineHeight
        {
            get { return _lineHeight; }
        }


        /// <summary>
        /// Indicates the first line of the paragraph.
        /// </summary>
        public override bool FirstLineInParagraph
        {
            get { return _firstLineInParagraph; }
        }


        /// <summary>
        /// If true, the formatted line may always be collapsed. If false (the default),
        /// only lines that overflow the paragraph width are collapsed.
        /// </summary>
        public override bool AlwaysCollapsible
        {
            get { return _alwaysCollapsible; }
        }


        /// <summary>
        /// Paragraph's default run properties
        /// </summary>
        public override TextRunProperties DefaultTextRunProperties
        {
            get { return _defaultTextRunProperties; }
        }


        /// <summary>
        /// This property controls whether or not text wraps when it reaches the flow edge
        /// of its containing block box
        /// </summary>
        public override TextWrapping TextWrapping
        {
            get { return _textWrap; }
        }


        ///// <summary>
        ///// This property specifies marker characteristics of the first line in paragraph
        ///// </summary>
        //public override TextMarkerProperties TextMarkerProperties
        //{
        //    get { return null; }
        //}


        /// <summary>
        /// Line indentation
        /// </summary>
        public override float Indent
        {
            get { return _indent; }
        }

        /// <summary>
        /// Set text flow direction
        /// </summary>
        internal void SetFlowDirection(FlowDirection flowDirection)
        {
            _flowDirection = flowDirection;
        }


        /// <summary>
        /// Set text alignment
        /// </summary>
        internal void SetTextAlignment(TextAlignment textAlignment)
        {
            _textAlignment = textAlignment;
        }


        /// <summary>
        /// Set line height
        /// </summary>
        internal void SetLineHeight(float lineHeight)
        {
            _lineHeight = lineHeight;
        }

        /// <summary>
        /// Set text wrap
        /// </summary>
        internal void SetTextWrapping(TextWrapping textWrap)
        {
            _textWrap = textWrap;
        }

        private FlowDirection _flowDirection;
        private TextAlignment _textAlignment;
        private bool _firstLineInParagraph;
        private bool _alwaysCollapsible;
        private TextRunProperties _defaultTextRunProperties;
        private TextWrapping _textWrap;
        private float _indent;
        private float _lineHeight;
    }
}
