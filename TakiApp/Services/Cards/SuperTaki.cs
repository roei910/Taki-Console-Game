using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class SuperTaki : TakiCard
    {
        public SuperTaki(IDiscardPileRepository discardPileRepository, 
            IPlayersRepository playersRepository, IUserCommunicator userCommunicator, 
            IAlgorithmService algorithmService) : 
            base(discardPileRepository, playersRepository, userCommunicator, algorithmService) { }

        public override List<Card> GenerateCardsForDeck()
        {
            return Enumerable.Range(0, 2)
                .Select(j => new Card(typeof(SuperTaki).ToString(), Color.Empty.ToString())).ToList();
        }

        public override async Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            while (!Colors.Any(x => x.ToString() == cardPlayed.CardColor.ToString()))
                cardPlayed.CardColor = _algorithmService.ChooseColor(player).ToString();

            await _discardPileRepository.UpdateCardAsync(cardPlayed);
            await _playersRepository.SendMessagesToPlayersAsync(player.Name!, $"Changed color to {cardPlayed.CardColor}", player);

            await base.PlayAsync(player, cardPlayed, cardPlayService);
        }

        public override bool CanStackOtherOnThis(Card topDiscard, Card otherCard, ICardPlayService cardPlayService)
        {
            if (topDiscard.CardColor == DEFAULT_COLOR.ToString())
                return true;

            return base.CanStackOtherOnThis(topDiscard, otherCard, cardPlayService);
        }

        public override async Task FinishPlayAsync(Card cardToReset)
        {
            Card topDiscard = await _discardPileRepository.GetTopDiscardAsync();

            if (topDiscard.Id != cardToReset.Id)
            {
                cardToReset.CardColor = DEFAULT_COLOR.ToString();
                await _discardPileRepository.UpdateCardAsync(cardToReset);
            }
        }
    }
}
