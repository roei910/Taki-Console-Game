using MongoDB.Bson;

namespace TakiApp.Interfaces
{
    public interface IDal<T>
    {
        Task CreateOneAsync(T value);
        Task CreateManyAsync(List<T> values);
        Task UpdateOneAsync(T valueToUpdate);
        Task<List<T>> FindAsync();
        Task<T> FindOneAsync(ObjectId objectId);
        Task DeleteAsync(T value);
        Task DeleteManyAsync(List<T> values);
        Task DeleteAllAsync();
    }
}