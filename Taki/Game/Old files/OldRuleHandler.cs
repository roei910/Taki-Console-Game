using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.General;
using Taki.Game.Players;

namespace Taki.Game.GameRules
{
    internal class OldRuleHandler(LinkedList<Player> players, CardDeck cardDeck)
    {
        LinkedList<Player> players = players;
        CardDeck cardDeck = cardDeck;
        private bool isDirectionNormal = true;

        public void PlayerPlay()
        {
            Player first = players.First();
            Card topDiscard = cardDeck.GetTopDiscardPile();
            if (!first.AskPlayerToPickCard(topDiscard, out Card userCard))
                first.AddCard(cardDeck.DrawCard());
            else if (!TryHandleCard(userCard))
            {
                first.AddCard(userCard);
                PlayerPlay();
            }
        }

        public void NextPlayer()
        {
            Player currentPlayer = players.First();
            if (isDirectionNormal)
            {
                players.RemoveFirst();
                players.AddLast(currentPlayer);
            }
            else
            {
                players.RemoveLast();
                players.AddFirst(currentPlayer);
            }
        }

        private bool TryHandleCard(Card card)
        {
            if (!TryAddCardToDiscardPile(card))
                return false;
            if (UniqueCard.IsUniqueCard(card))
                HandleUniqueCard(card);
            return true;
        }

        private bool TryAddCardToDiscardPile(Card card)
        {
            //TODO: ??? fix behavior where user takes out a card from hand when giving one

            if (!cardDeck.CanPlayCard(card))
            {
                Utilities.PrintConsoleError("Please follow the card stacking rules");
                return false;
            }
            cardDeck.AddCardToDiscardPile(card);
            return true;
        }

        private void HandleUniqueCard(Card card)
        {
            if (UniqueCard.IsPlus(card))
                PlayerPlay();
            else if (UniqueCard.IsPlus2(card))
                HandlePlus2Card();
            else if (UniqueCard.IsTaki(card))
                HandleTakiCard();
            else if (UniqueCard.IsSuperTaki(card))
                HandleSuperTakiCard();
            else if (UniqueCard.IsStop(card))
                NextPlayer();
            else if (UniqueCard.IsChangeColor(card))
                HandleChangeColorCard(Card.IsCard);
            else if (UniqueCard.IsChangeDirection(card))
                isDirectionNormal = !isDirectionNormal;
            else if (UniqueCard.IsSwitchCardsWithDirection(card))
                HandleSwitchCardsWithDirectionCard();
            else
                throw new Exception("something unexpected happened");
        }

        private void HandleSuperTakiCard()
        {
            HandleChangeColorCard(UniqueCard.IsTaki, skipCurrentPlayer: false);
            HandleTakiCard();
        }

        private void HandleChangeColorCard(Func<Card, bool> checkCardFunction, 
            bool skipCurrentPlayer = true)
        {
            Color color = Utilities.GetColorFromUserEnum<CardColorsEnum>("of color");
            if (skipCurrentPlayer)
                NextPlayer();
            GetColorCardFromUser(checkCardFunction, color);
        }

        private void HandleTakiCard()
        {
            //TODO: check how to add the last card as player next handle card
            

            Player first = players.First();
            Card topDiscard = cardDeck.GetTopDiscardPile();
            while (first.AskPlayerToPickCard(topDiscard, out Card userCard))
            {
                if (!TryHandleCard(userCard))
                    first.AddCard(userCard);
                topDiscard = userCard;
            }
            first.AddCard(cardDeck.DrawCard());
            Console.WriteLine("Taki closed!");
        }

        private void GetColorCardFromUser(Func<Card, bool> checkCardFunction, Color color)
        {
            //Utilities.PrintConsoleAlert($"Please choose a card with the new color: {color}");
            //Card playerCard = GetCardFromPlayer();
            //if (CheckCardIsTopDiscard(playerCard))
            //{
            //    PlayerDrawCard();
            //    NextPlayer();
            //    GetColorCardFromUser(checkCardFunction, color);
            //    return;
            //}
            //if (!(checkCardFunction(playerCard) && playerCard.CheckColorMatch(color)))
            //{
            //    Utilities.PrintConsoleError("Wrong color, try again");
            //    GetColorCardFromUser(checkCardFunction, color);
            //    return;
            //}
            //cardDeck.AddCardToDiscardPile(playerCard);
        }

        private void HandleSwitchCardsWithDirectionCard()
        {
            Player first = players.First();
            List<Card> savedCards = first.PlayerCards;
            first.PlayerCards = [];
            NextPlayer();
            while (!players.First().Equals(first))
            {
                List<Card> cards = players.First().PlayerCards;
                players.First().PlayerCards = savedCards;
                savedCards = cards;
                NextPlayer();
            }
            first.PlayerCards = savedCards;
        }

        private void HandlePlus2Card()
        {
            ////TODO: plus 2 * number of plus 2 cards
            //NextPlayer();
            //Console.WriteLine("Please choose a plus2 card or draw 2 cards from deck");
            //Card card = GetCardFromPlayer();
            //if (UniqueCard.IsPlus2(card))
            //    HandlePlus2Card();
            //else
            //    Enumerable.Range(0, 2).ToList().ForEach(_ => PlayerDrawCard());
        }
    }
}
