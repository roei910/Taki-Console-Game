using MongoDB.Bson;

namespace TakiApp.Interfaces
{
    public interface IGameTurnService
    {
        Task PlayTurnByIdAsync(ObjectId playerId);
        Task WaitTurnByIdAsync(ObjectId playerId);
    }
}