using TakiApp.Models;

namespace TakiApp.Dal
{
    internal class DrawPileDal : CardDal
    {
        public DrawPileDal(MongoDbConfig configuration) : 
            base(configuration, configuration.DrawPileCollectionName) { }
    }
}
