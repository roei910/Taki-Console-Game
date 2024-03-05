using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class TakiCard : ColorCard
    {
        public TakiCard(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository) : 
            base(discardPileRepository, playersRepository) { }

        public override List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(TakiCard).ToString(), color.ToString())).ToList();

            return cards;
        }
    }
}
