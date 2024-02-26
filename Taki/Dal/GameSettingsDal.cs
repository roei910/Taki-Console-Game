using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Models.GameLogic;

namespace Taki.Dal
{
    internal class GameSettingsDal : MongoDal<GameSettings>
    {
        public GameSettingsDal(IConfiguration configuration, string collectionName) : 
            base(configuration, collectionName) { }

        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteAll()
        {
            var filter = Builders<GameSettings>.Filter.Empty;
            _collection.DeleteMany(filter);

            return true;
        }

        public override void UpdateOne(GameSettings value)
        {
            throw new NotImplementedException();
        }
    }
}
