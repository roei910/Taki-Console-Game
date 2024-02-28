using Taki.Shared.Models;

namespace Taki.Dal
{
    internal class DrawPileDal : CardDal
    {
        public DrawPileDal(MongoDbConfig configuration) : 
            base(configuration, configuration.DrawPileCollectionName) { }
    }
}
