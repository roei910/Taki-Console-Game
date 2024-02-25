using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Taki.Interfaces;

namespace Taki.Dal
{
    internal abstract class MongoDal<T> : IDal<T>
    {
        protected readonly MongoClient _client;
        protected readonly IMongoDatabase _database;
        protected readonly IMongoCollection<T> _collection;

        public MongoDal(IConfiguration configuration, string collectionName)
        {
            var mongoUrl = configuration.GetSection("MongoUrl").Value ??
                throw new NullReferenceException("please define mongoDB url");
            _client = new MongoClient(mongoUrl);

            var dbName = configuration.GetSection("MongoDatabaseName").Value ??
                throw new NullReferenceException("please define mongoDB url");
            _database = _client.GetDatabase(dbName);

            _collection = _database.GetCollection<T>(collectionName);
        }

        public MongoDal(string mongoUrl, string dbName, string collectionName)
        {
            _client = new MongoClient(mongoUrl);
            _database = _client.GetDatabase(dbName);
            _collection = _database.GetCollection<T>(collectionName);
        }

        public bool CloseDB()
        {
            _client.Cluster.Dispose();
            return true;
        }

        public virtual bool Create(T value)
        {
            _collection.InsertOne(value);
            return true;
        }

        public bool IsEmpty()
        {
            return FindAll().Count == 0;
        }

        public bool CreateMany(List<T> values)
        {
            values.ForEach(val => Create(val));
            return true;
        }

        public List<T> FindAll()
        {
            var filter = Builders<T>.Filter.Empty;
            var result = _collection.Find(filter).ToList();
            return result;
        }

        public T? FindOne(FilterDefinition<T> filterDefinition)
        {
            var result = _collection.Find(filterDefinition).ToList();
            return result.FirstOrDefault();
        }

        public void ReplaceOne(FilterDefinition<T> filterDefinition, T newValue)
        {
            _collection.ReplaceOne(filterDefinition, newValue);
        }

        public bool Delete(FilterDefinition<T> filterDefinition)
        {
            return _collection.DeleteOne(filterDefinition).IsAcknowledged;
        }

        public abstract void UpdateAll(List<T> values);
        public abstract void UpdateOne(T value);
        public abstract bool DeleteAll();
        public abstract bool Delete(int id);
    }
}
