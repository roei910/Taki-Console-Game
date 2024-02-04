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
    internal class RuleHandler(PlayerHandler playerHandler, CardDeck cardDeck)
    {
        protected readonly PlayerHandler playerHandler = playerHandler;
        protected readonly CardDeck cardDeck = cardDeck;
        private bool isDirectionNormal = true;
        private Card? CurrentTakiCard { get; set; } = null;
        private int countPlus2 = 0;
        private Color changeColor = Color.Empty;

        public void PlayTurn()
        {
            Card topDiscard = cardDeck.GetTopDiscardPile();
            while (!UniqueCard.IsChangeColor(topDiscard) && topDiscard.Color == Color.Empty)
                topDiscard = cardDeck.GetNextDiscard(topDiscard);
            Player first = playerHandler.CurrentPlayer;
            topDiscard = CheckCardFlags(topDiscard);
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

        //TODO: shorten
        private bool TryHandleCard(Card topDiscard, Card card)
        {
            if (!changeColor.Equals(Color.Empty))
            {
                if (!card.CheckColorMatch(changeColor))
                {
                    Utilities.PrintConsoleError($"Please choose a {changeColor} color card");
                    return false;
                }
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

        private Card CheckCardFlags(Card topDiscard)
        {
            if (UniqueCard.IsPlus2(topDiscard) && countPlus2 == 0)
                topDiscard = new NumberCard("", topDiscard.Color);
            if (changeColor != Color.Empty)
                topDiscard = new NumberCard("", changeColor);
            return topDiscard;
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