using System.Drawing;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class NumberCard : ColorCard
    {
        public override List<Card> GenerateCardsForDeck()
        {
            var cards = Enumerable.Range(3, 7)
                .SelectMany(number => new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color =>
                    new Card(typeof(NumberCard).ToString() + number, color.Name))).ToList();

            return cards;
        }
    }
}
