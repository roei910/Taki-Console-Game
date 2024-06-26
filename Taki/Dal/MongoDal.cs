﻿using MongoDB.Driver;
using Taki.Shared.Interfaces;
using Taki.Shared.Models;

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
            var result = _collection.Find(_ => true).ToList();
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

        public abstract void UpdateOne(T value);
        public abstract bool DeleteAll();
        public abstract bool Delete(int id);
    }
}
