using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls.Primitives
{
    /// <summary>
    /// Type of scrolling that causes ScrollEvent.
    /// </summary>
    public enum ScrollEventType
    {
        /// <summary>
        /// Thumb has stopped moving.
        /// </summary>
        EndScroll,
        /// <summary>
        /// Thumb was moved to the Minimum position.
        /// </summary>
        First,
        /// <summary>
        /// Thumb was moved a large distance. The user clicked the scroll bar to the left(horizontal) or above(vertical) the scroll box.
        /// </summary>
        LargeDecrement,
        /// <summary>
        /// Thumb was moved a large distance. The user clicked the scroll bar to the right(horizontal) or below(vertical) the scroll box.
        /// </summary>
        LargeIncrement,
        /// <summary>
        /// Thumb was moved to the Maximum position
        /// </summary>
        Last,
        /// <summary>
        /// Thumb was moved a small distance. The user clicked the left(horizontal) or top(vertical) scroll arrow.
        /// </summary>
        SmallDecrement,
        /// <summary>
        /// Thumb was moved a small distance. The user clicked the right(horizontal) or bottom(vertical) scroll arrow.
        /// </summary>
        SmallIncrement,
        /// <summary>
        /// Thumb was moved.
        /// </summary>
        ThumbPosition,
        /// <summary>
        /// Thumb is currently being moved.
        /// </summary>
        ThumbTrack
    }
}
