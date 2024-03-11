using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Dal
{
    public class DrawPileDal : CardDal, IDrawPileDal
    {
        public DrawPileDal(MongoDbConfig configuration) : 
            base(configuration, configuration.DrawPileCollectionName) { }
    }
}
