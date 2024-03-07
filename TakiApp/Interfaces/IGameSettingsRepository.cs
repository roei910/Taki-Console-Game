using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IGameSettingsRepository
    {
        Task CreateGameSettings(GameSettings gameSettings);
        Task DeleteGameAsync();
        Task FinishGameAsync();
        Task<GameSettings?> GetGameSettingsAsync();
        Task UpdateWinnersAsync(string name);
        Task WaitGameStartAsync(int numberOfCurrentPlayers);
    }
}