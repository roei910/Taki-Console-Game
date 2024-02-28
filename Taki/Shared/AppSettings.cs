using Taki.Shared.Models;

namespace Taki
{
    //TODO: is it good => itzhaki
    internal class AppSettings
    {
        public ConstantVariables ConstantVariables { get; set; }
        public MongoDbConfig MongoDbConfig { get; set; }

        public AppSettings(ConstantVariables constantVariables, MongoDbConfig mongoDbConfig)
        {
            ConstantVariables = constantVariables;
            MongoDbConfig = mongoDbConfig;
        }
    }
}
