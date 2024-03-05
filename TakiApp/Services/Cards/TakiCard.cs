using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class TakiCard : ColorCard
    {
        private readonly IUserCommunicator _userCommunicator;
        private readonly IPlayerService _playerService;

        public TakiCard(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository,
            IUserCommunicator userCommunicator, IPlayerService playerService) : 
            base(discardPileRepository, playersRepository)
        {
            _userCommunicator = userCommunicator;
            _playerService = playerService;
        }

        public override List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(TakiCard).ToString(), color.ToString())).ToList();

            return cards;
        }

        public override async Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {//TODO: check
            var isStackable = cardPlayService.CanStack(cardPlayed);
            Card topDiscard = cardPlayed;

            _userCommunicator.SendAlertMessage("Taki Open!\n");
            _userCommunicator.SendMessageToUser($"Only {topDiscard.CardColor} cards allowed");

            Card? playerCard = _playerService.PickCard(player, isStackable);

            while (playerCard is not null)
            {
                topDiscard = playerCard;
                _userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");
                await _discardPileRepository.AddCardAsync(playerCard);

                player.Cards.Remove(playerCard);
                await _playersRepository.UpdatePlayer(player);

                playerCard = _playerService.PickCard(player, isStackable, elseMessage: "or -1 to finish taki");
            }

            _userCommunicator.SendAlertMessage("Taki Closed!\n");

            if (cardPlayed != topDiscard)
                await cardPlayService.PlayCardAsync(player, topDiscard);
        }
    }
}
