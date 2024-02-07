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
        public Color ChangeColor { get; private set; } = Color.Empty;
        protected Card? CurrentTakiCard { get; set; } = null;
        private int countPlus2 = 0;
        private int noPlayCounter = 0;

        public bool IsTaki()
        {
            return CurrentTakiCard != null;
        }

        public bool IsPlus2()
        {
            return countPlus2 > 0;
        }

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
            if (CurrentTakiCard == null)
                throw new Exception("trying to close taki by accident");
            Communicator.PrintMessage($"Player[{first.Id}]: Taki closed!", Communicator.MessageType.Alert);
            if (UniqueCard.IsUniqueCard(topDiscard) && topDiscard.Id != CurrentTakiCard.Id)
                next(topDiscard);
            CloseTaki();
        }

        internal void CloseTaki()
        {
            CurrentTakiCard = null;
        }

        internal void FinishPlus2(CardDeck cardDeck, Func<int, CardDeck, bool> drawCards)
        {
            int numberOfDrawCards = countPlus2 > 0 ? countPlus2 * 2 : 1;
            countPlus2 = 0;
            if (!drawCards(numberOfDrawCards, cardDeck))
                noPlayCounter++;
        }

        internal void IncrementPlus2()
        {
            countPlus2++;
        }

        internal void ResetPlayCounter()
        {
            noPlayCounter = 0;
        }

        internal void UpdateTakiCard(Card currentTaki)
        {
            CurrentTakiCard = currentTaki;
        }

        internal void UpdateChangeColor(Color color)
        {
            ChangeColor = color;
        }
    }
}