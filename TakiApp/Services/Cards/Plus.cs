using System.Drawing;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Services.Cards
{
    public class Plus : ColorCard
    {
        private readonly IAlgorithmService _algorithmService;

        public Plus(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository,
            IUserCommunicator userCommunicator, IAlgorithmService algorithService) : 
            base(discardPileRepository, playersRepository, userCommunicator)
        {
            _algorithmService = algorithService;
        }

        public override List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(Plus).ToString(), color.Name)).ToList();

            return cards;
        }

        public override async Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            _userCommunicator.SendAlertMessage("Please choose one more card or draw\n");

            _userCommunicator.SendAlertMessage($"Top discard: {cardPlayed}\n");

            Func<Card, bool> canStack = (Card card) => CanStackOtherOnThis(cardPlayed, card, cardPlayService);

            var playerCard = _algorithmService.PickCard(player, canStack);

            if (playerCard == null)
            {
                var cardsDrew = await _playersRepository.DrawCardsAsync(player, CardsToDraw(cardPlayed));

                if (cardsDrew.Count == 0)
                    _userCommunicator.SendErrorMessage("Couldnt draw cards from deck");

                await base.PlayAsync(player, cardPlayed, cardPlayService);

                return;
            }

            player.Cards.Remove(playerCard);

            await _playersRepository.UpdatePlayerAsync(player);

            await _discardPileRepository.AddCardOrderedAsync(playerCard);

            await cardPlayService.PlayCardAsync(player, playerCard);
        }
    }
}
