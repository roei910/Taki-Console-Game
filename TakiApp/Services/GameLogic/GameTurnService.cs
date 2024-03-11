﻿using MongoDB.Bson;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Services.GameLogic
{
    public class GameTurnService : IGameTurnService
    {
        private readonly IPlayersRepository _playerRepository;
        private readonly IAlgorithmService _algorithmService;
        private readonly IDiscardPileRepository _discardPileRepository;
        private readonly IUserCommunicator _userCommunicator;
        private readonly ICardPlayService _cardPlayService;
        private readonly IGameSettingsRepository _gameSettingsRepository;

        public GameTurnService(IPlayersRepository playerRepository, IAlgorithmService algorithmService,
            IDiscardPileRepository discardPileRepository, IUserCommunicator userCommunicator, 
            ICardPlayService cardPlayService, IGameSettingsRepository gameSettingsRepository)
        {
            _playerRepository = playerRepository;
            _algorithmService = algorithmService;
            _discardPileRepository = discardPileRepository;
            _userCommunicator = userCommunicator;
            _cardPlayService = cardPlayService;
            _gameSettingsRepository = gameSettingsRepository;
        }

        public async Task<Player> PlayTurnByIdAsync(ObjectId playerId)
        {
            var topDiscard = await _discardPileRepository.GetTopDiscardAsync();
            var currentPlayer = await _playerRepository.GetCurrentPlayerAsync();

            _userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");

            var canStack = _cardPlayService.CanStack(topDiscard);
            var cardsToDraw = _cardPlayService.CardsToDraw(topDiscard);

            Card? card = _algorithmService.PickCard(currentPlayer, canStack, $"or -1 to draw {cardsToDraw} card(s)");
            
            if (card is null)
            {
                await _cardPlayService.FinishNoPlayAsync(topDiscard);

                var cardsDrew = await _playerRepository.DrawCardsAsync(currentPlayer, cardsToDraw, true);

                if (cardsDrew.Count == 0)
                    _userCommunicator.SendErrorMessage("Couldnt draw cards from deck");

                await _playerRepository.NextPlayerAsync();

                return currentPlayer;
            }

            currentPlayer.Cards.Remove(card);
            await _playerRepository.UpdatePlayerAsync(currentPlayer);
            await _discardPileRepository.AddCardOrderedAsync(card);

            await _cardPlayService.PlayCardAsync(currentPlayer, card);
            await _cardPlayService.FinishPlayAsync(topDiscard);

            return currentPlayer;
        }

        public async Task WaitGameEndAsync(ObjectId id)
        {
            var player = await _playerRepository.GetPlayerByIdAsync(id);

            var gameSettings = await _gameSettingsRepository.GetGameSettingsAsync();

            await SendMessagesToPlayer(player);

            if (gameSettings!.HasGameEnded)
                return;

            await Task.Delay(1000);
            await WaitGameEndAsync(id);
        }

        public async Task WaitTurnByIdAsync(ObjectId playerId)
        {
            Player player = await _playerRepository.GetPlayerByIdAsync(playerId);

            await SendMessagesToPlayer(player);

            if (player != null && player.IsPlaying)
                return;

            await Task.Delay(1000);
            await WaitTurnByIdAsync(playerId);
        }

        private async Task SendMessagesToPlayer(Player player)
        {
            foreach (var message in player.Messages)
                _userCommunicator.SendAlertMessage(message);

            player.Messages.Clear();

            await _playerRepository.UpdatePlayerAsync(player);
        }
    }
}
