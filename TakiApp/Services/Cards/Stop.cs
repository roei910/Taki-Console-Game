using System.Drawing;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Services.Cards
{
    public class Stop : ColorCard
    {
        public Stop(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository,
            IUserCommunicator userCommunicator) : 
            base(discardPileRepository, playersRepository, userCommunicator) { }

        public override List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(Stop).ToString(), color.Name)).ToList();

            return cards;
        }

        public async override Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            await _playersRepository.SkipPlayers(1);
        }
    }
}
