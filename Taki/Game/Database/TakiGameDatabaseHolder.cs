using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.Players;

namespace Taki.Game.Database
{
    internal class TakiGameDatabaseHolder
    {
        private readonly IDatabase<PlayerDTO> _playersDatabase;
        private readonly IDatabase<CardDTO> _drawPileDatabase;
        private readonly IDatabase<CardDTO> _discardPileDatabase;

        public TakiGameDatabaseHolder(IDatabase<PlayerDTO> playersDatabase, IServiceProvider serviceProvider)
        {
            _drawPileDatabase = serviceProvider.GetRequiredKeyedService<IDatabase<CardDTO>>("drawPile");
            _discardPileDatabase = serviceProvider.GetRequiredKeyedService<IDatabase<CardDTO>>("discardPile");
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

            var discardDTOs = discardCards.GetAllCards().Select(CardDTO.CardToDTO).ToList();
            var drawDTOs = drawCards.GetAllCards().Select(CardDTO.CardToDTO).ToList();

            _discardPileDatabase.CreateMany(discardDTOs);
            _drawPileDatabase.CreateMany(drawDTOs);
        }

        public bool IsEmpty()
        {
            return _playersDatabase.IsEmpty() && _discardPileDatabase.IsEmpty() && _drawPileDatabase.IsEmpty(); 
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

        internal List<CardDTO> GetDrawPile()
        {
            return _drawPileDatabase.FindAll();
        }

        internal List<CardDTO> GetDiscardPile()
        {
            return _discardPileDatabase.FindAll();
        }

        public void UpdateDatabase(IPlayersHolder playersHolder, ICardDecksHolder cardDecksHolder)
        {
            var players = playersHolder.Players.Select(PlayerDTO.PlayerToDTO).ToList();
            var drawPile = cardDecksHolder.GetDrawPile().GetAllCards().Select(CardDTO.CardToDTO).ToList();
            var discardPile = cardDecksHolder.GetDiscardPile().GetAllCards().Select(CardDTO.CardToDTO).ToList();

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
