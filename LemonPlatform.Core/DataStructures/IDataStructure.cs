namespace LemonPlatform.Core.DataStructures
{
    public interface IDataStructure
    {
        Task AddAsync(int key);

        Task RemoveAsync(int key);

        bool IsEmpty();

        Task<bool> Contains(int key);

        ICollection<int> Keys { get; set; }
    }
}