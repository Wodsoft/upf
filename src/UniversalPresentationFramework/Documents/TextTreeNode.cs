using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    public abstract class TextTreeNode
    {
        private TextTreeNode? _parentNode, _previousNode, _nextNode, _firstChildNode, _lastChildNode;
        private int _childrenCount, _generation;
        private int? _symbolOffsetCache, _symbolCountCache, _treeLevelCache;

        public TextTreeNode? ParentNode => _parentNode;

        public TextTreeNode? PreviousNode => _previousNode;

        public TextTreeNode? NextNode => _nextNode;

        public TextTreeNode? FirstChildNode => _firstChildNode;

        public TextTreeNode? LastChildNode => _lastChildNode;

        public virtual int StartSymbolCount => 1;

        public virtual int EndSymbolCount => 1;

        public virtual int Generation => _generation;

        public int ChildrenCount => _childrenCount;

        protected virtual int InternalSymbolCount => 0;

        public int? SymbolOffsetCache => _symbolOffsetCache;

        public int? SymbolCountCache => _symbolCountCache;

        public int? TreeLevelCache => _treeLevelCache;

        internal int GetSymbolOffset(int generation)
        {
            if (generation == _generation && _symbolOffsetCache.HasValue)
                return _symbolOffsetCache.Value;
            int symbolOffset;
            if (_previousNode != null)
                symbolOffset = _previousNode.GetSymbolOffset(generation) + _previousNode.GetSymbolCount(generation) + _previousNode.StartSymbolCount + _previousNode.EndSymbolCount;
            else if (_parentNode != null)
                symbolOffset = _parentNode.GetSymbolOffset(generation) + _parentNode.StartSymbolCount;
            else
                symbolOffset = 0;
            _symbolOffsetCache = symbolOffset;
            if (generation != _generation)
            {
                _symbolCountCache = null;
                _treeLevelCache = null;
            }
            _generation = generation;
            return symbolOffset;
        }

        /// <summary>
        /// Count of symbols covered by this node and any contained nodes.
        /// Does not include edge symbol.
        /// </summary>
        /// <param name="generation"></param>
        /// <returns></returns>
        internal int GetSymbolCount(int generation)
        {
            if (generation == _generation && _symbolCountCache.HasValue)
                return _symbolCountCache.Value;
            var symbolCount = InternalSymbolCount;
            if (symbolCount != 0)
                return symbolCount;
            var node = _firstChildNode;
            while (node != null)
            {
                symbolCount += node.GetSymbolCount(generation) + node.StartSymbolCount + node.EndSymbolCount;
                node = node._nextNode;
            }
            if (generation != _generation)
            {
                _symbolOffsetCache = null;
                _treeLevelCache = null;
            }
            _symbolCountCache = symbolCount;
            _generation = generation;
            return symbolCount;
        }

        internal int GetTreeLevel(int generation)
        {
            if (generation == _generation && _treeLevelCache.HasValue)
                return _treeLevelCache.Value;
            int treeLevel;
            if (_parentNode == null)
                treeLevel = 0;
            else
                treeLevel = _parentNode.GetTreeLevel(generation) + 1;
            if (generation != _generation)
            {
                _symbolCountCache = null;
                _symbolOffsetCache = null;
            }
            _treeLevelCache = treeLevel;
            _generation = generation;
            return treeLevel;
        }

        protected internal void InsertNodeAt(TextTreeNode node, ElementEdge edge)
        {
            if (node._parentNode != null)
                throw new InvalidOperationException("Node belong to other parent node.");
            switch (edge)
            {
                case ElementEdge.BeforeStart:
                    if (_parentNode == null)
                        throw new InvalidOperationException("Do not support insert node before start at root node.");
                    PrependSibling(node);
                    break;
                case ElementEdge.AfterEnd:
                    if (_parentNode == null)
                        throw new InvalidOperationException("Do not support insert node after end at root node.");
                    AppendSibling(node);
                    break;
                case ElementEdge.AfterStart:
                    PrependChild(node);
                    break;
                case ElementEdge.BeforeEnd:
                    AppendChild(node);
                    break;
                default:
                    throw new NotSupportedException("Unrecognized element edge.");
            }
        }

        protected internal void RemoveFromTree()
        {
            var parentNode = _parentNode;
            if (parentNode == null)
                throw new InvalidOperationException("Node doesn't belong to a tree.");
            if (_previousNode != null && _nextNode != null)
            {
                _previousNode._nextNode = _nextNode;
                _nextNode._previousNode = _previousNode;
            }
            else if (_previousNode != null)
            {
                _previousNode._nextNode = null;
                parentNode._lastChildNode = _previousNode;
            }
            else if (_nextNode != null)
            {
                _nextNode._previousNode = null;
                parentNode._firstChildNode = _nextNode;
            }
            else
            {
                parentNode._firstChildNode = parentNode._lastChildNode = null;
            }
            parentNode._childrenCount--;
            _parentNode = null;
        }

        private void PrependSibling(TextTreeNode node)
        {
            var parentNode = _parentNode;
            node._parentNode = parentNode;
            if (_previousNode != null)
            {
                node._previousNode = _previousNode;
                _previousNode._nextNode = node;
            }
            node._nextNode = this;
            _previousNode = node;
            parentNode!._childrenCount++;
        }

        private void AppendSibling(TextTreeNode node)
        {
            var parentNode = _parentNode;
            node._parentNode = parentNode;
            if (_nextNode != null)
            {
                node._nextNode = _nextNode;
                _nextNode._previousNode = node;
            }
            node._previousNode = this;
            _nextNode = node;
            parentNode!._childrenCount++;
        }

        private void PrependChild(TextTreeNode node)
        {
            node._parentNode = this;
            var firstChildNode = _firstChildNode;
            if (firstChildNode != null)
            {
                node._nextNode = firstChildNode;
                _firstChildNode = firstChildNode._previousNode = node;
            }
            else
            {
                _firstChildNode = _lastChildNode = node;
            }
            _childrenCount++;
        }

        private void AppendChild(TextTreeNode node)
        {
            node._parentNode = this;
            var lastChildNode = _lastChildNode;
            if (lastChildNode != null)
            {
                node._previousNode = lastChildNode;
                _lastChildNode = lastChildNode._nextNode = node;
            }
            else
            {
                _firstChildNode = _lastChildNode = node;
            }
            _childrenCount++;
        }

        internal static TextTreeNode FindCommonParent(int generation, TextTreeNode left, TextTreeNode right, out List<TextTreeNode> leftPath, out List<TextTreeNode> rightPath)
        {
            leftPath = new List<TextTreeNode>() { left };
            rightPath = new List<TextTreeNode>() { right };
            if (left == right)
                return left;
            if (left.ParentNode == right.ParentNode)
                return left.ParentNode!;
            var leftLevel = left.GetTreeLevel(generation);
            var rightLevel = right.GetTreeLevel(generation);
            if (leftLevel > rightLevel)
            {
                for (; leftLevel > rightLevel; leftLevel--)
                {
                    left = left.ParentNode!;
                    leftPath.Add(left);
                }
                if (left == right)
                    return right;
            }
            else if (rightLevel > leftLevel)
            {
                for (; rightLevel > leftLevel; rightLevel--)
                {
                    right = right.ParentNode!;
                    rightPath.Add(right);
                }
                if (right == left)
                    return left;
            }
            while (left.ParentNode != right.ParentNode)
            {
                left = left.ParentNode!;
                right = right.ParentNode!;
                leftPath.Add(left);
                rightPath.Add(right);
            }
            return left.ParentNode!;
        }
    }
}
