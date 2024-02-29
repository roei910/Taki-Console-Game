using MongoDB.Bson;
using MongoDB.Driver;
using Taki.Dal;
using TakiApp.Models;

namespace TakiApp.Dal
{
    internal class CardDal : MongoDal<Card>
    {
        public CardDal(MongoDbConfig configuration, string collectionName) :
            base(configuration, collectionName) { }

        public async override Task DeleteAsync(Card value)
        {
            var filter = Builders<Card>.Filter.Eq(x => x.Id, value.Id);
            await _collection.DeleteOneAsync(filter);
        }

        public async override Task<Card> FindOneAsync(ObjectId id)
        {
            var filter = Builders<Card>.Filter.Eq(x => x.Id, id);
            var found = await _collection.FindAsync(filter);

            return found.First();
        }

        public async override Task UpdateOneAsync(Card valueToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
