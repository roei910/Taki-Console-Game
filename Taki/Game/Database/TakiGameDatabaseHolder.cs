using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Dto;
using Taki.Game.Interfaces;
using Taki.Game.Models.Players;

namespace Taki.Game.Database
{
    internal class TakiGameDatabaseHolder
    {
        private readonly IDatabase<PlayerDTO> _playersDatabase;
        private readonly IDatabase<CardDto> _drawPileDatabase;
        private readonly IDatabase<CardDto> _discardPileDatabase;

        public TakiGameDatabaseHolder(IDatabase<PlayerDTO> playersDatabase, IServiceProvider serviceProvider)
        {
            _drawPileDatabase = serviceProvider.GetRequiredKeyedService<IDatabase<CardDto>>("drawPile");
            _discardPileDatabase = serviceProvider.GetRequiredKeyedService<IDatabase<CardDto>>("discardPile");
            _playersDatabase = playersDatabase;
        }

        public void CreateAllPlayers(List<Player> players)
        {
            var playerDTOs = players.Select(PlayerDTO.PlayerToDTO).ToList();
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

        public List<PlayerDTO> GetAllPlayers()
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
            var players = playersHolder.Players.Select(PlayerDTO.PlayerToDTO).ToList();
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
