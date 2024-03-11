using System.Drawing;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Services.Cards
{
    public class ChangeDirection : ColorCard
    {
        public ChangeDirection(IDiscardPileRepository discardPileRepository, 
            IPlayersRepository playersRepository, IUserCommunicator userCommunicator) :
            base(discardPileRepository, playersRepository, userCommunicator) { }

        public override List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(ChangeDirection).ToString(), color.Name)).ToList();

            return cards;
        }

        public async override Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            await _playersRepository.SendMessagesToPlayersAsync(player.Name!, $"Changing direction!\n", player, player);

            var players = await _playersRepository.GetAllAsync();

            if (players.Count == 0)
                throw new Exception("something went wrong!");

            players = players.OrderByDescending(x => x.Order).ToList();

            await _playersRepository.UpdateOrder(players);

            await base.PlayAsync(player, cardPlayed, cardPlayService);
        }
    }
}
