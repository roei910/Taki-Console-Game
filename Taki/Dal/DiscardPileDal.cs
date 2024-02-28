using Taki.Shared.Models;

namespace Taki.Dal
{
    internal class DiscardPileDal : CardDal
    {
        public DiscardPileDal(MongoDbConfig configuration) : 
            base(configuration, configuration.DiscardPileCollectionName) { }
    }
}
