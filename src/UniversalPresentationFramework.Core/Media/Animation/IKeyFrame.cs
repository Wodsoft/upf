using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    /// <summary>
    /// This interface should be implemented by all key frames to provide
    /// untyped access to the key time and value.
    /// </summary>
    public interface IKeyFrame
    {
        /// <summary>
        /// The key time associated with the key frame.
        /// </summary>
        /// <value></value>
        KeyTime KeyTime { get; set; }

        /// <summary>
        /// The value associated with the key frame.
        /// </summary>
        /// <value></value>
        object? Value { get; set; }
    }
}
