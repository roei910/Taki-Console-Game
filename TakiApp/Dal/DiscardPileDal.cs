using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Dal
{
    public class DiscardPileDal : CardDal, IDiscardPileDal
    {
        public DiscardPileDal(MongoDbConfig configuration) : 
            base(configuration, configuration.DiscardPileCollectionName) { }
    }
}
