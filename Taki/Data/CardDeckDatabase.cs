using Taki.Dal;
using Taki.Models.Deck;
using Taki.Shared.Abstract;
using Taki.Shared.Interfaces;
using Taki.Shared.Models.Dto;

namespace Taki.Data
{
    internal class CardDeckDatabase
    {
        private readonly IDal<CardDto> _drawPileDatabase;
        private readonly IDal<CardDto> _discardPileDatabase;

        public CardDeckDatabase(DrawPileDal drawPileDal, DiscardPileDal discardPileDal)
        {
            _drawPileDatabase = drawPileDal;
            _discardPileDatabase = discardPileDal;
        }

        public void AddDiscardCard(Card card)
        {
            var discardPile = _discardPileDatabase.FindAll();
            _discardPileDatabase.DeleteAll();
            _discardPileDatabase.Create(card.ToCardDto());
            _discardPileDatabase.CreateMany(discardPile);
        }

        public void UpdateAllCards(CardDeck discardPile, CardDeck drawPile)
        {
            _drawPileDatabase.DeleteAll();
            _drawPileDatabase.CreateMany(drawPile.GetAllCards().Select(card => card.ToCardDto()).ToList());
            _discardPileDatabase.DeleteAll();
            _discardPileDatabase.CreateMany(discardPile.GetAllCards().Select(card => card.ToCardDto()).ToList());
        }

        public void DrawCard(Card card)
        {
            _drawPileDatabase.Delete(card.Id);
        }

        public List<CardDto> GetDiscardCards()
        {
            return _discardPileDatabase.FindAll();
        }

        public List<CardDto> GetDrawCards()
        {
            return _drawPileDatabase.FindAll();
        }

        public void DeleteAll()
        {
            _drawPileDatabase.DeleteAll();
            _discardPileDatabase.DeleteAll();
        }
    }
}
