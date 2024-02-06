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
            try
            {
                LinkedListNode<Card>? card = discardPile.Find(discardCard)?.Next;
                return card == null ? throw new NullReferenceException("null exception") : card.Value;
            }
            catch (Exception ex) 
            {
                throw new Exception("error with getting the next card", ex);
            }
        }

        public void DrawFirstCard()
        {
            TryDrawCard(out Card? card);
            while (card?.GetType() == typeof(UniqueCard))
            {
                drawPile.AddLast(card);
                TryDrawCard(out card);
            }
            AddCardToDiscardPile(card!);
        }

        public Card DrawCard()
        {
            if (drawPile.Count == 0)
            {
                drawPile = discardPile;
                drawPile = new();
                ShuffleDeck();
                DrawFirstCard();
            }
            Card card = drawPile.First();
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
            if (!discardPile.Remove(card))
                throw new Exception("error getting random discard card");
            return card;
        }
    }
}
