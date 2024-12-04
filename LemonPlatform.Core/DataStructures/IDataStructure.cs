namespace LemonPlatform.Core.DataStructures
{
    public interface IDataStructure
    {
        void Add(int key);

        void Remove(int key);

        bool IsEmpty();

        bool Contains(int key);

        ICollection<int> Keys { get; set; }
    }
}