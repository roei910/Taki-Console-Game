using MongoDB.Driver;

namespace Taki.Interfaces
{
    internal interface IDatabase<T>
    {
        bool Create(T value);
        bool CreateMany(List<T> values);
        T? FindOne(FilterDefinition<T> filterDefinition);
        List<T> FindAll();
        void ReplaceOne(FilterDefinition<T> filterDefinition, T newValue);
        bool Delete(FilterDefinition<T> filterDefinition);
        bool CloseDB();
        bool DeleteAll();
        bool IsEmpty();
        void UpdateAll(List<T> values);
        void UpdateOne(T value);
    }
}
