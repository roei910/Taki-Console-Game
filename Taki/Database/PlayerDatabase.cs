using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Taki.Dto;

namespace Taki.Database
{
    internal class PlayerDatabase : AbstractDatabase<PlayerDTO>
    {
        public Func<int, FilterDefinition<PlayerDTO>> FilterById =
            (id) => Builders<PlayerDTO>.Filter.Eq(player => player.Id, id);

        public Func<string, FilterDefinition<PlayerDTO>> FilterByName =
            (name) => Builders<PlayerDTO>.Filter.Eq(player => player.Name, name);

        public PlayerDatabase(IConfiguration configuration) :
            base(configuration, "Players")
        { }

        public PlayerDatabase(string mongoUrl, string dbName, string collectionName) :
            base(mongoUrl, dbName, collectionName)
        { }

        public override bool DeleteAll()
        {
            FindAll().ForEach(player => Delete(FilterById(player.Id)));
            return true;
        }

        public override void UpdateAll(List<PlayerDTO> players)
        {
            players.ForEach(UpdateOne);
        }

        public override void UpdateOne(PlayerDTO playerToUpdate)
        {
            Delete(FilterById(playerToUpdate.Id));
            Create(playerToUpdate);
        }
    }
}
