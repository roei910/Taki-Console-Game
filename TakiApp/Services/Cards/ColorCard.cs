﻿using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public abstract class ColorCard : ICardService
    {
        public static readonly Color DEFAULT_COLOR = Color.Empty;
        public static List<Color> Colors = [Color.Green, Color.Red, Color.Yellow, Color.Blue];

        protected readonly IDiscardPileRepository _discardPileRepository;
        protected readonly IPlayersRepository _playersRepository;

        public ColorCard(IDiscardPileRepository discardPileRepository,
            IPlayersRepository playersRepository)
        {
            _discardPileRepository = discardPileRepository;
            _playersRepository = playersRepository;
        }

        public virtual async Task PlayAsync(Player player, Card cardPlayed, Card topDiscard)
        {
            await _discardPileRepository.AddCardAsync(cardPlayed);
            await _playersRepository.NextPlayerAsync(player);
        }

        public virtual bool CanStackOtherOnThis(Card topDiscard, Card otherCard)
        {
            if (topDiscard.CardColor == DEFAULT_COLOR.Name)
                return true;

            if (topDiscard.Type == otherCard.Type)
                return true;

            return topDiscard.CardColor == otherCard.CardColor;
        }

        public virtual int CardsToDraw(Card cardPlayed) => 1;

        public virtual void FinishNoPlay(Card cardPlayed) { }

        public virtual void ResetCard(Card cardToReset) { }

        public abstract List<Card> GenerateCardsForDeck();
    }
}
