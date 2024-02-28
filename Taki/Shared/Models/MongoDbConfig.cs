using Microsoft.Extensions.Configuration;
using Taki.Extensions;

namespace Taki.Shared.Models
{
    internal class MongoDbConfig
    {
        public string MongoUrl { get; set; }
        public string MongoDatabaseName { get; set; }
        public string DrawPileCollectionName { get; set; }
        public string DiscardPileCollectionName { get; set; }
        public string PlayersCollectionName { get; set; }
        public string GameSettingsCollectionName { get; set; }

        public MongoDbConfig(IConfiguration configuration) 
        {
            MongoUrl = configuration.GetRequiredValue("Mongo", nameof(MongoUrl));
            MongoDatabaseName = configuration.GetRequiredValue("Mongo", nameof(MongoDatabaseName));
            DrawPileCollectionName = configuration.GetRequiredValue("Mongo", nameof(DrawPileCollectionName));
            DiscardPileCollectionName = configuration.GetRequiredValue("Mongo", nameof(DiscardPileCollectionName));
            PlayersCollectionName = configuration.GetRequiredValue("Mongo", nameof(PlayersCollectionName));
            GameSettingsCollectionName = configuration.GetRequiredValue("Mongo", nameof(GameSettingsCollectionName));
        }
    }
}
