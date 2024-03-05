using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class Stop : ColorCard
    {
        private readonly IUserCommunicator _userCommunicator;

        public Stop(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository,
            IUserCommunicator userCommunicator) : 
            base(discardPileRepository, playersRepository)
        {
            _userCommunicator = userCommunicator;
        }

        public override List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(Stop).ToString(), color.ToString())).ToList();

            return cards;
        }

        public async override Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            await _playersRepository.NextPlayerAsync();

            var nextPlayer = await _playersRepository.GetCurrentPlayerAsync();

            _userCommunicator.SendErrorMessage($"Player {nextPlayer.Name} was stopped by {player.Name}");
        }
    }
}
