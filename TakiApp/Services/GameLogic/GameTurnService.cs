using MongoDB.Bson;
using TakiApp.Dal;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.GameLogic
{
    public class GameTurnService : IGameTurnService
    {
        private readonly IPlayersDal _playersDal;
        private readonly IPlayerService _playersService;
        private readonly IDiscardPileRepository _discardPileRepository;
        private readonly IDrawPileRepository _drawPileRepository;

        public GameTurnService(IPlayersDal playersDal, IPlayerService playerService,
            IDiscardPileRepository discardPileRepository, IDrawPileRepository drawPileRepository)
        {
            _playersDal = playersDal;
            _playersService = playerService;
            _discardPileRepository = discardPileRepository;
            _drawPileRepository = drawPileRepository;
        }

        public async void PlayTurnById(ObjectId playerId)
        {
            //TODO: choose card from hand
            var topDiscard = await _discardPileRepository.GetTopDiscard();

            var players = await _playersDal.FindAsync();
            var currentPlayer = players.First();

            //TODO: play the card
            Card card = _playersService.PickCard(currentPlayer, topDiscard);
        }

        public async Task WaitTurnById(ObjectId playerId)
        {
            await _playersDal.WaitTurn(playerId);
        }
    }
}
