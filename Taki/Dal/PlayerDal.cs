using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Taki.Dto;

namespace Taki.Repository
{
    internal class PlayerDal : MongoDal<PlayerDto>
    {
        public Func<int, FilterDefinition<PlayerDto>> FilterById =
            (id) => Builders<PlayerDto>.Filter.Eq(player => player.Id, id);

        public Func<string, FilterDefinition<PlayerDto>> FilterByName =
            (name) => Builders<PlayerDto>.Filter.Eq(player => player.Name, name);

        public PlayerDal(IConfiguration configuration) :
            base(configuration, "Players")
        { }

        public PlayerDal(string mongoUrl, string dbName, string collectionName) :
            base(mongoUrl, dbName, collectionName)
        { }

        public override bool Delete(int id)
        {
            return Delete(FilterById(id));
        }

        public override bool DeleteAll()
        {
            FindAll().ForEach(player => Delete(FilterById(player.Id)));
            return true;
        }

        public override void UpdateAll(List<PlayerDto> players)
        {
            players.ForEach(UpdateOne);
        }

        public override void UpdateOne(PlayerDto playerToUpdate)
        {
            Delete(FilterById(playerToUpdate.Id));
            Create(playerToUpdate);
        }
    }
}
