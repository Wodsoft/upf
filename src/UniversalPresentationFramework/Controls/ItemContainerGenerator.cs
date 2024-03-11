using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls.Primitives;

namespace Wodsoft.UI.Controls
{
    public class ItemContainerGenerator : IRecyclingItemContainerGenerator
    {
        private readonly IGeneratorHost _host;
        private readonly Node _root;
        private Generator? _generator;

        public ItemContainerGenerator(IGeneratorHost host)
        {
            _host = host;
            _host.ListenCollectionChanged(Host_CollectionChanged);
            _root = new Node();
            _root.Index = -1;
            _root.Size = _host.Items.Count;
        }

        private void Host_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        var position = GeneratorPositionFromIndex(e.NewStartingIndex, out var node);
                        var count = e.NewItems!.Count;
                        _generator?.ItemsChanged(e.Action, e.NewStartingIndex, count);
                        node.Size += count;
                        ItemsChanged?.Invoke(this, new ItemsChangedEventArgs(e.Action, position, count, 0));
                        break;
                    }
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        var position = GeneratorPositionFromIndex(e.OldStartingIndex, out var node);
                        var count = e.OldItems!.Count;
                        _generator?.ItemsChanged(e.Action, e.OldStartingIndex, count);
                        Remove(position, count, false, out var uiCount);
                        ItemsChanged?.Invoke(this, new ItemsChangedEventArgs(e.Action, position, count, uiCount));
                        break;
                    }
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    {
                        var oldPosition = GeneratorPositionFromIndex(e.OldStartingIndex, out var oldNode);
                        var newPosition = GeneratorPositionFromIndex(e.NewStartingIndex, out var newNode);
                        var count = e.OldItems!.Count;
                        int oldOffset = oldPosition.Offset, newOffset = newPosition.Offset;
                        var uiCount = 0;
                        for (int i = 0; i < count; i++)
                        {
                            bool hasContainer = false;
                            if (oldOffset == 0)
                            {
                                UIElement oldContainer = oldNode.Container!;
                                var item = e.OldItems![i];
                                oldContainer.SetValue(_ItemForItemContainerProperty, item);
                                oldContainer.SetValue(FrameworkElement.DataContextProperty, item);
                                if (oldNode.Size == 0)
                                {
                                    oldNode = oldNode.Next!;
                                }
                                else
                                {
                                    oldOffset++;
                                }
                                hasContainer = true;
                            }
                            else
                            {
                                if (oldOffset == oldNode.Size)
                                {
                                    oldOffset = 0;
                                    oldNode = oldNode.Next!;
                                }
                                else
                                {
                                    oldOffset++;
                                }
                            }
                            if (newOffset == 0)
                            {
                                UIElement newContainer = newNode.Container!;
                                var item = e.NewItems![i];
                                newContainer.SetValue(_ItemForItemContainerProperty, item);
                                newContainer.SetValue(FrameworkElement.DataContextProperty, item);
                                if (newNode.Size == 0)
                                {
                                    newNode = newNode.Next!;
                                }
                                else
                                {
                                    newOffset++;
                                }
                                hasContainer = true;
                            }
                            else
                            {
                                if (newOffset == newNode.Size)
                                {
                                    newOffset = 0;
                                    newNode = newNode.Next!;
                                }
                                else
                                {
                                    newOffset++;
                                }
                            }
                            if (hasContainer)
                                uiCount++;
                        }
                        ItemsChanged?.Invoke(this, new ItemsChangedEventArgs(e.Action, newPosition, oldPosition, count, uiCount));
                        break;
                    }
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    {
                        var position = GeneratorPositionFromIndex(e.NewStartingIndex, out var node);
                        var count = e.NewItems!.Count;
                        int offset = position.Offset;
                        var uiCount = 0;
                        for (int i = 0; i < count; i++)
                        {
                            if (offset == 0)
                            {
                                UIElement oldContainer = node.Container!;
                                var item = e.NewItems[i];
                                oldContainer.SetValue(_ItemForItemContainerProperty, item);
                                oldContainer.SetValue(FrameworkElement.DataContextProperty, item);
                                if (node.Size == 0)
                                {
                                    node = node.Next!;
                                }
                                else
                                {
                                    offset++;
                                }
                                uiCount++;
                            }
                            else
                            {
                                if (offset == node.Size)
                                {
                                    offset = 0;
                                    node = node.Next!;
                                }
                                else
                                {
                                    offset++;
                                }
                            }
                        }
                        ItemsChanged?.Invoke(this, new ItemsChangedEventArgs(e.Action, position, count, uiCount));
                        break;
                    }
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    {
                        Refresh();
                        break;
                    }
            }
        }

        public event ItemsChangedEventHandler? ItemsChanged;

        public UIElement? GenerateNext()
        {
            if (_generator == null)
                throw new InvalidOperationException("Generation not in progress.");
            return _generator.GenerateNext(true, out _);
        }

        public UIElement? GenerateNext(out bool isNewlyRealized)
        {
            if (_generator == null)
                throw new InvalidOperationException("Generation not in progress.");
            return _generator.GenerateNext(false, out isNewlyRealized);
        }

        public GeneratorPosition GeneratorPositionFromIndex(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= _host.Items.Count)
                throw new ArgumentOutOfRangeException(nameof(itemIndex));
            return GeneratorPositionFromIndex(itemIndex, out _);
        }

        private GeneratorPosition GeneratorPositionFromIndex(int itemIndex, out Node node)
        {
            int index = -1;
            int offset = 0;
            node = _root;
            while (node.Index < itemIndex)
            {
                if (node.Index + node.Size <= itemIndex)
                {
                    offset = itemIndex - node.Index;
                    break;
                }
                node = node.Next!;
            }
            return new GeneratorPosition(index, offset);
        }

        public int IndexFromGeneratorPosition(GeneratorPosition position)
        {
            if (position.Index == -1)
            {
                if (_root.Size < position.Offset)
                    return -1;
            }
            if (position.Index < -1)
                return -1;
            Node node = _root;
            for (int i = 0; i <= position.Index; i++)
            {
                if (node.Next == null)
                    return -1;
                node = node.Next;
            }
            if (position.Offset == 0)
                return node.Index;
            var index = node.Index + position.Offset;
            if (index < 0 || index >= _host.Items.Count)
                return -1;
            return index;
        }

        public void PrepareItemContainer(UIElement container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
            _host.PrepareItemContainer(container, container.GetValue(_ItemForItemContainerProperty));
        }

        public void Recycle(GeneratorPosition position, int count)
        {
            Remove(position, count, true, out _);
        }

        public void Remove(GeneratorPosition position, int count)
        {
            Remove(position, count, false, out _);
        }

        public void RemoveAll()
        {
            Node node = _root;
            Node previous;
            while (node.Next != null)
            {
                previous = node;
                node = node.Next;

                UIElement container = node.Container!;
                container.ClearValue(_ItemForItemContainerProperty);
                container.SetValue(FrameworkElement.DataContextProperty, null);

                previous.Size += 1 + node.Size;
                previous.Next = node.Next;
                if (node.Next == null)
                    return;
            }
        }

        public void Refresh()
        {
            RemoveAll();
            var position = new GeneratorPosition(0, 0);
            _generator?.ItemsChanged(System.Collections.Specialized.NotifyCollectionChangedAction.Reset, 0, 0);
            ItemsChanged?.Invoke(this, new ItemsChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset, position, 0, 0));
        }

        private void Remove(GeneratorPosition position, int count, bool isRecycling, out int uiCount)
        {
            if (position.Offset != 0)
                throw new ArgumentException("Remove offset must be zero.", "position");
            if (count <= 0)
                throw new ArgumentException("Remove count must be larger than zero.", "count");
            uiCount = 0;
            Node node = _root;
            for (int i = 0; i <= position.Index; i++)
            {
                if (node.Next == null)
                    return;
                node = node.Next;
            }
            if (node == _root)
                return;
            var previous = _root;
            while (count != 0)
            {
                UIElement container = node.Container!;
                container.ClearValue(_ItemForItemContainerProperty);
                container.SetValue(FrameworkElement.DataContextProperty, null);
                if (isRecycling)
                {
                    _recyclableContainers.Enqueue(container);
                }
                previous.Size += 1 + node.Size;
                previous.Next = node.Next;
                count -= node.Size;
                if (count <= 0)
                    return;
                if (node.Next == null)
                    return;
                previous = node;
                node = node.Next;
                count--;
                uiCount++;
            }
        }

        public IDisposable StartAt(GeneratorPosition position, GeneratorDirection direction) => StartAt(position, direction, false);

        public IDisposable StartAt(GeneratorPosition position, GeneratorDirection direction, bool allowStartAtRealizedItem)
        {
            if (_generator != null)
                throw new InvalidOperationException("Generation in progress.");
            int offset = position.Offset;
            Node node = _root;
            if (position.Index != -1)
            {
                for (int i = 0; i <= position.Index; i++)
                {
                    if (node.Next == null)
                    {
                        offset = 0;
                        break;
                    }
                    node = node.Next;
                }
            }
            if (offset > node.Size)
                offset = node.Size;
            else if (offset <= 0 && node.Index == -1)
                offset = 1;
            _generator = new Generator(this, node, offset, direction, allowStartAtRealizedItem);
            return _generator;
        }

        public bool IsItemContainer(UIElement element)
        {
            return element.HasLocalValue(_ItemForItemContainerProperty);
        }

        private Queue<UIElement> _recyclableContainers = new Queue<UIElement>();

        private static readonly DependencyProperty _ItemForItemContainerProperty =
                DependencyProperty.RegisterAttached("ItemForItemContainer", typeof(object), typeof(ItemContainerGenerator));

        private class Generator : IDisposable
        {
            private readonly ItemContainerGenerator _parent;
            private readonly GeneratorDirection _direction;
            private readonly bool _allowStartAtRealizedItem;
            private readonly IList _items;
            private Node _node;
            private int _offset;

            public Generator(ItemContainerGenerator parent, Node node, int offset, GeneratorDirection direction, bool allowStartAtRealizedItem)
            {
                _parent = parent;
                _node = node;
                _direction = direction;
                _allowStartAtRealizedItem = allowStartAtRealizedItem;
                _items = parent._host.Items;
                _node = node;
                _offset = offset;
            }

            public void ItemsChanged(System.Collections.Specialized.NotifyCollectionChangedAction action, int index, int count)
            {
                switch (action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        if (index >= _node.Index && index < _node.Index + _offset)
                            _offset += count;
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        if (index <= _node.Index && index + count >= _node.Index)
                        {
                            Node previousNode = _node.Previous!;
                            while (previousNode.Index > index)
                                previousNode = previousNode.Previous!;
                            _offset = _node.Index + _offset - previousNode.Index;
                            _node = previousNode;
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        _node = _parent._root;
                        _offset = 1;
                        break;
                }
            }

            public UIElement? GenerateNext(bool stopAtRealized, out bool isNewlyRealized)
            {
                if (_offset == -1)
                {
                    isNewlyRealized = false;
                    return null;
                }
                if (_offset == 0)
                {
                    isNewlyRealized = false;
                    if (_node.Index == -1)
                        return null;
                    var c = _node.Container;
                    if (_direction == GeneratorDirection.Forward)
                    {
                        if (_node.Size != 0)
                            _offset++;
                        else if (_node.Next != null)
                            _node = _node.Next;
                    }
                    else
                    {
                        _node = _node.Previous!;
                        _offset = _node.Size;
                    }
                    if (stopAtRealized)
                        return null;
                    return c;
                }
                var index = _node.Index + _offset;
                if (index < 0 || index >= _items.Count)
                {
                    isNewlyRealized = false;
                    return null;
                }
                var item = _items[index];
                UIElement container;
                if (item != null && _parent._recyclableContainers.Count != 0 && !_parent._host.IsItemItsOwnContainer(item))
                {
                    container = _parent._recyclableContainers.Dequeue();
                    isNewlyRealized = false;
                }
                else
                {
                    container = _parent._host.GetContainerForItem(item);
                    isNewlyRealized = true;
                }
                container.SetValue(_ItemForItemContainerProperty, item);
                if (item != container)
                    container.SetValue(FrameworkElement.DataContextProperty, item);

                var newNode = new Node { Container = container, Index = index, Previous = _node, Next = _node.Next, Size = _node.Size - _offset };
                if (newNode.Next != null)
                    newNode.Next.Previous = newNode;
                _node.Next = newNode;
                _node.Size = _offset - 1;

                if (_direction == GeneratorDirection.Forward)
                {
                    _node = newNode;
                    if (newNode.Size == 0 && _node.Next != null)
                    {
                        _node = _node.Next;
                        _offset = 0;
                    }
                    else
                    {
                        _offset = 1;
                    }
                }
                else
                {
                    _offset--;
                }
                return container;
            }

            public void Dispose()
            {
                if (_offset != -1)
                {
                    _offset = -1;
                    _parent._generator = null;
                }
            }
        }

        private record class Node
        {
            public Node? Previous;
            public Node? Next;

            public UIElement? Container;

            /// <summary>
            /// Index of item
            /// </summary>
            public int Index;
            /// <summary>
            /// Size of unrealized items
            /// </summary>
            public int Size;
        }
    }
}
