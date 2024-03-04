using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IPlayersRepository
    {
        Task CreateNewAsync(Player player);
        Task<List<Player>> GetAllAsync();
        Task CreateManyAsync(List<Player> players);
        Task<Player> PlayerDrawCardAsync(Player player);
    }
}