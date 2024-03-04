using MongoDB.Bson;
using MongoDB.Driver;
using Taki.Dal;
using TakiApp.Models;

namespace TakiApp.Dal
{
    public class PlayersDal : MongoDal<Player>, IPlayersDal
    {
        public PlayersDal(MongoDbConfig configuration) :
            base(configuration, configuration.PlayersCollectionName)
        { }

        public async override Task DeleteAsync(Player value)
        {
            var filter = Builders<Player>.Filter.Eq(x => x.Id, value.Id);
            await _collection.DeleteOneAsync(filter);
        }

        public async override Task<Player> FindOneAsync(ObjectId id)
        {
            var filter = Builders<Player>.Filter.Eq(x => x.Id, id);
            var found = await _collection.FindAsync(filter);

            return found.First();
        }

        public async override Task UpdateOneAsync(Player valueToUpdate)
        {
            throw new NotImplementedException();
        }

        public async Task WaitTurn(ObjectId playerId)
        {
            while (true)
            {
                var player = await FindOneAsync(playerId);

                if (player != null && player.IsPlaying)
                    return;
                await Task.Run(async() => await Task.Delay(3000));
            }
        }
    }
}
