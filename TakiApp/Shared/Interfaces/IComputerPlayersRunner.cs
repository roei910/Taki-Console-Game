using TakiApp.Shared.Models;

namespace TakiApp.Shared.Interfaces
{
    public interface IComputerPlayersRunner
    {
        Task<Player> GeneragePlayer(int index);
        Task<List<Player>> GenerateComputerPlayers();
        Task RunPlayers();
    }
}