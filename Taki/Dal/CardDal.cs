using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Taki.Dto;

namespace Taki.Dal
{
    internal class CardDal : MongoDal<CardDto>
    {
        public CardDal(IConfiguration configuration, string collectionName) :
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

        //TODO: make better, remove the ones deleted only or add the ones need adding in the end
        public override void UpdateAll(List<CardDto> values)
        {
            DeleteAll();
            CreateMany(values);
        }

        public override void UpdateOne(CardDto value)
        {
            throw new NotImplementedException();
        }

        public override bool Create(CardDto value)
        {
            return base.Create(value);
        }

        public override bool Delete(int id)
        {
            return Delete(FilterById(id));
        }
    }
}
