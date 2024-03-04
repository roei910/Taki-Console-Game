using MongoDB.Bson;
using MongoDB.Driver;
using Taki.Dal;
using TakiApp.Models;

namespace TakiApp.Dal
{
    public class GameSettingsDal : MongoDal<GameSettings>
    {
        public GameSettingsDal(MongoDbConfig configuration) : 
            base(configuration, configuration.GameSettingsCollectionName) { }

        public async override Task DeleteAsync(GameSettings value)
        {
            var filter = Builders<GameSettings>.Filter.Eq(x => x.Id, value.Id);
            await _collection.DeleteOneAsync(filter);
        }

        public async override Task<GameSettings> FindOneAsync(ObjectId objectId)
        {
            var filter = Builders<GameSettings>.Filter.Eq(x => x.Id, objectId);
            var result = await _collection.FindAsync(filter);

            return result.First();
        }

        public async override Task UpdateOneAsync(GameSettings valueToUpdate)
        {
            var filter = Builders<GameSettings>.Filter.Eq(x => x.Id, valueToUpdate.Id);
            var update = Builders<GameSettings>.Update
                .Set(x => x.NumberOfPlayerCards, valueToUpdate.NumberOfPlayerCards)
                .Set(x => x.HasGameStarted, valueToUpdate.HasGameStarted);

            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
