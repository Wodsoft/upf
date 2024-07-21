using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using System.Xml.Linq;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Data
{
    public class RelativeSource : MarkupExtension, ISupportInitialize
    {
        #region constructors

        /// <summary>Constructor
        /// </summary>
        public RelativeSource()
        {
            // default mode to FindAncestor so that setting Type and Level would be OK
            _mode = RelativeSourceMode.FindAncestor;
        }

        /// <summary>Constructor
        /// </summary>
        public RelativeSource(RelativeSourceMode mode)
        {
            InitializeMode(mode);
        }

        /// <summary>Constructor for FindAncestor mode
        /// </summary>
        public RelativeSource(RelativeSourceMode mode, Type ancestorType, int ancestorLevel)
        {
            InitializeMode(mode);
            AncestorType = ancestorType;
            AncestorLevel = ancestorLevel;
        }

        #endregion constructors

        #region ISupportInitialize

        /// <summary>Begin Initialization</summary>
        void ISupportInitialize.BeginInit()
        {
        }

        /// <summary>End Initialization, verify that internal state is consistent</summary>
        void ISupportInitialize.EndInit()
        {
            if (IsUninitialized)
                throw new InvalidOperationException("Mode not set.");
            if (_mode == RelativeSourceMode.FindAncestor && (AncestorType == null))
                throw new InvalidOperationException("Ancestor type not set.");
        }

        #endregion ISupportInitialize

        #region Properties

        private RelativeSourceMode _mode;
        public RelativeSourceMode Mode
        {
            get => _mode;
            set
            {
                if (IsUninitialized)
                {
                    InitializeMode(value);
                }
                else if (value != _mode)    // mode changes are not allowed
                {
                    throw new InvalidOperationException("Relative source is immutable.");
                }
            }
        }

        private Type? _ancestorType;
        /// <summary> The Type of ancestor to look for, in FindAncestor mode.
        /// </summary>
        /// <remarks> if Mode has not been set explicitly, setting AncestorType will implicitly lock Mode to FindAncestor. </remarks>
        /// <exception cref="InvalidOperationException"> RelativeSource is not in FindAncestor mode </exception>
        public Type? AncestorType
        {
            get { return _ancestorType; }
            set
            {
                if (IsUninitialized)
                {
                    Debug.Assert(_mode == RelativeSourceMode.FindAncestor);
                    AncestorLevel = 1;  // lock the mode and set default level
                }

                if (_mode != RelativeSourceMode.FindAncestor)
                {
                    // in all other modes, AncestorType should not get set to a non-null value
                    if (value != null)
                        throw new InvalidOperationException("Only FindAncestor mode can set ancestor type.");
                }
                else
                {
                    _ancestorType = value;
                }
            }
        }

        private int _ancestorLevel;
        /// <summary> The level of ancestor to look for, in FindAncestor mode.  Use 1 to indicate the one nearest to the target element.
        /// </summary>
        /// <remarks> if Mode has not been set explicitly, getting AncestorLevel will return -1 and
        /// setting AncestorLevel will implicitly lock Mode to FindAncestor. </remarks>
        /// <exception cref="InvalidOperationException"> RelativeSource is not in FindAncestor mode </exception>
        /// <exception cref="ArgumentOutOfRangeException"> AncestorLevel cannot be set to less than 1 </exception>
        public int AncestorLevel
        {
            get { return _ancestorLevel; }
            set
            {
                Debug.Assert((!IsUninitialized) || (_mode == RelativeSourceMode.FindAncestor));

                if (_mode != RelativeSourceMode.FindAncestor)
                {
                    // in all other modes, AncestorLevel should not get set to a non-zero value
                    if (value != 0)
                        throw new InvalidOperationException("Only FindAncestor mode can set ancestor type.");
                }
                else if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Invalid ancestor level.");
                }
                else
                {
                    _ancestorLevel = value;
                }
            }
        }

        private bool IsUninitialized
        {
            get { return (_ancestorLevel == -1); }
        }

        #endregion

        #region Methods

        void InitializeMode(RelativeSourceMode mode)
        {
            if (mode == RelativeSourceMode.FindAncestor)
            {
                // default level
                _ancestorLevel = 1;
                _mode = mode;
            }
            else if (mode == RelativeSourceMode.PreviousData
                || mode == RelativeSourceMode.Self
                || mode == RelativeSourceMode.TemplatedParent)
            {
                _ancestorLevel = 0;
                _mode = mode;
            }
            else
            {
                throw new ArgumentException("Invalid mode.");
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_mode == RelativeSourceMode.PreviousData)
                return PreviousData;
            if (_mode == RelativeSourceMode.Self)
                return Self;
            if (_mode == RelativeSourceMode.TemplatedParent)
                return TemplatedParent;
            return this;
        }

        public object? GetSource(DependencyObject element)
        {
            switch (_mode)
            {
                case RelativeSourceMode.Self:
                    return element;
                case RelativeSourceMode.TemplatedParent:
                    return (element as FrameworkElement)?.TemplatedParent;
                case RelativeSourceMode.PreviousData:
                    return null;
                case RelativeSourceMode.FindAncestor:
                    if (element is Visual visual)
                        return FindAncestorOfType(AncestorType!, AncestorLevel, visual);
                    return null;
                default:
                    return null;
            }
        }

        //private object? GetPreviousData(FrameworkElement? fe)
        //{
        //    // move up to the next containing DataContext scope
        //    for (; fe != null; fe = fe.Parent)
        //    {
        //        if (fe.HasLocalValue(FrameworkElement.DataContextProperty))
        //        {
        //            // special case:  if the element is a ContentPresenter
        //            // whose templated parent is a ContentControl or
        //            // HeaderedItemsControl, and both have the same
        //            // DataContext, we'll use the parent instead of the
        //            // ContentPresenter.  In this case, the DataContext
        //            // of the CP is set by various forwarding rules, and
        //            // shouldn't count as a new scope.
        //            // Similarly, do the same for a FE whose parent
        //            // is a GridViewRowPresenter;  this enables Previous bindings
        //            // inside ListView.
        //            FrameworkElement parent, child;

        //            if (fe is ContentPresenter cp)
        //            {
        //                child = cp;
        //                parent = cp.TemplatedParent;
        //                //if (!(parent is ContentControl || parent is HeaderedItemsControl))
        //                //{
        //                //    parent = cp.Parent as System.Windows.Controls.Primitives.GridViewRowPresenterBase;
        //                //}
        //            }
        //            else
        //            {
        //                child = fe;
        //                parent = child.Parent as System.Windows.Controls.Primitives.GridViewRowPresenterBase;
        //            }

        //            if (child != null && parent != null &&
        //                ItemsControl.EqualsEx(child.DataContext, parent.DataContext))
        //            {
        //                d = parent;
        //                if (!BindingExpression.HasLocalDataContext(parent))
        //                {
        //                    continue;
        //                }
        //            }

        //            break;
        //        }
        //    }

        //    if (fe == null)
        //        return DependencyProperty.UnsetValue;   // we fell off the tree

        //    // this only makes sense within generated content.  If this
        //    // is the case, then d is now the wrapper element, its visual
        //    // parent is the layout element, and the layout's ItemsOwner
        //    // is the govening ItemsControl.
        //    Visual v = d as Visual;
        //    DependencyObject layout = (v != null) ? VisualTreeHelper.GetParent(v) : null;
        //    ItemsControl ic = ItemsControl.GetItemsOwner(layout);
        //    if (ic == null)
        //    {
        //        return null;
        //    }

        //    // now look up the wrapper's previous sibling within the
        //    // layout's children collection
        //    Visual v2 = layout as Visual;
        //    int count = (v2 != null) ? v2.InternalVisualChildrenCount : 0;
        //    int j = -1;
        //    Visual prevChild = null;   //child at j-1th index
        //    if (count != 0)
        //    {
        //        j = IndexOf(v2, v, out prevChild);
        //    }
        //    if (j > 0)
        //    {
        //        d = prevChild;
        //    }
        //    else
        //    {
        //        d = null;
        //    }
        //    return d;
        //}


        private DependencyObject? FindAncestorOfType(Type type, int level, Visual? visual)
        {
            if (type == null)
                return null;
            if (level < 1)
                return null;

            while (visual != null)
            {
                if (type.IsInstanceOfType(visual))   // found it!
                {
                    if (--level <= 0)
                        break;
                }
                visual = visual.VisualParent;
            }

            return visual;
        }

        #endregion

        #region Fields

        public static readonly RelativeSource PreviousData = new RelativeSource(RelativeSourceMode.PreviousData);

        /// <summary>static instance of RelativeSource for TemplatedParent mode.
        /// </summary>
        public static readonly RelativeSource TemplatedParent = new RelativeSource(RelativeSourceMode.TemplatedParent);

        /// <summary>static instance of RelativeSource for Self mode.
        /// </summary>
        public static readonly RelativeSource Self = new RelativeSource(RelativeSourceMode.Self);

        #endregion
    }
}
