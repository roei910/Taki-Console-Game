﻿using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IGameSettingsRepository
    {
        Task CreateGameSettings(GameSettings gameSettings);
        Task<GameSettings?> GetGameSettingsAsync();
        Task WaitGameStart(int numberOfCurrentPlayers);
    }
}