using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.GameRules;

namespace Taki.Game.Factories
{
    internal class CardsHandlerFactory
    {
        private readonly CardDeck cardDeck;

        public CardsHandlerFactory()
        {
            List<Card> cards = GetCardsList();
            cardDeck = new(cards);
        }

        public CardsHandler GenerateCardsHandler()
        {
            CardDeck drawPile = new(cardDeck);
            CardDeck discardPile = new();
            return new CardsHandler(drawPile, discardPile);
        }

        public static int MaxNumberOfCards()
        {
            return GetCardsList().Count;
        }

        private static List<Card> GetCardsList()
        {
            List<Card> cards = [.. GenerateNumberCards(), .. GenerateUniqueCards()];
            return cards;
        }

        private static List<Card> GenerateNumberCards()
        {
            var cards = Enumerable.Range(0, 2)
                .SelectMany(i => Enumerable.Range(3, 7)
                .SelectMany(number => ColorCard.Colors
                .Select(color => (Card)new NumberCard(number, color)))).ToList();
            return cards;
        }

        private static List<Card> GenerateUniqueCards()
        {
            List<Card> cards = [];
            foreach (var _ in Enumerable.Range(0, 2))
            {
                cards.Add(new SuperTaki());
                cards.Add(new SwitchCardsWithDirection());
                Enumerable.Range(0, 2).ToList()
                    .ForEach(_ => cards.Add(new ChangeColor()));

                ColorCard.Colors.ForEach(color =>
                {
                    cards.Add(new ChangeDirection(color));
                    cards.Add(new Plus(color));
                    cards.Add(new Plus2(color));
                    cards.Add(new TakiCard(color));
                    cards.Add(new Stop(color));
                });
            }
            return cards;
        }
    }
}
