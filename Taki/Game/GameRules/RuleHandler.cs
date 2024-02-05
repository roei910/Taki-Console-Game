using System;
using System.Diagnostics;
using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.General;
using Taki.Game.Players;

namespace Taki.Game.GameRules
{
    //not important
    //TODO: fix messages in screen not appearing in the right timeline - happends in taki
    //TODO: check error stuck after switch cards with direction - seems to work, try again
    //TODO: fix error cannot put change direction on plus same color - tried to test, but worked. try again
    //TODO: if no one can play the game is a tie, must declare it.
    //TODO: need to edit the behavior of asking card, need to know how to ask for specific card only or every card that fits
    //TODO: pyramid error - player says cannot draw cards before winning
    internal class RuleHandler(PlayerHandler playerHandler, CardDeck cardDeck)
    {
        protected readonly PlayerHandler playerHandler = playerHandler;
        protected readonly CardDeck cardDeck = cardDeck;
        protected bool isDirectionNormal = true;
        protected Card? CurrentTakiCard { get; set; } = null;
        private int countPlus2 = 0;
        private Color changeColor = Color.Empty;

        public void PlayTurn()
        {
            Card topDiscard = cardDeck.GetTopDiscardPile();
            while (changeColor == Color.Empty && 
                !UniqueCard.IsChangeColor(topDiscard) && 
                topDiscard.Color == Color.Empty)
                topDiscard = cardDeck.GetNextDiscard(topDiscard);
            Player first = playerHandler.CurrentPlayer;
            if (changeColor != Color.Empty)
                topDiscard = new NumberCard("", changeColor);
            if (!first.AskPlayerToPickCard(topDiscard, out Card playerCard))
                HandlePlayerFinishTurn(first, topDiscard);
            else if (!TryHandleCard(topDiscard, playerCard))
            {
                playerHandler.ReturnUnhandledCard(playerCard);
                PlayTurn();
            }
        }

        private void HandlePlayerFinishTurn(Player first, Card topDiscard)
        {
            if (first.IsHandEmpty())
                return;
            if (CurrentTakiCard != null)
            {
                Utilities.PrintConsoleAlert($"Player[{first.Id}]: Taki closed!");
                if (UniqueCard.IsUniqueCard(topDiscard) && topDiscard.Id != CurrentTakiCard.Id)
                {
                    CurrentTakiCard = null;
                    HandleUniqueCard(topDiscard);
                }
                CurrentTakiCard = null;
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
            if (CurrentTakiCard == null)
                playerHandler.NextPlayer(isDirectionNormal);
        }

        private bool TryHandleCard(Card topDiscard, Card card)
        {
            if (!CheckCardFlags(topDiscard, card))
                return false;
            cardDeck.AddCardToDiscardPile(card);
            if (playerHandler.CurrentPlayer.IsHandEmpty())
                return true;
            if(CurrentTakiCard == null && UniqueCard.IsUniqueCard(card))
                HandleUniqueCard(card);
            Utilities.PrintConsoleAlert($"Player[{playerHandler.CurrentPlayer.Id}] played {card}");
            return true;
        }

        private void HandleUniqueCard(Card card)
        {
            Console.WriteLine($"card is {card}");
            if (UniqueCard.IsPlus(card))
                PlayTurn();
            else if (UniqueCard.IsPlus2(card))
                countPlus2++;
            else if (UniqueCard.IsTaki(card))
            {
                Console.WriteLine("TAKI open");
                CurrentTakiCard = card;
            }
            else if (UniqueCard.IsSuperTaki(card))
            {
                HandleUniqueCard(new UniqueCard(UniqueCardEnum.ChangeColor));
                HandleUniqueCard(new UniqueCard(UniqueCardEnum.Taki));
            }
            else if (UniqueCard.IsStop(card))
                playerHandler.NextPlayer(isDirectionNormal);
            else if (UniqueCard.IsChangeColor(card))
                changeColor = playerHandler.GetColorFromPlayer();
            else if (UniqueCard.IsChangeDirection(card))
                isDirectionNormal = !isDirectionNormal;
            else if (UniqueCard.IsSwitchCardsWithDirection(card))
                playerHandler.SwitchCardsWithDirectionCard(isDirectionNormal);
            else
                throw new NotImplementedException("card functionality not implemented yet");
        }

        private bool CheckCardFlags(Card topDiscard, Card card)
        {
            if (!changeColor.Equals(Color.Empty))
            {
                if (!card.CheckColorMatch(changeColor))
                {
                    Utilities.PrintConsoleError($"Please choose a {changeColor} color card");
                    return false;
                }
                if (card.Color != Color.Empty || CurrentTakiCard == null)
                    changeColor = Color.Empty;
            }
            else if (countPlus2 != 0)
            {
                if (!UniqueCard.IsPlus2(card) && countPlus2 > 0)
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
            return true;
        }

        public int GetWinner()
        {
            while (!PlayerFinishedHand())
            {
                Console.WriteLine("------------------------");
                PlayTurn();
                RequestNextPlayer();
            }
            return playerHandler.RemoveWinner(isDirectionNormal);
        }

        protected virtual bool PlayerFinishedHand()
        {
            return playerHandler.PlayerFinishedHand();
        }
    }
}