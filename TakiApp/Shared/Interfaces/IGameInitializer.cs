using TakiApp.Shared.Models;

namespace TakiApp.Shared.Interfaces
{
    public interface IGameInitializer
    {
        Player GetPlayer { get; }
        Task<bool> IsGameInitializedAsync();
        GameSettings GetGameSettings();
        Task InitializeGame();
    }
}