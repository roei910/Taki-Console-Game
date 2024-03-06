using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Dal
{
    public class DiscardPileDal : CardDal, IDiscardPileDal
    {
        public DiscardPileDal(MongoDbConfig configuration) : 
            base(configuration, configuration.DiscardPileCollectionName) { }

        public async Task<List<Card>> GetOrderedCardsAsync()
        {
            var cards = await FindAsync();

            return cards;
        }
    }
}
