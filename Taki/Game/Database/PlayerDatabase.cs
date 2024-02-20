using Microsoft.Extensions.Configuration;
using Taki.Game.Players;

namespace Taki.Game.Database
{
    internal class PlayerDatabase : AbstractDatabase<Player>
    {
        public PlayerDatabase(IConfiguration configuration) : 
            base(configuration, "PlayersCollection") { }
    }
}
