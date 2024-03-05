using MongoDB.Bson;
using MongoDB.Driver;
using Taki.Dal;
using TakiApp.Models;

namespace TakiApp.Dal
{
    public class PlayersDal : MongoDal<Player>
    {
        public PlayersDal(MongoDbConfig configuration) :
            base(configuration, configuration.PlayersCollectionName)
        { }

        public async override Task DeleteAsync(Player value)
        {
            var filter = Builders<Player>.Filter.Eq(x => x.Id, value.Id);
            await _collection.DeleteOneAsync(filter);
        }

        public async override Task DeleteManyAsync(List<Player> values)
        {
            var listOfIds = values.Select(x => x.Id).ToList();
            var filter = Builders<Player>.Filter.In(x => x.Id, listOfIds);

            await _collection.DeleteManyAsync(filter);
        }

        public async override Task<Player> FindOneAsync(ObjectId id)
        {
            var filter = Builders<Player>.Filter.Eq(x => x.Id, id);
            var found = await _collection.FindAsync(filter);

            return found.First();
        }

        public async override Task UpdateOneAsync(Player valueToUpdate)
        {
            var filter = Builders<Player>.Filter.Eq(x => x.Id, valueToUpdate.Id);
            var update = Builders<Player>.Update
                .Set(x => x.LastCheckIn, DateTime.UtcNow)
                .Set(x => x.Cards, valueToUpdate.Cards)
                .Set(x => x.IsPlaying, valueToUpdate.IsPlaying);

            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
