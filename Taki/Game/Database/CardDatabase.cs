using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Taki.Game.Cards;

namespace Taki.Game.Database
{
    internal class CardDatabase : AbstractDatabase<CardDTO>
    {
        public CardDatabase(IConfiguration configuration, string collectionName) :
            base(configuration, collectionName) { }

        public CardDatabase(string mongoUrl, string dbName, string collectionName) : 
            base(mongoUrl, dbName, collectionName) { }

        public static FilterDefinition<CardDTO> FilterById(int id)
        {
            return Builders<CardDTO>.Filter.Eq(card => card.Id, id);
        }

        public override bool DeleteAll()
        {
            FindAll().ForEach(card => Delete(FilterById(card.Id)));
            return true;
        }

        //TODO: make better, remove the ones deleted only or add the ones need adding in the end
        public override void UpdateAll(List<CardDTO> values)
        {
            DeleteAll();
            CreateMany(values);
        }

        public override void UpdateOne(CardDTO value)
        {
            throw new NotImplementedException();
        }
    }
}
