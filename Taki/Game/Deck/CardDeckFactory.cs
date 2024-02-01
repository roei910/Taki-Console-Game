using Taki.Game.Cards;
using Taki.Game.General;

namespace Taki.Game.Deck
{
    internal abstract class CardDeckFactory
    {
        private static readonly List<UniqueCardEnum> uniqueCardsWithColor =
            [ UniqueCardEnum.Stop, UniqueCardEnum.Plus, UniqueCardEnum.Taki,
                    UniqueCardEnum.ChangeDirection, UniqueCardEnum.Plus2];

        public static CardDeck GenerateCardDeck()
        {
            List<Card> cards = GetCardsList();
            LinkedList<Card> linkedCards = new(cards);
            CardDeck cardDeck = new(linkedCards);
            return cardDeck;
        }

        private static List<Card> GetCardsList()
        {
            List<Card> cards = [.. GenerateNumberCards(), .. GenerateUniqueCards()];
            return cards;
        }

        private static List<Card> GenerateNumberCards()
        {
            List<Card> cards = [];
            foreach (var _ in Enumerable.Range(0, 2))
                foreach (var number in Enumerable.Range(3, 7))
                    Enums.CardColors.Values.ToList()
                        .ForEach(color => cards.Add(new NumberCard(number.ToString(), color)));
            return cards;
        }

        private static List<Card> GenerateUniqueCards()
        {
            List<Card> cards = [];

            foreach (var _ in Enumerable.Range(0, 2))
            {
                cards.Add(new UniqueCard(UniqueCardEnum.SuperTaki));
                cards.Add(new UniqueCard(UniqueCardEnum.SwitchCardsWithDirection));

                foreach (UniqueCardEnum uniqueCardEnum in uniqueCardsWithColor)
                    Enums.CardColors.Values.ToList()
                        .ForEach(color => cards.Add(new UniqueCard(uniqueCardEnum, color)));
            }

            foreach (var _ in Enumerable.Range(0, 4))
                cards.Add(new UniqueCard(UniqueCardEnum.ChangeColor));

            return cards;
        }

        public static int MaxNumberOfCards()
        {
            return GenerateCardDeck().GetNumberOfCards();
        }
    }
}
