using MongoDB.Bson;
using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.GameLogic
{
    public class GameTurnService : IGameTurnService
    {
        private readonly IPlayersRepository _playerRepository;
        private readonly IPlayerService _playersService;
        private readonly IDiscardPileRepository _discardPileRepository;
        //TODO: not used check if needed
        private readonly IDrawPileRepository _drawPileRepository;
        private readonly IUserCommunicator _userCommunicator;
        private readonly ICardPlayService _cardPlayService;

        public GameTurnService(IPlayersRepository playerRepository, IPlayerService playerService,
            IDiscardPileRepository discardPileRepository, IDrawPileRepository drawPileRepository,
            IUserCommunicator userCommunicator, List<ICardService> cardServices, ICardPlayService cardPlayService)
        {
            _playerRepository = playerRepository;
            _playersService = playerService;
            _discardPileRepository = discardPileRepository;
            _drawPileRepository = drawPileRepository;
            _userCommunicator = userCommunicator;
            _cardPlayService = cardPlayService;
        }

        public async Task PlayTurnByIdAsync(ObjectId playerId)
        {
            var topDiscard = await _discardPileRepository.GetTopDiscardAsync();
            var currentPlayer = await _playerRepository.GetCurrentPlayerAsync();

            _userCommunicator.SendAlertMessage($"Top discard: {topDiscard.Type}, {Color.FromName(topDiscard.CardColor)}");
            _userCommunicator.SendAlertMessage($"Current player: {currentPlayer.Name}");

            var canStack = _cardPlayService.CanStack(topDiscard);
            Card? card = _playersService.PickCard(currentPlayer, canStack);
            
            if (card is null)
            {
                await _playersService.DrawCard(currentPlayer);

                return;
            }

            currentPlayer.Cards.Remove(card);
            await _playerRepository.UpdatePlayer(currentPlayer);
            await _discardPileRepository.AddCardAsync(card);

            await _cardPlayService.PlayCardAsync(currentPlayer, card);
        }

        public async Task WaitTurnByIdAsync(ObjectId playerId)
        {
            //TODO: update the screen from here when changes to the topdiscard is happening, maybe with is completed on the task
            await _playerRepository.WaitTurnAsync(playerId);
        }
    }
}
