using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IGameInitializer
    {
        GameSettings GetGameSettings();
        Task InitializeGame();
    }
}