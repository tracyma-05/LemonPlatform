﻿using LemonPlatform.Module.DataStructure.Helpers;
using LemonPlatform.Module.DataStructure.Messages;
using SkiaSharp;

namespace LemonPlatform.Module.DataStructure.Models.AVL
{
    public class AVLTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private AVLNode<TKey, TValue>? _root;
        private int _size;

        public AVLTree()
        {
            _root = null;
            _size = 0;
        }

        public AVLNode<TKey, TValue> GetRoot() => _root;

        public int GetSize() => _size;

        public bool IsEmpty => _size == 0;

        // 对节点y进行向右旋转操作，返回旋转后新的根节点x
        //        y                              x
        //       / \                           /   \
        //      x   T4     向右旋转 (y)        z     y
        //     / \       - - - - - - - ->    / \   / \
        //    z   T3                       T1  T2 T3 T4
        //   / \
        // T1   T2
        private AVLNode<TKey, TValue> RightRotate(AVLNode<TKey, TValue> y)
        {
            var x = y.Left;
            var t3 = x.Right;

            x.Right = y;
            y.Left = t3;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x;
        }

        // 对节点y进行向左旋转操作，返回旋转后新的根节点x
        //    y                             x
        //  /  \                          /   \
        // T1   x      向左旋转 (y)       y     z
        //     / \   - - - - - - - ->   / \   / \
        //   T2  z                     T1 T2 T3 T4
        //      / \
        //     T3 T4
        private AVLNode<TKey, TValue> LeftRotate(AVLNode<TKey, TValue> y)
        {
            var x = y.Right;
            var t2 = x.Left;

            x.Left = y;
            y.Right = t2;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x;
        }

        public async Task Add(TKey key, TValue value, int delay, bool isDebug, EventHandler eventHandler)
        {
            _root = await Add(_root, key, value, delay, isDebug, eventHandler);
        }

        private void UpdateColor(AVLNode<TKey, TValue>? nodeRoot, SKColor color)
        {
            var queue = new Queue<AVLNode<TKey, TValue>>();
            nodeRoot.CircleSKColor = color;
            queue.Enqueue(nodeRoot);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node.Left != null)
                {
                    node.Left.CircleSKColor = color;
                    queue.Enqueue(node.Left);
                }

