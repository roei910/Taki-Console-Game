using System;
using System.Diagnostics;
using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.General;
using Taki.Game.Players;

namespace Taki.Game.GameRules
{
    internal class RuleHandler(PlayerHandler playerHandler, CardDeck cardDeck)
    {
        protected readonly PlayerHandler playerHandler = playerHandler;
        protected readonly CardDeck cardDeck = cardDeck;
        protected bool isDirectionNormal = true;
        protected Card? CurrentTakiCard { get; set; } = null;
        private int countPlus2 = 0;
        private Color changeColor = Color.Empty;
        private int noPlayCounter = 0;

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

            bool isPlayerPickCard = (countPlus2 > 0) ?
                first.AskPlayerToPickCardPlus2(topDiscard, out Card playerCard) :
                first.AskPlayerToPickCard(topDiscard, out playerCard);

            if (!isPlayerPickCard)
                HandlePlayerFinishTurn(first, topDiscard);
            else if (!TryHandleCard(topDiscard, playerCard))
            {
                noPlayCounter = 0;
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
            if (!playerHandler.DrawCards(numberOfDrawCards, cardDeck))
                noPlayCounter++;
            if (noPlayCounter == 8)
                throw new Exception("no play for 8 rounds, we will call this a tie ;)");
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
            noPlayCounter = 0;
            return true;
        }

        private void HandleUniqueCard(Card card)
        {
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