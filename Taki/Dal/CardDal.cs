using MongoDB.Driver;
using Taki.Shared.Models;
using Taki.Shared.Models.Dto;

namespace Taki.Dal
{
    internal class CardDal : MongoDal<CardDto>
    {
        public CardDal(MongoDbConfig configuration, string collectionName) :
            base(configuration, collectionName)
        { }

        public CardDal(string mongoUrl, string dbName, string collectionName) :
            base(mongoUrl, dbName, collectionName)
        { }

        public static FilterDefinition<CardDto> FilterById(int id)
        {
            return Builders<CardDto>.Filter.Eq(card => card.Id, id);
        }

        public override bool DeleteAll()
        {
            FindAll().ForEach(card => Delete(FilterById(card.Id)));
            return true;
        }

        public override void UpdateOne(CardDto value)
        {
            var filter = Builders<CardDto>.Filter.Eq(card => card.Id, value.Id);
            var update = Builders<CardDto>.Update
                .Set(card => card.CardColor, value.CardColor)
                .Set(card => card.CardConfigurations, value.CardConfigurations);

            _collection.UpdateOne(filter, update);
        }

        public override bool Create(CardDto value)
        {
            return base.Create(value);
        }

        public override bool Delete(int id)
        {
            return Delete(FilterById(id));
        }

        //TODO: get only Id and Type
        public CardDto GetIdAndType()
        {
            throw new NotImplementedException();
        }
    }
}
