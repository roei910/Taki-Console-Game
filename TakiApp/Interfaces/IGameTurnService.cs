using MongoDB.Bson;

namespace TakiApp.Interfaces
{
    public interface IGameTurnService
    {
        void PlayTurnById(ObjectId playerId);
        Task WaitTurnById(ObjectId playerId);
    }
}