using MongoDB.Bson;
using TakiApp.Shared.Models;

namespace TakiApp.Shared.Interfaces
{
    public interface IGameTurnService
    {
        Task<Player> PlayTurnByIdAsync(ObjectId playerId, bool shouldGetTerminalGetMessages = true);
        Task WaitGameEndAsync(ObjectId id, bool shouldReadMessages = true);
        Task WaitTurnByIdAsync(ObjectId playerId, bool shouldReadMessages = true);
    }
}