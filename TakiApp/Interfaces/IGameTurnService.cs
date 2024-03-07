using MongoDB.Bson;
using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IGameTurnService
    {
        Task<Player> PlayTurnByIdAsync(ObjectId playerId);
        Task WaitGameEndAsync(ObjectId id);
        Task WaitTurnByIdAsync(ObjectId playerId);
    }
}