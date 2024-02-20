using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Taki.Game.Database
{
    internal abstract class AbstractDatabase<T> : IDatabase<T>
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<T> _collection;

        public AbstractDatabase(IConfiguration configuration, string collectionName) 
        {
            var mongoUrl = configuration.GetSection("MongoUrl").Value ??
                throw new NullReferenceException("please define mongoDB url");
            _client = new MongoClient(mongoUrl);
            
            var dbName = configuration.GetSection("MongoDatabaseName").Value ??
                throw new NullReferenceException("please define mongoDB url");
            _database = _client.GetDatabase(dbName);

            _collection = _database.GetCollection<T>(collectionName);
        }

        public bool CloseDB()
        {
            _client.Cluster.Dispose();
            return true;
        }

        public bool Create(T value)
        {
            _collection.InsertOne(value);
            return true;
        }

        public bool DeletAll()
        {
            //TODO: delete all instances of the db
            return true;
        }

        public bool Delete(string key, string val)
        {
            var deleteFilter = Builders<T>.Filter.Eq(key, val);
            _collection.DeleteOne(deleteFilter);
            return true;
        }

        public bool IsEmpty()
        {
            //TODO: create is empty => check if the db is currently empty
            return true;
        }

        public T Read(string key, string val)
        {
            var filter = Builders<T>.Filter.Eq(key, val);
            var result = _collection.Find(filter).ToList();
            return result.First();
        }

        public void Update(string key, string val, T newValue)
        {
            var updateFilter = Builders<T>.Filter.Eq("key", "value");
            var update = Builders<T>.Update.Set("key", "updatedValue");
            _collection.UpdateOne(updateFilter, update);
        }
    }
}
