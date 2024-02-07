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
    internal class MoveHandler
    {
        public bool IsTaki { get; private set; } = false;
        public bool IsPlus2 { get; private set; } = false;
        public Color ChangeColor { get; private set; } = Color.Empty;
        protected Card? CurrentTakiCard { get; set; } = null;
        private int countPlus2 = 0;
        private int noPlayCounter = 0;

        internal static bool IsValidDiscard(Card topDiscard, Color changeColor)
        {
            return changeColor == Color.Empty &&
                !UniqueCard.IsChangeColor(topDiscard) &&
                topDiscard.Color == Color.Empty;
        }

        internal void CheckForTie()
        {
            if (noPlayCounter == 8)
                throw new Exception("no play for 8 rounds, we will call this a tie ;)");
        }

        internal void CloseTaki(Card topDiscard, Player first, Action<Card> next)
        {
            if (CurrentTakiCard != null)
            {
                Communicator.PrintMessage($"Player[{first.Id}]: Taki closed!", Communicator.MessageType.Alert);
                if (UniqueCard.IsUniqueCard(topDiscard) && topDiscard.Id != CurrentTakiCard.Id)
                {
                    CurrentTakiCard = null;
                    next(topDiscard);
                }
                CurrentTakiCard = null;
                return;
            }
        }

        internal void FinishPlus2(CardDeck cardDeck, Func<int, CardDeck, bool> drawCards)
        {
            int numberOfDrawCards = countPlus2 > 0 ? countPlus2 * 2 : 1;
            countPlus2 = 0;
            if (!drawCards(numberOfDrawCards, cardDeck))
                noPlayCounter++;
            IsPlus2 = false;
        }

        internal void ResetPlayCounter()
        {
            noPlayCounter = 0;
        }

        internal void IncrementPlus2()
        {
            countPlus2++;
            IsPlus2 = true;
        }

        internal void UpdateTakiCard(Card currentTaki)
        {
            CurrentTakiCard = currentTaki;
            IsTaki = true;
        }

        internal void UpdateChangeColor(Color color)
        {
            ChangeColor = color;
        }

        internal void CloseTaki()
        {
            CurrentTakiCard = null;
        }
    }
}