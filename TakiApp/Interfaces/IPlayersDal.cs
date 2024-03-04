using MongoDB.Bson;
using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IPlayersDal : IDal<Player>
    {
        Task WaitTurn(ObjectId playerId);
    }
}