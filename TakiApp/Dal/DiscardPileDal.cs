using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Dal
{
    public class DiscardPileDal : CardDal, IDiscardPileDal
    {
        public DiscardPileDal(MongoDbConfig configuration) : 
            base(configuration, configuration.DiscardPileCollectionName) { }
    }
}
