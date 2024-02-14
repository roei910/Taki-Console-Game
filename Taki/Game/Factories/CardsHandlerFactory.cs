using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.GameRules;

namespace Taki.Game.Factories
{
    //TODO: Split into deck factory to handler factory (if the handler factory is necessary at all)
    internal class CardsHandlerFactory
    {
        public CardsHandler GenerateCardsHandler(IServiceProvider serviceProvider)
        {
            CardDeck drawPile = new(GetCardsList(), serviceProvider);
            CardDeck discardPile = new(serviceProvider);
            return new CardsHandler(drawPile, discardPile);
        }

        public int MaxNumberOfCards()
        {
            return GetCardsList().Count;
        }

        private List<Card> GetCardsList()
        {
            List<Card> cards = [.. GenerateNumberCards(), .. GenerateSpecialCards()];

            return cards;
        }

        private List<Card> GenerateNumberCards()
        {
            var cards = Enumerable.Range(0, 2)
                .SelectMany(_ => Enumerable.Range(3, 7)
                .SelectMany(number => ColorCard.Colors
                .Select(color => (Card)new NumberCard(number, color)))).ToList();

            return cards;
        }

        private List<Card> GenerateSpecialCards()
        {
            var cards = Enumerable.Range(0, 2)
                .SelectMany(i => GenerateSpecialNoColor())
                .Union(GenerateSpecialWithColor()).ToList();

            return cards;
        }

        private IEnumerable<Card> GenerateSpecialWithColor()
        {
            return ColorCard.Colors.SelectMany(color =>
            {
                return new List<Card>()
                {
                    new ChangeDirection(color),
                    new ChangeDirection(color),
                    new Plus(color),
                    new Plus2(color),
                    new TakiCard(color),
                    new Stop(color)
                };
            }).ToList();
        }

        private IEnumerable<Card> GenerateSpecialNoColor()
        {
            var cards = new List<Card>() {
                new SuperTaki(),
                new SwitchCardsWithDirection()
            };

            return Enumerable.Range(0, 2)
                .Select(j => new ChangeColor())
                .Union(cards).ToList(); ;
        }
    }
}
