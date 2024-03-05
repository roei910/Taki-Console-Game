using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class ChangeColor : ICardService
    {
        private readonly IDiscardPileRepository _discardPileRepository;
        private readonly IPlayersRepository _playersRepository;
        private readonly IPlayerService _playerService;

        public ChangeColor(IDiscardPileRepository discardPileRepository,
            IPlayersRepository playersRepository,
            IPlayerService playerService)
        {
            _discardPileRepository = discardPileRepository;
            _playersRepository = playersRepository;
            _playerService = playerService;
        }

        public bool CanStackOtherOnThis(Card topDiscard, Card otherCard)
        {
            if (topDiscard.CardColor == Color.Empty.ToString())
                return true;

            return topDiscard.CardColor == otherCard.CardColor;
        }

        public int CardsToDraw(Card cardPlayed)
        {
            throw new NotImplementedException();
        }

        public void FinishNoPlay(Card cardPlayed) { }

        public List<Card> GenerateCardsForDeck()
        {
            return Enumerable.Range(0, 2)
                .Select(j => new Card(typeof(ChangeColor).ToString(), Color.Empty.Name)).ToList();
        }

        public async Task PlayAsync(Player player, Card cardPlayed, Card topDiscard)
        {
            while (!ColorCard.Colors.Contains(Color.FromName(cardPlayed.CardColor)))
                cardPlayed.CardColor = _playerService.ChooseColor(player).Name;

            await _discardPileRepository.AddCardAsync(cardPlayed);
            await _playersRepository.NextPlayerAsync(player);
        }

        public void ResetCard(Card cardToReset)
        {
            cardToReset.CardColor = Color.Empty.Name;

            _discardPileRepository.UpdateCard(cardToReset);
        }
    }
}
