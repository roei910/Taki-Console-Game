using MongoDB.Bson;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.GameLogic
{
    public class GameTurnService : IGameTurnService
    {
        private readonly IPlayersRepository _playerRepository;
        private readonly IAlgorithmService _algorithmService;
        private readonly IDiscardPileRepository _discardPileRepository;
        private readonly IUserCommunicator _userCommunicator;
        private readonly ICardPlayService _cardPlayService;

        public GameTurnService(IPlayersRepository playerRepository, IAlgorithmService algorithmService,
            IDiscardPileRepository discardPileRepository, IUserCommunicator userCommunicator, 
            ICardPlayService cardPlayService)
        {
            _playerRepository = playerRepository;
            _algorithmService = algorithmService;
            _discardPileRepository = discardPileRepository;
            _userCommunicator = userCommunicator;
            _cardPlayService = cardPlayService;
        }

        public async Task PlayTurnByIdAsync(ObjectId playerId)
        {
            var topDiscard = await _discardPileRepository.GetTopDiscardAsync();
            var currentPlayer = await _playerRepository.GetCurrentPlayerAsync();

            _userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");
            _userCommunicator.SendAlertMessage($"Current player: {currentPlayer.Name}");

            var canStack = _cardPlayService.CanStack(topDiscard);
            var cardsToDraw = _cardPlayService.CardsToDraw(topDiscard);

            Card? card = _algorithmService.PickCard(currentPlayer, canStack, $"or draw {cardsToDraw} card(s)");
            
            if (card is null)
            {
                await _cardPlayService.FinishNoPlayAsync(topDiscard);

                var cardsDrew = await _playerRepository.DrawCardsAsync(currentPlayer, cardsToDraw);
                if(cardsDrew.Count == 0)
                {
                    _userCommunicator.SendErrorMessage("Couldnt draw cards from deck");
                    await _playerRepository.NextPlayerAsync();

                    return;
                }
                await _playerRepository.SendMessagesFromPlayerAsync(currentPlayer, $"{currentPlayer.Name} drew {cardsDrew.Count} card(s)\n");
                await _playerRepository.NextPlayerAsync();

                return;
            }

            currentPlayer.Cards.Remove(card);
            await _playerRepository.UpdatePlayerAsync(currentPlayer);
            await _discardPileRepository.AddCardAsync(card);

            await _cardPlayService.PlayCardAsync(currentPlayer, card);
            await _cardPlayService.FinishPlayAsync(topDiscard);
        }

        public async Task WaitTurnByIdAsync(ObjectId playerId)
        {
            Player player = await _playerRepository.GetPlayerByIdAsync(playerId);

            foreach (var message in player.Messages)
                _userCommunicator.SendAlertMessage(message);

            player.Messages.Clear();

            await _playerRepository.UpdatePlayerAsync(player);

            if (player != null && player.IsPlaying)
                return;

            await Task.Delay(1000);
            await WaitTurnByIdAsync(playerId);
        }
    }
}
