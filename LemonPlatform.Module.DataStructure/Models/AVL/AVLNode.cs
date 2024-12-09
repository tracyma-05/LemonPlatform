namespace LemonPlatform.Module.DataStructure.Models.AVL
{
    public class AVLNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        public TKey Key { get; set; }

        public TValue Value { get; set; }

        public AVLNode<TKey, TValue>? Left { get; set; }

        public AVLNode<TKey, TValue>? Right { get; set; }

        public int Height { get; set; }
        public int Level { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public AVLNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Left = null;
            Right = null;
            Height = 1;
        }
    }
}