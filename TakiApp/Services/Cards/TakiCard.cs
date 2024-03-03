using System.Drawing;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    internal class TakiCard : ICardService
    {
        public List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(TakiCard).ToString(), color.Name)).ToList();

            return cards;
        }
    }
}
