using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Dal
{
    public class DrawPileDal : CardDal, IDrawPileDal
    {
        public DrawPileDal(MongoDbConfig configuration) : 
            base(configuration, configuration.DrawPileCollectionName) { }
    }
}
