using Microsoft.Extensions.Configuration;

namespace Taki
{
    //TODO: Move to configuration
    internal class ProgramVariables
    {
        public readonly int MIN_NUMBER_OF_PLAYERS;
        public readonly int MAX_NUMBER_OF_PLAYERS;
        public readonly int MIN_NUMBER_OF_PLAYER_CARDS;
        public readonly int MAX_NUMBER_OF_PLAYER_CARDS;
        public readonly int NUMBER_OF_PYRAMID_PLAYER_CARDS;
        public readonly int NUMBER_OF_TOTAL_WINNERS;

        public ProgramVariables(IConfiguration configuration)
        {
            var variables = configuration.GetSection("LocalVariables");

            MIN_NUMBER_OF_PLAYERS = int.Parse(variables["MIN_NUMBER_OF_PLAYERS"]?? "2");
            MAX_NUMBER_OF_PLAYERS = int.Parse(variables["MAX_NUMBER_OF_PLAYERS"] ?? "8");
            MIN_NUMBER_OF_PLAYER_CARDS = int.Parse(variables["MIN_NUMBER_OF_PLAYER_CARDS"] ?? "7");
            MAX_NUMBER_OF_PLAYER_CARDS = int.Parse(variables["MAX_NUMBER_OF_PLAYER_CARDS"] ?? "20");
            NUMBER_OF_PYRAMID_PLAYER_CARDS = int.Parse(variables["NUMBER_OF_PYRAMID_PLAYER_CARDS"] ?? "10");
            NUMBER_OF_TOTAL_WINNERS = int.Parse(variables["NUMBER_OF_TOTAL_WINNERS"] ?? "2");
        }
    }
}
