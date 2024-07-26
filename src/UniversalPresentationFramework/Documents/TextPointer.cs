using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Documents
{
    public class TextPointer
    {
        private readonly TextTreeNode _node;
        private readonly ElementEdge _edge;
        private readonly LogicalDirection _direction;
        private readonly int _elementOffset;
        private readonly IReadOnlyTextContainer _textContainer;

        public TextPointer(IReadOnlyTextContainer textContainer, TextTreeNode node, ElementEdge edge, LogicalDirection direction) :
            this(textContainer, node, edge, direction, 0)
        {

        }

        private TextPointer(IReadOnlyTextContainer textContainer, TextTreeNode node, ElementEdge edge, LogicalDirection direction, int offset)
        {
            _textContainer = textContainer;
            _node = node;
            _edge = edge;
            _direction = direction;
            _elementOffset = offset;
        }

        public TextTreeNode Node => _node;

        public ElementEdge Edge => _edge;

        public LogicalDirection LogicalDirection => _direction;

        public TextPointer DocumentStart => _textContainer.DocumentStart;

        public TextPointer DocumentEnd => _textContainer.DocumentEnd;

        public IReadOnlyTextContainer TextContainer => _textContainer;

        public int Offset => _node.GetSymbolOffset(_textContainer.Generation);

        public TextPointer? GetPositionAtOffset(int offset) => GetPositionAtOffset(offset, _direction);

        public TextPointer? GetPositionAtOffset(int offset, LogicalDirection direction)
        {
            if (offset == 0)
                return this;
            if (_elementOffset == 0)
            {
                //handle child node
                if (_node.FirstChildNode is not null)
                    return FetchPosition(_node, offset, _direction);
                if (_edge == ElementEdge.BeforeStart || _edge == ElementEdge.AfterEnd)
                    offset -= direction == LogicalDirection.Backward ? _node.EndSymbolCount : _node.StartSymbolCount;
            }
            //inner edge of node
            if (offset == 0)
                return new TextPointer(_textContainer, _node, direction == LogicalDirection.Backward ? ElementEdge.BeforeEnd : ElementEdge.AfterStart, direction);
            int count;
            if (direction == _direction)
                count = _node.GetSymbolCount(_textContainer.Generation) - _elementOffset;
            else
                count = _elementOffset;
            //inside node
            if (offset < count)
            {
                if (direction == _direction)
                    return new TextPointer(_textContainer, _node, _edge, direction, _elementOffset + offset);
                else
                    return new TextPointer(_textContainer, _node, _edge, direction, _elementOffset - offset);
            }
            offset -= count;
            //inner edge of node
            if (offset == 0)
            {
                if (direction == LogicalDirection.Backward && _node.StartSymbolCount != 0)
                    return new TextPointer(_textContainer, _node, ElementEdge.AfterStart, direction);
                else if (direction == LogicalDirection.Forward && _node.EndSymbolCount != 0)
                    return new TextPointer(_textContainer, _node, ElementEdge.BeforeEnd, direction);
            }
            offset -= direction == LogicalDirection.Backward ? _node.StartSymbolCount : _node.EndSymbolCount;
            //outer edge of node
            if (offset == 0)
                return new TextPointer(_textContainer, _node, direction == LogicalDirection.Backward ? ElementEdge.BeforeStart : ElementEdge.AfterEnd, direction);
            //other node
            return FetchPosition(offset, direction);
        }

        private TextPointer? FetchPosition(int offset, LogicalDirection direction)
        {
            return FetchPosition(FetchNode(_node, direction), offset, direction);
        }

        private TextPointer? FetchPosition(TextTreeNode? node, int offset, LogicalDirection direction)
        {
            while (node != null)
            {
                int symbolCount;
                if (node.Generation == _textContainer.Generation && node.SymbolCountCache.HasValue)
                    symbolCount = node.SymbolCountCache.Value;
                else if (node.FirstChildNode == null)
                    symbolCount = node.GetSymbolCount(_textContainer.Generation);
                else
                {
                    offset -= direction == LogicalDirection.Backward ? node.StartSymbolCount : node.EndSymbolCount;
                    node = direction == LogicalDirection.Backward ? node.LastChildNode : node.FirstChildNode;
                    continue;
                }
                //inside node
                if (symbolCount > offset)
                {
                    if (node.FirstChildNode == null)
                        return new TextPointer(_textContainer, node, direction == LogicalDirection.Backward ? ElementEdge.BeforeEnd : ElementEdge.AfterStart, direction, offset);
                    offset -= direction == LogicalDirection.Backward ? node.StartSymbolCount : node.EndSymbolCount;
                    node = direction == LogicalDirection.Backward ? node.LastChildNode : node.FirstChildNode;
                    continue;
                }
                offset -= symbolCount;
                //inner edge of node
                if (offset == 0)
                {
                    if (direction == LogicalDirection.Backward && node.StartSymbolCount != 0)
                        return new TextPointer(_textContainer, node, ElementEdge.AfterStart, direction);
                    else if (direction == LogicalDirection.Forward && node.EndSymbolCount != 0)
                        return new TextPointer(_textContainer, node, ElementEdge.BeforeEnd, direction);
                }
                offset -= direction == LogicalDirection.Backward ? node.StartSymbolCount : node.EndSymbolCount;
                //outer edge of node
                if (offset == (direction == LogicalDirection.Backward ? node.StartSymbolCount : node.EndSymbolCount))
                    return new TextPointer(_textContainer, node, direction == LogicalDirection.Backward ? ElementEdge.BeforeStart : ElementEdge.AfterEnd, direction);
                //fetch next node
                node = FetchNode(node, direction);
            }
            return null;
        }

        private TextTreeNode? FetchNode(TextTreeNode node, LogicalDirection direction)
        {
            if (direction == LogicalDirection.Backward)
            {
                if (node.PreviousNode != null)
                    return node.PreviousNode;
                if (node.ParentNode == null)
                    return null;
                return node.ParentNode.PreviousNode;
            }
            else
            {
                if (node.NextNode != null)
                    return node.NextNode;
                if (node.ParentNode == null)
                    return null;
                return node.ParentNode.NextNode;
            }
        }

        public int GetOffsetToPosition(TextPointer position)
        {
            //same node, compare offset
            if (position._node == _node)
                return position.GetOffsetAlignToStart() - GetOffsetAlignToStart();
            if (_textContainer != position._textContainer)
                throw new InvalidOperationException("Position not belong to same text container.");
            var leftSymbolOffset = _node.GetSymbolOffset(_textContainer.Generation) + GetOffsetAlignToStart();
            var rightSymbolOffset = position._node.GetSymbolOffset(_textContainer.Generation) + position.GetOffsetAlignToStart();
            return rightSymbolOffset - leftSymbolOffset;
        }

        private int GetOffsetAlignToStart()
        {
            if (_edge == ElementEdge.BeforeStart)
                return 0;
            else if (_edge == ElementEdge.AfterStart)
                return _node.StartSymbolCount + _elementOffset;
            else if (_edge == ElementEdge.BeforeEnd)
                return _node.StartSymbolCount + _node.GetSymbolCount(_textContainer.Generation) - _elementOffset;
            else
                return _node.StartSymbolCount + _node.GetSymbolCount(_textContainer.Generation) + _node.EndSymbolCount;
        }

        #region Operators

        public override int GetHashCode()
        {
            return _node.GetHashCode() ^ _edge.GetHashCode() ^ _elementOffset.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is TextPointer pointer)
                return pointer == this;
            return false;
        }

        public static bool operator ==(TextPointer? left, TextPointer? right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            if (left._node != right._node)
                return false;
            if ((left._edge == ElementEdge.BeforeStart || left._edge == ElementEdge.AfterEnd) && left._edge != right._edge)
                return false;
            if (left._edge == right._edge && left._elementOffset == right._elementOffset)
                return true;
            if (left._edge != right._edge && left._elementOffset == 0 && right._elementOffset == 0)
                return false;
            var symbolCount = left._node.GetSymbolCount(left._textContainer.Generation);
            if (left._edge == ElementEdge.AfterStart)
                return left._elementOffset == symbolCount - right._elementOffset;
            else
                return right._elementOffset == symbolCount - left._elementOffset;
        }

        public static bool operator !=(TextPointer? left, TextPointer? right)
        {
            if (left is null && right is null)
                return false;
            if (left is null || right is null)
                return true;
            if (left._node != right._node)
                return true;
            if ((left._edge == ElementEdge.BeforeStart || left._edge == ElementEdge.AfterEnd) && left._edge != right._edge)
                return true;
            if (left._edge == right._edge && left._elementOffset == right._elementOffset)
                return false;
            if (left._edge != right._edge && left._elementOffset == 0 && right._elementOffset == 0)
                return true;
            var symbolCount = left._node.GetSymbolCount(left._textContainer.Generation);
            if (left._edge == ElementEdge.AfterStart)
                return left._elementOffset != symbolCount - right._elementOffset;
            else
                return right._elementOffset != symbolCount - left._elementOffset;
        }

        public static bool operator <(TextPointer? left, TextPointer? right)
        {
            if (left is null || right is null)
                return false;
            if (left._textContainer != right._textContainer)
                return false;
            if (left._node == right._node)
                return left.GetOffsetToPosition(right) > 0;
            var generation = left._textContainer.Generation;
            var commonParent = TextTreeNode.FindCommonParent(generation, left.Node, right.Node, out var leftPath, out var rightPath);
            if (commonParent == left._node)
                return left._edge == ElementEdge.BeforeStart || left._edge == ElementEdge.AfterStart;
            else if (commonParent == right._node)
                return right._edge == ElementEdge.BeforeEnd || right._edge == ElementEdge.AfterEnd;
            var leftNode = leftPath[leftPath.Count - 1];
            var rightNode = rightPath[rightPath.Count - 1];
            if (leftNode.Generation == generation && leftNode.SymbolOffsetCache.HasValue && rightNode.Generation == generation && rightNode.SymbolOffsetCache.HasValue)
                return leftNode.SymbolOffsetCache.Value < rightNode.SymbolOffsetCache.Value;
            while (rightNode.PreviousNode != null)
            {
                if (rightNode.PreviousNode == leftNode)
                    return true;
                rightNode = rightNode.PreviousNode;
            }
            return false;
        }

        public static bool operator <=(TextPointer? left, TextPointer? right)
        {
            if (left is null || right is null)
                return false;
            if (left._textContainer != right._textContainer)
                return false;
            if (left._node == right._node)
                return left.GetOffsetToPosition(right) > 0;
            var generation = left._textContainer.Generation;
            var commonParent = TextTreeNode.FindCommonParent(generation, left.Node, right.Node, out var leftPath, out var rightPath);
            if (commonParent == left._node)
                return left._edge == ElementEdge.BeforeStart || left._edge == ElementEdge.AfterStart;
            else if (commonParent == right._node)
                return right._edge == ElementEdge.BeforeEnd || right._edge == ElementEdge.AfterEnd;
            var leftNode = leftPath[leftPath.Count - 1];
            var rightNode = rightPath[rightPath.Count - 1];
            if (leftNode.Generation == generation && leftNode.SymbolOffsetCache.HasValue && rightNode.Generation == generation && rightNode.SymbolOffsetCache.HasValue)
                return leftNode.SymbolOffsetCache.Value < rightNode.SymbolOffsetCache.Value;
            while (rightNode.PreviousNode != null)
            {
                if (rightNode.PreviousNode == leftNode)
                    return true;
                rightNode = rightNode.PreviousNode;
            }
            return false;
        }

        public static bool operator >(TextPointer? left, TextPointer? right)
        {
            if (left is null || right is null)
                return false;
            if (left._textContainer != right._textContainer)
                return false;
            if (left._node == right._node)
                return left.GetOffsetToPosition(right) < 0;
            var generation = left._textContainer.Generation;
            var commonParent = TextTreeNode.FindCommonParent(generation, left.Node, right.Node, out var leftPath, out var rightPath);
            if (commonParent == left._node)
                return left._edge == ElementEdge.BeforeEnd || left._edge == ElementEdge.AfterEnd;
            else if (commonParent == right._node)
                return right._edge == ElementEdge.BeforeStart || right._edge == ElementEdge.AfterStart;
            var leftNode = leftPath[leftPath.Count - 1];
            var rightNode = rightPath[rightPath.Count - 1];
            if (leftNode.Generation == generation && leftNode.SymbolOffsetCache.HasValue && rightNode.Generation == generation && rightNode.SymbolOffsetCache.HasValue)
                return leftNode.SymbolOffsetCache.Value > rightNode.SymbolOffsetCache.Value;
            while (leftNode.PreviousNode != null)
            {
                if (leftNode.PreviousNode == rightNode)
                    return true;
                leftNode = leftNode.PreviousNode;
            }
            return false;
        }

        public static bool operator >=(TextPointer? left, TextPointer? right)
        {
            if (left is null || right is null)
                return false;
            if (left._textContainer != right._textContainer)
                return false;
            if (left._node == right._node)
                return left.GetOffsetToPosition(right) <= 0;
            var generation = left._textContainer.Generation;
            var commonParent = TextTreeNode.FindCommonParent(generation, left.Node, right.Node, out var leftPath, out var rightPath);
            if (commonParent == left._node)
                return left._edge == ElementEdge.BeforeEnd || left._edge == ElementEdge.AfterEnd;
            else if (commonParent == right._node)
                return right._edge == ElementEdge.BeforeStart || right._edge == ElementEdge.AfterStart;
            var leftNode = leftPath[leftPath.Count - 1];
            var rightNode = rightPath[rightPath.Count - 1];
            if (leftNode.Generation == generation && leftNode.SymbolOffsetCache.HasValue && rightNode.Generation == generation && rightNode.SymbolOffsetCache.HasValue)
                return leftNode.SymbolOffsetCache.Value > rightNode.SymbolOffsetCache.Value;
            while (leftNode.PreviousNode != null)
            {
                if (leftNode.PreviousNode == rightNode)
                    return true;
                leftNode = leftNode.PreviousNode;
            }
            return false;
        }

        #endregion
    }
}
