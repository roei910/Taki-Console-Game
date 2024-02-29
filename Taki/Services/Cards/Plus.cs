using System.Drawing;
using Taki.Models.Players;
using Taki.Shared.Abstract;
using Taki.Shared.Interfaces;

namespace Taki.Models.Cards
{
    public class Plus : ColorCard
    {
        public Plus(Color color, IUserCommunicator userCommunicator) :
            base(color, userCommunicator)
        { }

        public override string[] GetStringArray()
        {
            return [
                "***********",
                "*         *",
                "*    |    *",
                "*  --+--  *",
                "*    |    *",
                "*         *",
                "***********"];
        }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is Plus;
        }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            _userCommunicator.SendAlertMessage("please choose one more card or draw");

            _userCommunicator.SendAlertMessage($"Top discard: {this}");
            PrintCard();

            Player currentPlayer = playersHolder.CurrentPlayer;
            Card? playerCard = currentPlayer.PickCard(IsStackableWith);

            if (playerCard == null)
            {
                playersHolder.DrawCards(CardsToDraw(), currentPlayer, cardDecksHolder);
                base.Play(topDiscard, cardDecksHolder, playersHolder);

                return;
            }

            _userCommunicator.SendAlertMessage($"{currentPlayer.Name} chose {playerCard}\n");
            currentPlayer.PlayerCards.Remove(playerCard);
            cardDecksHolder.AddDiscardCard(playerCard);
            playerCard.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override string ToString()
        {
            return $"Plus {base.ToString()}";
        }
    }
}
