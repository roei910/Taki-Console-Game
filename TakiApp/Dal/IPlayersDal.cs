using MongoDB.Bson;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Dal
{
    public interface IPlayersDal : IDal<Player>
    {
        Task WaitTurn(ObjectId playerId);
    }
}