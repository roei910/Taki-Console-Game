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
    }
}
