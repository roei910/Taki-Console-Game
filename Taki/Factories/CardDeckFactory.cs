using Taki.Models.Cards;
using Taki.Models.Cards.NumberCards;
using Taki.Models.Deck;
using Taki.Shared.Interfaces;

namespace Taki.Factories
{
    internal class CardDeckFactory
    {
        private readonly IUserCommunicator _userCommunicator;
        private readonly Random _random;

        public CardDeckFactory(IUserCommunicator userCommunicator, Random random)
        {
            _userCommunicator = userCommunicator;
            _random = random;
        }

        public CardDeck GenerateCardDeck()
        {
            List<Card> cards = [.. GenerateNumberCards(), .. GenerateSpecialCards()];

            return new(cards, _random);
        }

        private List<Card> GenerateNumberCards()
        {
            var cards = Enumerable.Range(0, 2)
                .SelectMany(number => ColorCard.Colors
                .SelectMany(color =>
                    new List<Card>()
                    {
                        new ThreeCard(color, _userCommunicator),
                        new FourCard(color, _userCommunicator),
                        new FiveCard(color, _userCommunicator),
                        new SixCard(color, _userCommunicator),
                        new SevenCard(color, _userCommunicator),
                        new EightCard(color, _userCommunicator),
                        new NineCard(color, _userCommunicator)
                    })).ToList();

            return cards;
        }

        private List<Card> GenerateSpecialCards()
        {
            var cards = Enumerable.Range(0, 2)
                .SelectMany(i => GenerateSpecialNoColor())
                .Union(GenerateSpecialWithColor()).ToList();

            return cards;
        }

        private List<Card> GenerateSpecialWithColor()
        {
            return ColorCard.Colors.SelectMany(color =>
            {
                return new List<Card>()
                {
                    new ChangeDirection(color, _userCommunicator),
                    new ChangeDirection(color, _userCommunicator),
                    new Plus(color, _userCommunicator),
                    new Plus2(color, _userCommunicator),
                    new TakiCard(color, _userCommunicator),
                    new Stop(color, _userCommunicator)
                };
            }).ToList();
        }

        private List<Card> GenerateSpecialNoColor()
        {
            var cards = new List<Card>() {
                new SuperTaki(_userCommunicator),
                new SwitchCardsWithDirection(_userCommunicator),
                new SwitchCardsWithUser(_userCommunicator)
            };

            return Enumerable.Range(0, 2)
                .Select(j => new ChangeColor(_userCommunicator))
                .Union(cards).ToList(); ;
        }
    }
}
