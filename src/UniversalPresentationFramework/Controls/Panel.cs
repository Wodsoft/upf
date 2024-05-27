using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls.Primitives;
using Wodsoft.UI.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    [ContentProperty("Children")]
    public class Panel : FrameworkElement, IAddChild
    {
        private UIElementCollection? _children;

        public UIElementCollection Children
        {
            get
            {
                if (_isItemsHost)
                    EnsureGenerator();
                else
                {
                    _children ??= new UIElementCollection(this, IsItemsHost ? null : this);
                }
                return _children!;
            }
        }

        protected internal override int VisualChildrenCount => _children?.Count ?? 0;

        protected internal override Visual GetVisualChild(int index)
        {
            if (_children == null)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _children[index]!;
        }

        void IAddChild.AddChild(object value)
        {
            AddChild(value);
        }

        protected virtual void AddChild(object value)
        {
            if (value is UIElement element)
                Children.Add(element);
            else
                throw new NotSupportedException("Panel can add UIElement only.");
        }

        void IAddChild.AddText(string text)
        {
            throw new NotSupportedException("Panel doesn't support add text.");
        }

        private bool _isItemsHost;
        public static readonly DependencyProperty IsItemsHostProperty =
            DependencyProperty.Register(
                    "IsItemsHost",
                    typeof(bool),
                    typeof(Panel),
                    new FrameworkPropertyMetadata(
                            false, // defaultValue
                            FrameworkPropertyMetadataOptions.NotDataBindable,
                            OnIsItemsHostChanged));
        private static void OnIsItemsHostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool value = (bool)e.NewValue!;
            var panel = (Panel)d;
            panel._isItemsHost = value;
            if (!value && panel._generator != null)
            {
                panel.DisconnectToGenerator();
            }
        }

        public bool IsItemsHost
        {
            get { return (bool)GetValue(IsItemsHostProperty)!; }
            set { SetValue(IsItemsHostProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty =
                DependencyProperty.Register("Background",
                        typeof(Brush),
                        typeof(Panel),
                        new FrameworkPropertyMetadata(null,
                                FrameworkPropertyMetadataOptions.AffectsRender |
                                FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));
        public Brush? Background
        {
            get { return (Brush?)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Brush? background = Background;
            if (background != null)
            {
                Size renderSize = RenderSize;
                drawingContext.DrawRectangle(background,
                                 null,
                                 new Rect(0.0f, 0.0f, renderSize.Width, renderSize.Height));
            }
        }

        #region ItemsHost

        private IItemContainerGenerator? _generator;
        private void EnsureGenerator()
        {
            if (_generator == null)
            {
                ConnectToGenerator();

                if (_children == null || _children.LogicalParent != null)
                    _children = new UIElementCollection(this, null);
                else
                    _children.Clear();

                GenerateChildren();
            }
        }

        private void ConnectToGenerator()
        {
            if (TemplatedParent is ItemsPresenter itemsPresenter)
            {
                var owner = itemsPresenter.Owner;
                if (owner == null)
                    throw new InvalidOperationException("ItemsPresenter doesn't have owner.");
                _generator = owner.ItemContainerGenerator;
                if (_generator == null)
                    throw new InvalidOperationException("ItemsControl return null for ItemContainerGenerator.");
                _generator.ItemsChanged += OnItemsChanged;
                _generator.RemoveAll();
            }
            else
                throw new InvalidOperationException("TemplatedParent must be a ItemsPresenter if Panel is items host.");
        }

        private void DisconnectToGenerator()
        {
            _generator!.ItemsChanged -= OnItemsChanged;
            _generator.RemoveAll();
            _children!.Clear();
        }

        protected virtual void GenerateChildren()
        {
            if (_generator != null)
            {
                using (_generator.StartAt(new GeneratorPosition(-1, 0), GeneratorDirection.Forward))
                {
                    UIElement? child;
                    while ((child = _generator.GenerateNext()) != null)
                    {
                        _children!.AddInternal(child);
                        _generator.PrepareItemContainer(child);
                    }
                }
            }
        }

        protected virtual void OnItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddChildren(e.Position, e.ItemCount);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveChildren(e.Position, e.ItemUICount);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceChildren(e.Position, e.ItemCount, e.ItemUICount);
                    break;
                case NotifyCollectionChangedAction.Move:
                    MoveChildren(e.OldPosition, e.Position, e.ItemUICount);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    ResetChildren();
                    break;
            }
        }

        private void AddChildren(GeneratorPosition pos, int itemCount)
        {
            using (_generator!.StartAt(pos, GeneratorDirection.Forward))
            {
                for (int i = 0; i < itemCount; i++)
                {
                    UIElement? e = _generator.GenerateNext();
                    if (e == null)
                        break;
                    _children!.InsertInternal(pos.Index + 1 + i, e);
                    _generator.PrepareItemContainer(e);
                }
            }
        }

        private void RemoveChildren(GeneratorPosition pos, int containerCount)
        {
            // If anything is wrong, I think these collections should do parameter checking
            _children!.RemoveRangeInternal(pos.Index, containerCount);
        }

        private void ReplaceChildren(GeneratorPosition pos, int itemCount, int containerCount)
        {
            using (_generator!.StartAt(pos, GeneratorDirection.Forward, true))
            {
                for (int i = 0; i < itemCount; i++)
                {
                    bool isNewlyRealized;
                    UIElement? e = _generator.GenerateNext(out isNewlyRealized);
                    if (e == null)
                        break;
                    if (!isNewlyRealized)
                    {
                        _children!.SetInternal(pos.Index + i, e);
                        _generator.PrepareItemContainer(e);
                    }
                }
            }
        }

        private void MoveChildren(GeneratorPosition fromPos, GeneratorPosition toPos, int containerCount)
        {
            if (fromPos == toPos)
                return;

            int toIndex = _generator!.IndexFromGeneratorPosition(toPos);
            UIElement[] elements = new UIElement[containerCount];

            for (int i = 0; i < containerCount; i++)
                elements[i] = _children![fromPos.Index + i]!;

            _children!.RemoveRangeInternal(fromPos.Index, containerCount);

            for (int i = 0; i < containerCount; i++)
            {
                _children.InsertInternal(toIndex + i, elements[i]);
            }
        }

        private void ResetChildren()
        {
            _children!.Clear();
            GenerateChildren();
        }

        #endregion
    }
}
