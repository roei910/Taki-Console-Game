using MongoDB.Bson;
using MongoDB.Driver;
using Taki.Dal;
using TakiApp.Shared.Models;

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

        public async override Task DeleteManyAsync(List<GameSettings> values)
        {
            var listOfIds = values.Select(x => x.Id).ToList();
            var filter = Builders<GameSettings>.Filter.In(x => x.Id, listOfIds);

            await _collection.DeleteManyAsync(filter);
        }

        public async override Task<GameSettings> FindOneAsync(ObjectId objectId)
        {
            var filter = Builders<GameSettings>.Filter.Eq(x => x.Id, objectId);
            var result = await _collection.FindAsync(filter);

            return result.First();
        }

        public override Task UpdateManyAsync(List<GameSettings> valuesToUpdate)
        {
            throw new NotImplementedException();
        }

        public async override Task UpdateOneAsync(GameSettings valueToUpdate)
        {
            var filter = Builders<GameSettings>.Filter.Eq(x => x.Id, valueToUpdate.Id);
            var update = Builders<GameSettings>.Update
                .Set(x => x.NumberOfPlayerCards, valueToUpdate.NumberOfPlayerCards)
                .Set(x => x.HasGameStarted, valueToUpdate.HasGameStarted)
                .Set(x => x.HasGameEnded, valueToUpdate.HasGameEnded)
                .Set(x => x.winners, valueToUpdate.winners);

            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
