using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;

namespace Wodsoft.UI.Media
{
    [ContentProperty("Children")]
    public class TransformGroup : Transform
    {
        #region Properties

        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register("Children",
                                   typeof(TransformCollection),
                                   typeof(TransformGroup),
                                   new PropertyMetadata(TransformCollection.Empty));
        public TransformCollection? Children
        {
            get { return (TransformCollection?)GetValue(ChildrenProperty); }
            set { SetValue(ChildrenProperty, value); }
        }

        public override Matrix3x2 Value
        {
            get
            {
                ReadPreamble();

                TransformCollection? children = Children;
                if ((children == null) || (children.Count == 0))
                    return Matrix3x2.Identity;

                var transform = children[0].Value;
                for (int i = 1; i < children.Count; i++)
                {
                    transform *= children[i].Value;
                }
                return transform;
            }
        }

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore() => new TransformGroup();

        /// <summary>
        ///     Shadows inherited Clone() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new TransformGroup Clone()
        {
            return (TransformGroup)base.Clone();
        }

        /// <summary>
        ///     Shadows inherited CloneCurrentValue() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new TransformGroup CloneCurrentValue()
        {
            return (TransformGroup)base.CloneCurrentValue();
        }

        #endregion
    }
}
