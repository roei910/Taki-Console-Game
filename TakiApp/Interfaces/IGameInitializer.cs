using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IGameInitializer
    {
        Player GetPlayer { get; }
        GameSettings GetGameSettings();
        Task InitializeGame();
    }
}