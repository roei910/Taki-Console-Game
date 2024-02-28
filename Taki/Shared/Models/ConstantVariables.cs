using Microsoft.Extensions.Configuration;
using Taki.Extensions;

namespace Taki.Shared.Models
{
    public class ConstantVariables
    {
        public int MinNumberOfPlayers { get; set; }
        public int MaxNumberOfPlayers { get; set; }
        public int MinNumberOfPlayerCards { get; set; }
        public int MaxNumberOfPlayerCards{ get; set; }
        public int NumberOfPyramidPlayerCards { get; set; }
        public int NumberOfTotalWinners{ get; set; }

        public ConstantVariables(IConfiguration configuration)
        {
            MinNumberOfPlayers = configuration.GetRequiredIntegerValue("ConstantVariables", nameof(MinNumberOfPlayers));
            MaxNumberOfPlayers = configuration.GetRequiredIntegerValue("ConstantVariables", nameof(MaxNumberOfPlayers));
            MinNumberOfPlayerCards = configuration.GetRequiredIntegerValue("ConstantVariables", nameof(MinNumberOfPlayerCards));
            MaxNumberOfPlayerCards = configuration.GetRequiredIntegerValue("ConstantVariables", nameof(MaxNumberOfPlayerCards));
            NumberOfPyramidPlayerCards = configuration.GetRequiredIntegerValue("ConstantVariables", nameof(NumberOfPyramidPlayerCards));
            NumberOfTotalWinners = configuration.GetRequiredIntegerValue("ConstantVariables", nameof(NumberOfTotalWinners));
        }
    }   
}
