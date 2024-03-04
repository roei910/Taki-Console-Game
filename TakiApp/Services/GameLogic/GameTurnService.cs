using MongoDB.Bson;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.GameLogic
{
    public class GameTurnService : IGameTurnService
    {
        private readonly IPlayersRepository _playerRepository;
        private readonly IPlayerService _playersService;
        private readonly IDiscardPileRepository _discardPileRepository;
        private readonly IDrawPileRepository _drawPileRepository;
        private readonly IUserCommunicator _userCommunicator;
        private readonly List<ICardService> _cardServices;

        public GameTurnService(IPlayersRepository playerRepository, IPlayerService playerService,
            IDiscardPileRepository discardPileRepository, IDrawPileRepository drawPileRepository,
            IUserCommunicator userCommunicator, List<ICardService> cardServices)
        {
            _playerRepository = playerRepository;
            _playersService = playerService;
            _discardPileRepository = discardPileRepository;
            _drawPileRepository = drawPileRepository;
            _userCommunicator = userCommunicator;
            _cardServices = cardServices;
        }

        public async Task PlayTurnByIdAsync(ObjectId playerId)
        {
            var topDiscard = await _discardPileRepository.GetTopDiscard();
            var currentPlayer = await _playerRepository.GetCurrentPlayerAsync();

            _userCommunicator.SendAlertMessage($"Top discard: {topDiscard.Type}, {topDiscard.CardColor}");
            _userCommunicator.SendAlertMessage($"Current player: {currentPlayer.Name}");

            Func<Card, bool> canStack = (Card card) => MatchCardService(card).CanStackOtherOnThis(topDiscard, card);
            Card? card = _playersService.PickCard(currentPlayer, canStack);
            
            if (card is null)
            {
                Card? drawCard = await _drawPileRepository.DrawCardAsync();

                if (drawCard is null)
                    return;

                _playersService.AddCard(currentPlayer, drawCard);

                return;
            }

            await MatchCardService(card).PlayAsync(currentPlayer, card, topDiscard);
        }

        public async Task WaitTurnByIdAsync(ObjectId playerId)
        {
            //TODO: update the screen from here when changes to the topdiscard is happening, maybe with is completed on the task
            await _playerRepository.WaitTurnAsync(playerId);
        }

        private ICardService MatchCardService(Card card)
        {
            var found = _cardServices.Where(service => service.ToString() == card.Type.Split(':')[0]).FirstOrDefault();

            return found ?? throw new Exception("couldnt find the card service in the list");
        }

    }
}
