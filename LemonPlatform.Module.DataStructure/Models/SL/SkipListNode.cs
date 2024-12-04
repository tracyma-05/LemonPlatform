namespace LemonPlatform.Module.DataStructure.Models.SL
{
    public class SkipListNode<T> : IComparable<SkipListNode<T>> where T : IComparable<T>
    {
        public SkipListNode(T value, int level)
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException("Invalid value for level.");

            Value = value;
            Forwards = new SkipListNode<T>[level];
        }

        public virtual T Value { get; private set; }

        public virtual SkipListNode<T>[] Forwards { get; private set; }

        public virtual int Level
        {
            get { return Forwards.Length; }
        }

        public int CompareTo(SkipListNode<T> other)
        {
            if (other == null)
                return -1;

            return Value.CompareTo(other.Value);
        }
    }
}