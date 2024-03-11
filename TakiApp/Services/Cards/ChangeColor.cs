using System.Drawing;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Services.Cards
{
    public class ChangeColor : CardService
    {
        private readonly IAlgorithmService _algorithmService;

        public ChangeColor(IAlgorithmService algorithmService, IDiscardPileRepository discardPileRepository, 
            IPlayersRepository playersRepository, IUserCommunicator userCommunicator) : 
            base(discardPileRepository, playersRepository, userCommunicator)
        {
            _algorithmService = algorithmService;
        }

        public override bool CanStackOtherOnThis(Card topDiscard, Card otherCard, ICardPlayService cardPlayService)
        {
            if (topDiscard.CardColor == Color.Empty.Name)
                return true;

            if (topDiscard.CardColor == otherCard.CardColor)
                return true;

            return base.CanStackOtherOnThis(topDiscard, otherCard, cardPlayService);
        }

        public override List<Card> GenerateCardsForDeck()
        {
            return Enumerable.Range(0, 2)
                .Select(j => new Card(typeof(ChangeColor).ToString(), Color.Empty.Name)).ToList();
        }

        public async override Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            while (!ColorCard.Colors.Any(x => x.Name == cardPlayed.CardColor))
                cardPlayed.CardColor = _algorithmService.ChooseColor(player).Name;

            await _discardPileRepository.UpdateCardAsync(cardPlayed);
            await _playersRepository.SendMessagesToPlayersAsync(player.Name!, $"Changed color to {cardPlayed.CardColor}\n", player);

            await base.PlayAsync(player, cardPlayed, cardPlayService);
        }

        public override async Task FinishPlayAsync(Card cardToReset)
        {
            cardToReset.CardColor = Color.Empty.Name;

            await _discardPileRepository.UpdateCardAsync(cardToReset);
        }
    }
}
