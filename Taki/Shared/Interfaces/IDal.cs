using MongoDB.Driver;

namespace Taki.Shared.Interfaces
{
    internal interface IDal<T>
    {
        bool Create(T value);
        bool CreateMany(List<T> values);
        T? FindOne(FilterDefinition<T> filterDefinition);
        List<T> FindAll();
        void ReplaceOne(FilterDefinition<T> filterDefinition, T newValue);
        bool Delete(FilterDefinition<T> filterDefinition);
        bool Delete(int id);
        bool CloseDB();
        bool DeleteAll();
        bool IsEmpty();
        void UpdateOne(T value);
    }
}
