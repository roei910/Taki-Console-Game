﻿using Taki.Game.Cards;
using Taki.Game.Cards.DTOs;

namespace Taki.Game.Deck
{
    internal interface ICardDecksHolder
    {
        Card GetTopDiscard();
        void AddDiscardCard(Card card);
        void ResetCards();
        void ResetCards(List<Card> playerCards);
        Card? DrawCard();
        void DrawFirstCard();
        int CountAllCards();
        Card RemoveCardByDTO(CardDto card);
        CardDeck GetDrawPile();
        CardDeck GetDiscardPile();
        void UpdateCardsFromDB(List<CardDto> drawPile, List<CardDto> discardPile);
    }
}