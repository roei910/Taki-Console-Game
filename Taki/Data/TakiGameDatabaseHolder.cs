using Microsoft.Extensions.DependencyInjection;
using Taki.Dto;
using Taki.Interfaces;
using Taki.Models.Players;

namespace Taki.Database
{
    internal class TakiGameDatabaseHolder
    {
        private readonly IDal<PlayerDto> _playersDatabase;
        private readonly IDal<CardDto> _drawPileDatabase;
        private readonly IDal<CardDto> _discardPileDatabase;

        public TakiGameDatabaseHolder(IDal<PlayerDto> playersDatabase, IServiceProvider serviceProvider)
        {
            _drawPileDatabase = serviceProvider.GetRequiredKeyedService<IDal<CardDto>>("drawPile");
            _discardPileDatabase = serviceProvider.GetRequiredKeyedService<IDal<CardDto>>("discardPile");
            _playersDatabase = playersDatabase;
        }

        public void CreateAllPlayers(List<Player> players)
        {
            var playerDTOs = players.Select(p => p.ToPlayerDto()).ToList();
            _playersDatabase.CreateMany(playerDTOs);
        }

        public void CreateCardDecks(ICardDecksHolder cardDecksHolder)
        {
            var discardCards = cardDecksHolder.GetDiscardPile();
            var drawCards = cardDecksHolder.GetDrawPile();

            var discardDTOs = discardCards.GetAllCards().Select(card => card.ToCardDto()).ToList();
            var drawDTOs = drawCards.GetAllCards().Select(card => card.ToCardDto()).ToList();

            _discardPileDatabase.CreateMany(discardDTOs);
            _drawPileDatabase.CreateMany(drawDTOs);
        }

        public bool IsEmpty()
        {
            return _playersDatabase.IsEmpty() &&
                _discardPileDatabase.IsEmpty() &&
                _drawPileDatabase.IsEmpty();
        }

        public void DeleteAll()
        {
            _playersDatabase.DeleteAll();
            _discardPileDatabase.DeleteAll();
            _drawPileDatabase.DeleteAll();
        }

        public List<PlayerDto> GetAllPlayers()
        {
            return _playersDatabase.FindAll();
        }

        internal List<CardDto> GetDrawPile()
        {
            return _drawPileDatabase.FindAll();
        }

        internal List<CardDto> GetDiscardPile()
        {
            return _discardPileDatabase.FindAll();
        }

        public void UpdateDatabase(IPlayersHolder playersHolder, ICardDecksHolder cardDecksHolder)
        {
            var players = playersHolder.Players.Select(p => p.ToPlayerDto()).ToList();
            var drawPile = cardDecksHolder.GetDrawPile().GetAllCards().Select(card => card.ToCardDto()).ToList();
            var discardPile = cardDecksHolder.GetDiscardPile().GetAllCards().Select(card => card.ToCardDto()).ToList();

            _playersDatabase.UpdateAll(players);
            _discardPileDatabase.UpdateAll(discardPile);
            _drawPileDatabase.UpdateAll(drawPile);
        }

        internal void DeleteAllPlayers()
        {
            _playersDatabase.DeleteAll();
        }
    }
}
