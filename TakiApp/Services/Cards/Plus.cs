using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class Plus : ColorCard
    {
        private readonly IUserCommunicator _userCommunicator;
        private readonly IAlgorithmService _algorithmService;

        public Plus(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository,
            IUserCommunicator userCommunicator, IAlgorithmService algorithService) : 
            base(discardPileRepository, playersRepository)
        {
            _userCommunicator = userCommunicator;
            _algorithmService = algorithService;
        }

        public override List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(Plus).ToString(), color.ToString())).ToList();

            return cards;
        }

        public override async Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {//TODO: test
            _userCommunicator.SendAlertMessage("please choose one more card or draw");

            _userCommunicator.SendAlertMessage($"Top discard: {cardPlayed.Type}, {cardPlayed.CardColor}");

            Func<Card, bool> canStack = (Card card) => CanStackOtherOnThis(cardPlayed, card);

            var playerCard = _algorithmService.PickCard(player, canStack);

            if (playerCard == null)
            {
                await _playersRepository.DrawCardsAsync(player, CardsToDraw(cardPlayed));

                return;
            }

            player.Cards.Remove(playerCard);

            await _playersRepository.UpdatePlayerAsync(player);

            await _discardPileRepository.AddCardOrderedAsync(playerCard);

            await cardPlayService.PlayCardAsync(player, playerCard);
        }
    }
}
