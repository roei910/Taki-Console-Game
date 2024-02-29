using Taki.Dal;
using Taki.Models.Deck;
using Taki.Shared.Abstract;
using Taki.Shared.Interfaces;
using Taki.Shared.Models.Dto;

namespace Taki.Data
{
    public class CardDeckRepository : ICardDeckRepository
    {
        private readonly IDal<CardDto> _drawPileDal;
        private readonly IDal<CardDto> _discardPileDal;

        public CardDeckRepository(DrawPileDal drawPileDal, DiscardPileDal discardPileDal)
        {
            _drawPileDal = drawPileDal;
            _discardPileDal = discardPileDal;
        }

        public void AddDiscardCard(Card card)
        {
            var discardPile = _discardPileDal.FindAll();
            _discardPileDal.DeleteAll();
            _discardPileDal.Create(card.ToCardDto());
            _discardPileDal.CreateMany(discardPile);
        }

        public void UpdateAllCards(CardDeck discardPile, CardDeck drawPile)
        {
            _drawPileDal.DeleteAll();
            _drawPileDal.CreateMany(drawPile.GetAllCards().Select(card => card.ToCardDto()).ToList());
            _discardPileDal.DeleteAll();
            _discardPileDal.CreateMany(discardPile.GetAllCards().Select(card => card.ToCardDto()).ToList());
        }

        public void DrawCard(Card card)
        {
            _drawPileDal.Delete(card.Id);
        }

        public List<CardDto> GetDiscardCards()
        {
            return _discardPileDal.FindAll();
        }

        public List<CardDto> GetDrawCards()
        {
            return _drawPileDal.FindAll();
        }

        public void DeleteAll()
        {
            _drawPileDal.DeleteAll();
            _discardPileDal.DeleteAll();
        }

        public void UpdateTopDiscard(CardDto card)
        {
            _discardPileDal.UpdateOne(card);
        }
    }
}
