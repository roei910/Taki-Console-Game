using MongoDB.Bson;
using MongoDB.Driver;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace Taki.Dal
{
    public abstract class MongoDal<T> : IDal<T>
    {
        protected readonly MongoClient _client;
        protected readonly IMongoDatabase _database;
        protected readonly IMongoCollection<T> _collection;

        public MongoDal(MongoDbConfig configuration, string collectionName)
        {
            var mongoUrl = configuration.MongoUrl;
            _client = new MongoClient(mongoUrl);

            var dbName = configuration.MongoDatabaseName;
            _database = _client.GetDatabase(dbName);

            _collection = _database.GetCollection<T>(collectionName);
        }

        public MongoDal(string mongoUrl, string dbName, string collectionName)
        {
            _client = new MongoClient(mongoUrl);
            _database = _client.GetDatabase(dbName);
            _collection = _database.GetCollection<T>(collectionName);
        }

        public async Task CreateOneAsync(T value)
        {
            await _collection.InsertOneAsync(value);
        }

        public async Task CreateManyAsync(List<T> values)
        {
            await _collection.InsertManyAsync(values);
        }

        public async Task<List<T>> FindAsync()
        {
            var result = await _collection.FindAsync(_ => true);

            return result.ToList();
        }

        public async Task DeleteAllAsync()
        {
            await _collection.DeleteManyAsync(_ => true);
        }

        public abstract Task UpdateOneAsync(T valueToUpdate);
        public abstract Task<T> FindOneAsync(ObjectId objectId);
        public abstract Task DeleteAsync(T value);
        public abstract Task DeleteManyAsync(List<T> values);
        public abstract Task UpdateManyAsync(List<T> valuesToUpdate);
    }
}
