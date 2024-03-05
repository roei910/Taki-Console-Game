using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class ChangeColor : CardService
    {
        private readonly IPlayerService _playerService;

        public ChangeColor(IPlayerService playerService, IDiscardPileRepository discardPileRepository, 
            IPlayersRepository playersRepository) : 
            base(discardPileRepository, playersRepository)
        {
            _playerService = playerService;
        }

        public override bool CanStackOtherOnThis(Card topDiscard, Card otherCard)
        {
            if (topDiscard.CardColor == Color.Empty.ToString())
                return true;

            return base.CanStackOtherOnThis(topDiscard, otherCard);
        }

        public override List<Card> GenerateCardsForDeck()
        {
            return Enumerable.Range(0, 2)
                .Select(j => new Card(typeof(ChangeColor).ToString(), Color.Empty.Name)).ToList();
        }

        public async override Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            while (!ColorCard.Colors.Any(x => x.ToString() == cardPlayed.CardColor.ToString()))
                cardPlayed.CardColor = _playerService.ChooseColor(player).ToString();

            await _discardPileRepository.UpdateCardAsync(cardPlayed);
        }

        public override async Task ResetCard(Card cardToReset)
        {
            cardToReset.CardColor = Color.Empty.Name;

            await _discardPileRepository.UpdateCardAsync(cardToReset);
        }
    }
}
