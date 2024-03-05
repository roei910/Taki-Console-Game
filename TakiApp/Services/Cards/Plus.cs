using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class Plus : ColorCard
    {
        private readonly IUserCommunicator _userCommunicator;
        private readonly IPlayerService _playerService;

        public Plus(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository,
            IUserCommunicator userCommunicator, IPlayerService playerService) : 
            base(discardPileRepository, playersRepository)
        {
            _userCommunicator = userCommunicator;
            _playerService = playerService;
        }

        public override List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(Plus).ToString(), color.ToString())).ToList();

            return cards;
        }

        public override async Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            _userCommunicator.SendAlertMessage("please choose one more card or draw");

            _userCommunicator.SendAlertMessage($"Top discard: {this}");

            Func<Card, bool> canStack = (Card card) => CanStackOtherOnThis(cardPlayed, card);

            var playerCard = _playerService.PickCard(player, canStack);

            if (playerCard == null)
            {
                await _playersRepository.DrawCards(player, CardsToDraw(cardPlayed));
                await _playersRepository.NextPlayerAsync(player);

                return;
            }

            player.Cards.Remove(playerCard);

            await _playersRepository.UpdatePlayer(player);

            await _discardPileRepository.AddCardAsync(playerCard);

            await cardPlayService.PlayCardAsync(player, playerCard);
        }
    }
}
