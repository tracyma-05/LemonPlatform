namespace LemonPlatform.Module.DataStructure.Models.RB
{
    public class RBTreeNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        public TKey Key { get; set; }

        public TValue Value { get; set; }

        public RBTreeNode<TKey, TValue>? Left { get; set; }

        public RBTreeNode<TKey, TValue>? Right { get; set; }

        public RBTreeColor Color { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Level { get; set; }

        public RBTreeNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Left = null;
            Right = null;
            Color = RBTreeColor.Red;
        }
    }
}