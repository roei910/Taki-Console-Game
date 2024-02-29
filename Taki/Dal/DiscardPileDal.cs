using Taki.Shared.Models;

namespace Taki.Dal
{
    public class DiscardPileDal : CardDal
    {
        public DiscardPileDal(MongoDbConfig configuration) : 
            base(configuration, configuration.DiscardPileCollectionName) { }
    }
}
