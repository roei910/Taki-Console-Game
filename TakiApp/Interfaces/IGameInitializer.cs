using TakiApp.Models;

namespace TakiApp.Interfaces
{
    internal interface IGameInitializer
    {
        GameSettings GetGameSettings();
        void InitializeGame();
    }
}