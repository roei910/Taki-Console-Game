using MongoDB.Bson;
using TakiApp.Shared.Models;

namespace TakiApp.Shared.Interfaces
{
    public interface IGameTurnService
    {
        Task<Player> PlayTurnByIdAsync(ObjectId playerId);
        Task WaitGameEndAsync(ObjectId id);
        Task WaitTurnByIdAsync(ObjectId playerId);
    }
}