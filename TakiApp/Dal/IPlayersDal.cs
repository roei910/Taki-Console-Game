using MongoDB.Bson;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Dal
{
    internal interface IPlayersDal : IDal<Player>
    {
        Task WaitTurn(ObjectId playerId);
    }
}