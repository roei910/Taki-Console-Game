using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class SuperTaki : TakiCard
    {
        public SuperTaki(IDiscardPileRepository discardPileRepository, 
            IPlayersRepository playersRepository, IUserCommunicator userCommunicator, 
            IAlgorithmService algorithmService) : 
            base(discardPileRepository, playersRepository, userCommunicator, algorithmService) { }

        public override List<Card> GenerateCardsForDeck()
        {
            return Enumerable.Range(0, 2)
                .Select(j => new Card(typeof(SuperTaki).ToString(), Color.Empty.ToString())).ToList();
        }
    }
}
