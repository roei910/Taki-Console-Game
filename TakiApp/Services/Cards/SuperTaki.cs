using System.Drawing;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    internal class SuperTaki : ICardService
    {
        public List<Card> GenerateCardsForDeck()
        {
            return Enumerable.Range(0, 2)
                .Select(j => new Card(typeof(ChangeColor).ToString(), Color.Empty.Name)).ToList();
        }
    }
}
