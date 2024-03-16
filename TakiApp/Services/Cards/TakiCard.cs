using System.Drawing;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Services.Cards
{
    public class TakiCard : ColorCard
    {
        protected readonly IAlgorithmService _algorithmService;

        public TakiCard(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository,
            IUserCommunicator userCommunicator, IAlgorithmService algorithmService) : 
            base(discardPileRepository, playersRepository, userCommunicator)
        {
            _algorithmService = algorithmService;
        }

        public override List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(TakiCard).ToString(), color.Name)).ToList();

            return cards;
        }

        public override async Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            Func<Card,bool> isStackable = (Card card) => card.CardColor == cardPlayed.CardColor;
            Card topDiscard = cardPlayed;

            _userCommunicator.SendAlertMessage($"Taki Open, Only {topDiscard.CardColor} cards allowed!\n");

            Card? playerCard = _algorithmService.PickCard(player, isStackable, elseMessage: "or -1 to finish taki");

            while (playerCard is not null)
            {
                topDiscard = playerCard;
                _userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");
                await _discardPileRepository.AddCardOrderedAsync(playerCard);

                player.Cards.Remove(playerCard);
                await _playersRepository.UpdatePlayerAsync(player);

                playerCard = _algorithmService.PickCard(player, isStackable, elseMessage: "or -1 to finish taki");
            }

            _userCommunicator.SendAlertMessage("Taki Closed!\n");

            if (cardPlayed != topDiscard)
            {
                await cardPlayService.PlayCardAsync(player, topDiscard);
                return;
            }

            await base.PlayAsync(player, cardPlayed, cardPlayService);
        }
    }
}