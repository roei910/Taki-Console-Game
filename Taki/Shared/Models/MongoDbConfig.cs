using Microsoft.Extensions.Configuration;

namespace Taki.Shared.Models
{
    internal class MongoDbConfig
    {
        public string MongoUrl { get; set; } = null!;
        public string MongoDatabaseName { get; set; } = null!;
        public string DrawPileCollectionName { get; set; } = null!;
        public string DiscardPileCollectionName { get; set; } = null!;
        public string PlayersCollectionName { get; set; } = null!;
        public string GameSettingsCollectionName { get; set; } = null!;

        public MongoDbConfig(IConfiguration configuration) 
        {
            var mongoConfig = configuration.GetRequiredSection("Mongo");
            MongoUrl = mongoConfig.GetRequiredSection(nameof(MongoUrl)).Value ?? null!;
            MongoDatabaseName = mongoConfig.GetRequiredSection(nameof(MongoDatabaseName)).Value ?? null!;
            DrawPileCollectionName = mongoConfig.GetRequiredSection(nameof(DrawPileCollectionName)).Value ?? null!;
            DiscardPileCollectionName = mongoConfig.GetRequiredSection(nameof(DiscardPileCollectionName)).Value ?? null!;
            PlayersCollectionName = mongoConfig.GetRequiredSection(nameof(PlayersCollectionName)).Value ?? null!;
            GameSettingsCollectionName = mongoConfig.GetRequiredSection(nameof(GameSettingsCollectionName)).Value ?? null!;
        }
    }
}
