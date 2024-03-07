using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Factories
{
    //TODO: maybe delete and move the function to the cards service
    public class CardsFactory : ICardsFactory
    {
        private readonly List<ICardService> _cards;

        public CardsFactory(List<ICardService> cards)
        {
            _cards = cards;
        }

        public List<Card> GenerateDeck()
        {
            var cards = GenerateSingleDeck();

            return cards.Concat(GenerateSingleDeck()).ToList();
        }

        private List<Card> GenerateSingleDeck()
        {
            return _cards.SelectMany(card => card.GenerateCardsForDeck()).ToList();
        }
    }
}
