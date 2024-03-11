using TakiApp.Shared.Models;

namespace TakiApp.Shared.Interfaces
{
    public interface IGameSettingsRepository
    {
        Task CreateGameSettings(GameSettings gameSettings);
        Task DeleteGameAsync();
        Task<GameSettings> FinishGameAsync();
        Task<GameSettings?> GetGameSettingsAsync();
        Task<GameSettings> UpdateWinnersAsync(Player player);
        Task WaitGameStartAsync(int numberOfCurrentPlayers);
    }
}