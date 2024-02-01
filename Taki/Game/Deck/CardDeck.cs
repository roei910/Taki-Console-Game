using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Cards;

namespace Taki.Game.Deck
{
    internal class CardDeck
    {
        private LinkedList<Card> drawPile;
        private readonly LinkedList<Card> discardPile;

        public CardDeck(LinkedList<Card> cards)
        {
            discardPile = cards;
            drawPile = new();
            ShuffleDeck();
        }

        public Card GetTopDrawCard()
        {
            return drawPile.First();
        }

        public Card GetTopDiscardPile()
        {
            try
            {
                return discardPile.First();
            }
            catch
            {
                throw new Exception("Error occured while trying to extract the first card");
            }
        }

        public Card GetNextDiscard(Card discardCard)
        {
            return discardPile.Find(discardCard)!.Next!.Value;
        }

        public void DrawFirstCard()
        {
            TryDrawFirstCard(out Card? card);
            while (card?.GetType() == typeof(UniqueCard))
            {
                AddCardToEndOfDrawPile(card);
                TryDrawFirstCard(out card);
            }
            AddCardToDiscardPile(card!);
        }

        public void AddCardToEndOfDrawPile(Card card)
        {
            drawPile.AddLast(card);
        }

        public Card DrawCard()
        {
            if (IsDrawPileEmpty())
            {
                drawPile = discardPile;
                drawPile = new();
                ShuffleDeck();
                DrawFirstCard();
            }
            Card card = GetTopDrawCard();
            drawPile.Remove(card);
            return card;
        }

        public bool TryDrawCard(out Card? card)
        {
            card = null;
            int totalCards = drawPile.Count + discardPile.Count;
            if (totalCards <= 1)
                return false;
            try
            {
                card = DrawCard();
            }
            catch (Exception e)
            {
                throw new Exception("Error removing the card from draw pile", e);
            }
            return true;
        }

        public void AddCardToDiscardPile(Card card)
        {
            discardPile.AddFirst(card);
        }

        public bool IsDrawPileEmpty()
        {
            return drawPile.Count == 0;
        }

        public int GetNumberOfCards()
        {
            return discardPile.Count + drawPile.Count;
        }

        public override string ToString()
        {
            return $"Card deck: {drawPile.Count} cards in draw pile, {discardPile.Count} card in discard pile";
        }

        private void ShuffleDeck()
        {
            while (discardPile.Count != 0)
            {
                Card card = GetRandomDiscardCard();
                drawPile.AddFirst(card);
            }
        }

        private Card GetRandomDiscardCard()
        {
            Random random = new ();
            int index = random.Next(discardPile.Count);
            Card card = discardPile.ElementAt(index);
            discardPile.Remove(card);
            return card;
        }

        private bool TryDrawFirstCard(out Card? card)
        {
            if (!TryDrawCard(out card))
                return false;
            if (card == null)
                return false;
            return true;
        }
    }
}
