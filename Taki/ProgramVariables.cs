using Microsoft.Extensions.Configuration;

namespace Taki
{
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

            MIN_NUMBER_OF_PLAYERS = int.Parse(variables["MIN_NUMBER_OF_PLAYERS"]?? 
                throw new NullReferenceException("Please define minimum number of players"));

            MAX_NUMBER_OF_PLAYERS = int.Parse(variables["MAX_NUMBER_OF_PLAYERS"] ?? 
                throw new NullReferenceException("Please define maximum number of players"));

            MIN_NUMBER_OF_PLAYER_CARDS = int.Parse(variables["MIN_NUMBER_OF_PLAYER_CARDS"] ?? 
                throw new NullReferenceException("Please define a minimum number of player cards"));

            MAX_NUMBER_OF_PLAYER_CARDS = int.Parse(variables["MAX_NUMBER_OF_PLAYER_CARDS"] ??
                throw new NullReferenceException("Please define a maximum number of player cards"));

            NUMBER_OF_PYRAMID_PLAYER_CARDS = int.Parse(variables["NUMBER_OF_PYRAMID_PLAYER_CARDS"] ?? 
                throw new NullReferenceException("Please define a number of cards for pyramid player"));

            NUMBER_OF_TOTAL_WINNERS = int.Parse(variables["NUMBER_OF_TOTAL_WINNERS"] ?? 
                throw new NullReferenceException("Please define a number of total winners for the game"));
        }
    }
}
