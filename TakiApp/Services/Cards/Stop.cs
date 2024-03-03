using System.Drawing;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    internal class Stop : ICardService
    {
        public List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(Stop).ToString(), color.Name)).ToList();

            return cards;
        }
    }
}
