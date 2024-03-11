using TakiApp.Shared.Models;

namespace TakiApp.Shared.Interfaces
{
    public interface IGameInitializer
    {
        Player GetPlayer { get; }
        GameSettings GetGameSettings();
        Task InitializeGame();
    }
}