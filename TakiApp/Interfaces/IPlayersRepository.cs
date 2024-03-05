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
        Task WaitTurnAsync(ObjectId playerId);
        Task NextPlayerAsync(Player player);
        Task<Player> GetCurrentPlayerAsync();
        Task DeleteAllAsync();
        Task UpdatePlayer(Player player);
        Task DrawCards(Player player, int cardsToDraw);
    }
}