using MongoDB.Bson;
using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IPlayersRepository
    {
        Task CreatePlayerAsync(Player player);
        Task<List<Player>> GetAllAsync();
        Task CreateManyAsync(List<Player> players);
        Task<Player> PlayerDrawCardAsync(Player player);
        Task NextPlayerAsync();
        Task<Player> GetCurrentPlayerAsync();
        Task DeleteAllAsync();
        Task UpdatePlayer(Player player);
        Task DrawCardsAsync(Player player, int cardsToDraw);
        Task UpdateOrder(List<Player> players);
        Task AddMessagesToPlayersAsync(Player playerSent, string message);
        Task SkipPlayers(int playersToSkip = 1);
        Task<Player> GetPlayerByIdAsync(ObjectId playerId);
    }
}