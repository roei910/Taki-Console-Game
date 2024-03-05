using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class ChangeDirection : ColorCard
    {
        private readonly IUserCommunicator _userCommunicator;

        public ChangeDirection(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository,
            IUserCommunicator userCommunicator) :
            base(discardPileRepository, playersRepository) 
        {
            _userCommunicator = userCommunicator;
        }

        public override List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(ChangeDirection).ToString(), color.ToString())).ToList();

            return cards;
        }

        public async override Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            _userCommunicator.SendErrorMessage("User used change direction card!\n");

            var players = await _playersRepository.GetAllAsync();

            if (players.Count == 0)
                throw new Exception("something went wrong!");

            players = players.OrderByDescending(x => x.Order).ToList();

            await _playersRepository.UpdateOrder(players);
        }
    }
}
