using LemonPlatform.Module.DataStructure.Helpers;
using LemonPlatform.Module.DataStructure.Messages;

namespace LemonPlatform.Module.DataStructure.Models.RB
{
    public class RBTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private RBTreeNode<TKey, TValue> _root;
        private int _size;

        public RBTree()
        {
            _root = null;
            _size = 0;
        }

        public RBTreeNode<TKey, TValue> GetRoot() => _root;

        public int GetSize() => _size;

        public bool IsEmpty => _size == 0;

        public bool IsRed(RBTreeNode<TKey, TValue> node)
        {
            if (node == null) return false;
            return node.Color == RBTreeColor.Red;
        }

        //   node                     x
        //  /   \     左旋转         /  \
        // T1   x   --------->   node   T3
        //     / \              /   \
        //    T2 T3            T1   T2
        private RBTreeNode<TKey, TValue> LeftRotate(RBTreeNode<TKey, TValue> node)
        {
            var x = node.Right;

            node.Right = x.Left;
            x.Left = node;

            x.Color = node.Color;
            node.Color = RBTreeColor.Red;

            return x;
        }

        //     node                   x
        //    /   \     右旋转       /  \
        //   x    T2   ------->   y   node
        //  / \                       /  \
        // y  T1                     T1  T2
        private RBTreeNode<TKey, TValue> RightRotate(RBTreeNode<TKey, TValue> node)
        {
            var x = node.Left;

            node.Left = x.Right;
            x.Right = node;

            x.Color = node.Color;
            node.Color = RBTreeColor.Red;

            return x;
        }

        /// <summary>
        /// 颜色翻转
        /// </summary>
        /// <param name="node"></param>
        private void FlipColor(RBTreeNode<TKey, TValue> node)
        {
            node.Color = RBTreeColor.Red;
            node.Left.Color = RBTreeColor.Black;
            node.Right.Color = RBTreeColor.Black;
        }

        public void Add(TKey key, TValue value)
        {
            if (_root != null)
            {
                DataMessageHelper.SendMessage(RenderType.RBTree, $"{_root.Key} - Origin Root");
            }

            _root = Add(_root, key, value);
            _root.Color = RBTreeColor.Black;
        }

        private RBTreeNode<TKey, TValue> Add(RBTreeNode<TKey, TValue> node, TKey key, TValue value)
        {
            if (node == null)
            {
                _size++;
                return new RBTreeNode<TKey, TValue>(key, value);
            }

            DataMessageHelper.SendMessage(RenderType.RBTree, $"{node.Key} - Add Key {key}");

            if (key.CompareTo(node.Key) < 0)
            {
                DataMessageHelper.SendMessage(RenderType.RBTree, $"{node.Key} - Add Left");
                node.Left = Add(node.Left, key, value);
            }
            else if (key.CompareTo(node.Key) > 0)
            {
                DataMessageHelper.SendMessage(RenderType.RBTree, $"{node.Key} - Add Right");
                node.Right = Add(node.Right, key, value);
            }
            else
            {
                DataMessageHelper.SendMessage(RenderType.RBTree, $"{node.Key} - Update Value");
                node.Value = value;
            }

            if (IsRed(node.Right) && !IsRed(node.Left))
            {
                DataMessageHelper.SendMessage(RenderType.RBTree, $"{node.Key} - Left Rotate");
                node = LeftRotate(node);
            }

            if (IsRed(node.Left) && IsRed(node.Left.Left))
            {
                DataMessageHelper.SendMessage(RenderType.RBTree, $"{node.Key} - Right Rotate");
                node = RightRotate(node);
            }

            if (IsRed(node.Left) && IsRed(node.Right))
            {
                DataMessageHelper.SendMessage(RenderType.RBTree, $"{node.Key} - Flip Color");
                FlipColor(node);
            }

            return node;
        }
    }
}