                if (node.Right != null)
                {
                    node.Right.CircleSKColor = color;
                    queue.Enqueue(node.Right);
                }
            }
        }

        private async Task DebugAVLTreeAsync(AVLNode<TKey, TValue>? node, int delay, bool isDebug, EventHandler eventHandler, SKColor color)
        {
            if (isDebug)
            {
                UpdateColor(node, color);
                eventHandler.Invoke(this, EventArgs.Empty);
                await Task.Delay(delay);
            }
        }

        private async Task DebugNodeAVLTreeAsync(AVLNode<TKey, TValue>? node, int delay, bool isDebug, EventHandler eventHandler, SKColor color)
        {
            if (isDebug)
            {
                node.CircleSKColor = color;
                eventHandler.Invoke(this, EventArgs.Empty);
                await Task.Delay(delay);
            }
        }

        private async Task<AVLNode<TKey, TValue>> Add(AVLNode<TKey, TValue>? node, TKey key, TValue value, int delay, bool isDebug, EventHandler eventHandler)
        {
            if (node == null)
            {
                _size++;
                return new AVLNode<TKey, TValue>(key, value);
            }

            DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - Add Key {key}");
            UpdateColor(node, SKColors.Blue);

            await DebugNodeAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Red);
            if (key.CompareTo(node.Key) < 0)
            {
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - Add Left");
                node.Left = await Add(node.Left, key, value, delay, isDebug, eventHandler);
            }
            else if (key.CompareTo(node.Key) > 0)
            {
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - Add Right");
                node.Right = await Add(node.Right, key, value, delay, isDebug, eventHandler);
            }
            else
            {
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - Update Value");
                node.Value = value;
            }

            node.CircleSKColor = SKColors.Blue;
            node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
            var balanceFactor = GetBalanceFactor(node);

            // LL
            if (balanceFactor > 1 && GetBalanceFactor(node.Left) >= 0)
            {
                await DebugAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Brown);
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - LL");
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - Right Rotate");
                var result = RightRotate(node);
                await DebugAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Green);

                return result;
            }

            // RR
            if (balanceFactor < -1 && GetBalanceFactor(node.Right) <= 0)
            {
                await DebugAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Brown);
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - RR");
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - Left Rotate");
                var result = LeftRotate(node);
                await DebugAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Green);

                return result;
            }

            // LR
            if (balanceFactor > 1 && GetBalanceFactor(node.Left) < 0)
            {
                await DebugAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Brown);
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - LR");
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Left.Key} - Left Rotate");

                node.Left = LeftRotate(node.Left);

                await DebugAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Green);
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - Right Rotate");

                var result = RightRotate(node);
                await DebugAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Yellow);

                return result;
            }

            // RL
            if (balanceFactor < -1 && GetBalanceFactor(node.Right) > 0)
            {
                await DebugAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Brown);
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - RL");
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Right.Key} - Right Rotate");

                node.Right = RightRotate(node.Right);
                await DebugAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Green);

                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - Left Rotate");
                var result = LeftRotate(node);
                await DebugAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Yellow);

                return result;
            }

            return node;
        }

        public async Task<bool> Contains(TKey key)
        {
            await Task.Delay(100);
            return GetNode(_root, key) != null;
        }

        public TValue? Get(TKey key)
        {
            var node = GetNode(_root, key);
            return node == null ? default : node.Value;
        }

        public void Set(TKey key, TValue value)
        {
            var node = GetNode(_root, key);
            if (node == null) throw new ArgumentException($"{key} doesn't exist!");
            node.Value = value;
        }

        public async Task<TValue?> Remove(TKey key, int delay, bool isDebug, EventHandler eventHandler)
        {
            var node = GetNode(_root, key);
            if (node == null) return default;

            _root = await Remove(_root, key, delay, isDebug, eventHandler);
            return node.Value;
        }

        private async Task<AVLNode<TKey, TValue>?> Remove(AVLNode<TKey, TValue>? node, TKey key, int delay, bool isDebug, EventHandler eventHandler)
        {
            if (node == null) return default;
            AVLNode<TKey, TValue>? retNode = default;
            DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - Remove Key {key}");
            UpdateColor(node, SKColors.Blue);

            await DebugNodeAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Red);
            if (key.CompareTo(node.Key) < 0)
            {
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - Remove Left");
                node.Left = await Remove(node.Left, key, delay, isDebug, eventHandler);
                retNode = node;
            }
            else if (key.CompareTo(node.Key) > 0)
            {
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - Remove Right");
                node.Right = await Remove(node.Right, key, delay, isDebug, eventHandler);
                retNode = node;
            }
            else
            {
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - Remove Value");
                if (node.Left == null)
                {
                    var rightNode = node.Right;
                    node.Right = null;
                    _size--;

                    retNode = rightNode;
                }
                else if (node.Right == null)
                {
                    var leftNode = node.Left;
                    node.Left = null;
                    _size--;

                    retNode = leftNode;
                }
                else
                {
                    var successor = GetMinimum(node.Right);
                    successor.Right = await Remove(successor.Right, key, delay, isDebug, eventHandler);
                    successor.Left = node.Left;

                    node.Left = node.Right = null;
                    retNode = successor;
                }
            }

            if (retNode == null) return retNode;

            node.CircleSKColor = SKColors.Blue;
            retNode.Height = Math.Max(GetHeight(retNode.Left), GetHeight(retNode.Right)) + 1;
            var balanceFactor = GetBalanceFactor(retNode);

            // LL
            if (balanceFactor > 1 && GetBalanceFactor(retNode.Left) >= 0)
            {
                await DebugAVLTreeAsync(retNode, delay, isDebug, eventHandler, SKColors.Brown);
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{retNode.Key} - LL");
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{retNode.Key} - Right Rotate");
                var result = RightRotate(retNode);
                await DebugAVLTreeAsync(retNode, delay, isDebug, eventHandler, SKColors.Green);
                return result;
            }

            // RR
            if (balanceFactor < -1 && GetBalanceFactor(retNode.Right) <= 0)
            {
                await DebugAVLTreeAsync(retNode, delay, isDebug, eventHandler, SKColors.Brown);
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{retNode.Key} - RR");
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{retNode.Key} - Left Rotate");
                var result = LeftRotate(retNode);
                await DebugAVLTreeAsync(retNode, delay, isDebug, eventHandler, SKColors.Green);
                return result;
            }

            // LR
            if (balanceFactor > 1 && GetBalanceFactor(retNode.Left) < 0)
            {
                await DebugAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Brown);
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{retNode.Key} - LR");
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{retNode.Left.Key} - Left Rotate");
                retNode.Left = LeftRotate(retNode.Left!);

                await DebugAVLTreeAsync(node, delay, isDebug, eventHandler, SKColors.Green);
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{node.Key} - Right Rotate");
                var result = RightRotate(retNode);

                await DebugAVLTreeAsync(retNode, delay, isDebug, eventHandler, SKColors.Yellow);
                return result;
            }

            // RL
            if (balanceFactor < -1 && GetBalanceFactor(retNode.Right) > 0)
            {
                await DebugAVLTreeAsync(retNode, delay, isDebug, eventHandler, SKColors.Brown);
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{retNode.Key} - RL");
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{retNode.Right.Key} - Right Rotate");
                retNode.Right = RightRotate(retNode.Right!);

                await DebugAVLTreeAsync(retNode, delay, isDebug, eventHandler, SKColors.Green);
                DataMessageHelper.SendMessage(RenderType.AVLTree, $"{retNode.Key} - Left Rotate");
                var result = LeftRotate(retNode);

                await DebugAVLTreeAsync(retNode, delay, isDebug, eventHandler, SKColors.Yellow);

                return result;
            }

            return retNode;
        }

        private AVLNode<TKey, TValue> GetMinimum(AVLNode<TKey, TValue> node)
        {
            if (node.Left == null) return node;
            return GetMinimum(node.Left);
        }

        private AVLNode<TKey, TValue> GetMaximum(AVLNode<TKey, TValue> node)
        {
            if (node.Right == null) return node;
            return GetMaximum(node.Right);
        }

        private AVLNode<TKey, TValue>? GetNode(AVLNode<TKey, TValue>? node, TKey key)
        {
            if (node == null) return null;
            if (key.Equals(node.Key)) return node;
            else if (key.CompareTo(node.Key) < 0) return GetNode(node.Left, key);
            else return GetNode(node.Right, key);
        }

        public bool IsBST()
        {
            var keys = new List<TKey>();
            InOrder(_root, keys);
            for (int i = 1; i < keys.Count; i++)
            {
                if (keys[i - 1].CompareTo(keys[i]) > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void InOrder(AVLNode<TKey, TValue>? node, IList<TKey> keys)
        {
            if (node == null) return;
            InOrder(node.Left, keys);
            keys.Add(node.Key);
            InOrder(node.Right, keys);
        }

        private bool IsBalanced(AVLNode<TKey, TValue>? node)
        {
            if (node == null) return true;
            var balanceFactor = GetBalanceFactor(node);
            if (Math.Abs(balanceFactor) > 1) return false;
            return IsBalanced(node.Left) && IsBalanced(node.Right);
        }

        private int GetBalanceFactor(AVLNode<TKey, TValue>? node)
        {
            if (node == null) return 0;
            return GetHeight(node.Left) - GetHeight(node.Right);

        }

        private int GetHeight(AVLNode<TKey, TValue>? node)
        {
            if (node == null) return 0;
            return node.Height;
        }
    }
}