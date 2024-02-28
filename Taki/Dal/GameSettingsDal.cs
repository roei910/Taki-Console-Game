using MongoDB.Driver;
using Taki.Shared.Models;

namespace Taki.Dal
{
    internal class GameSettingsDal : MongoDal<GameSettings>
    {
        public GameSettingsDal(MongoDbConfig configuration) : 
            base(configuration, configuration.GameSettingsCollectionName!) { }

        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteAll()
        {
            var filter = Builders<GameSettings>.Filter.Empty;
            _collection.DeleteMany(filter);

            return true;
        }

        public override void UpdateOne(GameSettings value)
        {
            var filter = Builders<GameSettings>.Filter.Eq(g => g.Id, value.Id);
            var update = Builders<GameSettings>.Update
                .Set(g => g.NumberOfPlayerCards, value.NumberOfPlayerCards);

            _collection.UpdateOne(filter, update);
        }
    }
}
