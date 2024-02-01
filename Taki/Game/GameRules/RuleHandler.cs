using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    //TODO: something wrong with taki or super taki - fix
    internal class RuleHandler(LinkedList<Player> players, CardDeck cardDeck)
    {
        readonly PlayerHandler playerHandler = new(players);
        readonly CardDeck cardDeck = cardDeck;
        private bool isDirectionNormal = true;
        private bool isTaki = false;
        private int countPlus2 = 0;
        private Color changeColor = Color.Empty;

        public void PlayTurn()
        {
            Card topDiscard = cardDeck.GetTopDiscardPile();
            //while (UniqueCard.IsSwitchCardsWithDirection(topDiscard))//need to check all cards without color
            //    topDiscard = cardDeck.GetNextDiscard(topDiscard);
            while (topDiscard.Color == Color.Empty)
                topDiscard = cardDeck.GetNextDiscard(topDiscard);
            Player first = players.First();
            topDiscard = CheckCardFlags(topDiscard);
            //TODO: check why cards get out of hand and not return
            if (!first.AskPlayerToPickCard(topDiscard, out Card playerCard))
            {
                HandlePlayerFinishTurn(first, topDiscard);
                return;
            }
            if (!TryHandleCard(topDiscard, playerCard))
            {
                playerHandler.ReturnUnhandledCard(playerCard);
                PlayTurn();
            }
        }

        private void HandlePlayerFinishTurn(Player first, Card topDiscard)
        {
            if (first.IsHandEmpty())
                return;
            if (isTaki)
            {
                isTaki = false;
                Utilities.PrintConsoleAlert($"Player[{first.Id}]: Taki closed!");
                //TODO: i want to handle the last card used in taki not including the taki itself!!
                //if (UniqueCard.IsUniqueCard(topDiscard))
                //    HandleUniqueCard(topDiscard);
                return;
            }
            int numberOfDrawCards = countPlus2 > 0 ? countPlus2 * 2 : 1;
            countPlus2 = 0;
            playerHandler.DrawCards(numberOfDrawCards, cardDeck);
        }

        public void RequestNextPlayer()
        {
            if (playerHandler.CurrentPlayer.IsHandEmpty())
                return;
            if (!isTaki)
                playerHandler.NextPlayer(isDirectionNormal);
        }

        private bool TryHandleCard(Card topDiscard, Card card)
        {
            //TODO: need to check if is taki to know what to check.
            if (!changeColor.Equals(Color.Empty))
            {
                if (!card.CheckColorMatch(changeColor))
                {
                    Utilities.PrintConsoleError($"Please choose a {changeColor} color card");
                    return false;
                }

                //TODO: check this new if and see if it works
                //should only regard the cards without color
                if (topDiscard.Color != Color.Empty)
                    changeColor = Color.Empty;
            }
            else if (countPlus2 != 0)
            {
                if (!UniqueCard.IsPlus2(card))
                {
                    Utilities.PrintConsoleError($"you can only put plus2 cards");
                    return false;
                }
            }
            else if (!card.SimilarTo(topDiscard))
            {
                Utilities.PrintConsoleError("Please follow the card stacking rules");
                return false;
            }
            
            cardDeck.AddCardToDiscardPile(card);
            if(!isTaki && UniqueCard.IsUniqueCard(card))
                HandleUniqueCard(card);
            Utilities.PrintConsoleAlert($"Player[{players.First().Id}] played {card}");
            return true;
        }

        private void HandleUniqueCard(Card card)
        {
            if (UniqueCard.IsPlus(card))
                PlayTurn();
            else if (UniqueCard.IsPlus2(card))
                countPlus2++;
            else if (UniqueCard.IsTaki(card))
                isTaki = true;
            else if (UniqueCard.IsSuperTaki(card))
            {
                HandleUniqueCard(new UniqueCard(UniqueCardEnum.ChangeColor));
                HandleUniqueCard(new UniqueCard(UniqueCardEnum.Taki));
            }
            else if (UniqueCard.IsStop(card))
                playerHandler.NextPlayer(isDirectionNormal);
            else if (UniqueCard.IsChangeColor(card))
            {
                changeColor = playerHandler.GetColorFromPlayer();
                //changeColor = Utilities.GetColorFromUserEnum<CardColorsEnum>("of color");
                //Utilities.PrintConsoleAlert($"Please choose a card with the new color: {changeColor}");
            }
            else if (UniqueCard.IsChangeDirection(card))
                isDirectionNormal = !isDirectionNormal;
            else if (UniqueCard.IsSwitchCardsWithDirection(card))
                playerHandler.SwitchCardsWithDirectionCard(isDirectionNormal);
            else
                throw new NotImplementedException("card functionality not implemented yet");
        }

        private Card CheckCardFlags(Card topDiscard)
        {
            if (UniqueCard.IsPlus2(topDiscard) && countPlus2 == 0)
                topDiscard = new NumberCard("", topDiscard.Color);
            if (changeColor != Color.Empty)//problem with change color
                topDiscard = new NumberCard("", changeColor);
            return topDiscard;
        }

        public int RemoveWinner()
        {
            return playerHandler.RemoveWinner(isDirectionNormal);
        }

        internal bool PlayerFinishedHand()
        {
            return playerHandler.CurrentPlayer.IsHandEmpty();
        }
    }
}