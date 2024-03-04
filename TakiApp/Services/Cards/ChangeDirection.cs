using System.Drawing;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class ChangeDirection : ColorCard
    {
        public override List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(ChangeDirection).ToString(), color.Name)).ToList();

            return cards;
        }
    }
}
