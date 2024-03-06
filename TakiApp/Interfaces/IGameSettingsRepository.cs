﻿using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IGameSettingsRepository
    {
        Task CreateGameSettings(GameSettings gameSettings);
        Task DeleteGameAsync();
        Task<GameSettings?> GetGameSettingsAsync();
        Task WaitGameStartAsync(int numberOfCurrentPlayers);
    }
}