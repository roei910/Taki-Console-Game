using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class ChangeColor : CardService
    {
        private readonly IAlgorithmService _algorithmService;

        public ChangeColor(IAlgorithmService algorithmService, IDiscardPileRepository discardPileRepository, 
            IPlayersRepository playersRepository) : 
            base(discardPileRepository, playersRepository)
        {
            _algorithmService = algorithmService;
        }

        public override bool CanStackOtherOnThis(Card topDiscard, Card otherCard)
        {
            if (topDiscard.CardColor == Color.Empty.ToString())
                return true;

            if (topDiscard.CardColor == otherCard.CardColor)
                return true;

            return base.CanStackOtherOnThis(topDiscard, otherCard);
        }

        public override List<Card> GenerateCardsForDeck()
        {
            return Enumerable.Range(0, 2)
                .Select(j => new Card(typeof(ChangeColor).ToString(), Color.Empty.ToString())).ToList();
        }

        public async override Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            while (!ColorCard.Colors.Any(x => x.ToString() == cardPlayed.CardColor.ToString()))
                cardPlayed.CardColor = _algorithmService.ChooseColor(player).ToString();

            await _discardPileRepository.UpdateCardAsync(cardPlayed);
            await _playersRepository.SendMessagesToPlayersAsync(player.Name!, $"Changed color to {cardPlayed.CardColor}", player);
            await base.PlayAsync(player, cardPlayed, cardPlayService);
        }

        public override async Task FinishPlayAsync(Card cardToReset)
        {
            cardToReset.CardColor = Color.Empty.ToString();

            await _discardPileRepository.UpdateCardAsync(cardToReset);
        }
    }
}
