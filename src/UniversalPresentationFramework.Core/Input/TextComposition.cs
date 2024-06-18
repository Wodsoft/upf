using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{

    public enum TextCompositionStage
    {
        /// <summary>
        /// The composition is not start yet.
        /// </summary>
        None = 0,

        /// <summary>
        /// The composition has started.
        /// </summary>
        Started = 1,

        /// <summary>
        /// The composition is changing.
        /// </summary>
        Changing = 2,

        /// <summary>
        /// The composition has completed or canceled.
        /// </summary>
        Completed = 3,
    }

    public class TextComposition
    {
        public TextComposition()
        {
            CompositionText = string.Empty;
        }

        public string CompositionText { get; internal set; }

        public int CaretPosition { get; internal set; }

        public TextCompositionStage Stage { get; internal set; }
    }
}
