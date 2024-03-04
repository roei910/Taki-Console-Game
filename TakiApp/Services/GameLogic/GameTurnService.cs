using MongoDB.Bson;
using TakiApp.Dal;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.GameLogic
{
    public class GameTurnService : IGameTurnService
    {
        private readonly IPlayersDal _playersDal;
        private readonly IDal<Card> _drawPile;
        private readonly IDal<Card> _discardPile;
        private readonly IPlayerService _playersService;

        public GameTurnService(IPlayersDal playersDal, IPlayerService playerService,
            List<IDal<Card>> cardDals)
        {
            _playersDal = playersDal;
            _playersService = playerService;
            _drawPile = cardDals.Where(dal => dal.GetType() == typeof(DrawPileDal)).First();
            _discardPile = cardDals.Where(dal => dal.GetType() == typeof(DiscardPileDal)).First();
        }

        public async void PlayTurnById(ObjectId playerId)
        {
            //TODO: choose card from hand
            var cards = await _discardPile.FindAsync();
            var topDiscard = cards.First();

            var players = await _playersDal.FindAsync();
            var first = players.First();

            //TODO: play the card
        }

        public async Task WaitTurnById(ObjectId playerId)
        {
            await _playersDal.WaitTurn(playerId);
        }
    }
}
