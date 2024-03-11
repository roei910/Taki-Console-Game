using MongoDB.Bson;
using TakiApp.Shared.Models;

namespace TakiApp.Shared.Interfaces
{
    public interface IPlayersRepository
    {
        Task CreatePlayerAsync(Player player);
        Task<List<Player>> GetAllAsync();
        Task CreateManyAsync(List<Player> players);
        Task<Player> NextPlayerAsync();
        Task<Player> GetCurrentPlayerAsync();
        Task DeleteAllAsync();
        Task UpdatePlayerAsync(Player player);
        Task<List<Card>> DrawCardsAsync(Player player, int cardsToDraw, bool messageUsers = false);
        Task UpdateOrder(List<Player> players);
        Task SkipPlayers(int playersToSkip = 1);
        Task<Player> GetPlayerByIdAsync(ObjectId playerId);
        Task SendMessagesToPlayersAsync(string from, string message, params Player[] excludedPlayers);
        Task UpdateManyAsync(List<Player> players);
    }
}