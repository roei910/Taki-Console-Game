using Taki.Shared.Models;

namespace Taki.Dal
{
    public class DrawPileDal : CardDal
    {
        public DrawPileDal(MongoDbConfig configuration) : 
            base(configuration, configuration.DrawPileCollectionName) { }
    }
}
