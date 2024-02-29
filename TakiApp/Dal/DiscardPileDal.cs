using TakiApp.Models;

namespace TakiApp.Dal
{
    internal class DiscardPileDal : CardDal
    {
        public DiscardPileDal(MongoDbConfig configuration) : 
            base(configuration, configuration.DiscardPileCollectionName) { }
    }
}
