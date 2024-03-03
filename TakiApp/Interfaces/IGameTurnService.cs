using MongoDB.Bson;

namespace TakiApp.Interfaces
{
    internal interface IGameTurnService
    {
        void PlayTurnById(ObjectId playerId);
        Task WaitTurnById(ObjectId playerId);
    }
}