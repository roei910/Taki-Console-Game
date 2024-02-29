using Taki.Shared.Models;

namespace Taki
{
    //TODO: from tomer: is it good => itzhaki
    public class AppSettings
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
