using System.Drawing;
using Taki.Models.Players;
using Taki.Shared.Abstract;
using Taki.Shared.Interfaces;

namespace Taki.Models.Cards
{
    internal class TakiCard : ColorCard
    {
        public TakiCard(Color color, IUserCommunicator userCommunicator) :
            base(color, userCommunicator)
        { }

        public override string[] GetStringArray()
        {
            return [
                "**********",
                "*  TAKI  *",
                "*        *",
                "*        *",
                "*        *",
                "*        *",
                "**********"];
        }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is TakiCard;
        }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            Player currentPlayer = playersHolder.CurrentPlayer;
            Func<Card, bool> isStackable = card => card is ColorCard colorCard && colorCard.GetColor() == _color;
            Card previous = topDiscard;
            topDiscard = this;
            _userCommunicator.SendAlertMessage("Taki Open!\n");
            _userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");
            topDiscard.PrintCard();

            Card? playerCard = playersHolder.GetCardFromCurrentPlayer(
                cardDecksHolder, isStackable, elseMessage: "or -1 to finish taki");

            while (playerCard is not null)
            {
                _userCommunicator.SendAlertMessage($"{currentPlayer.Name} chose " +
                    $"{playerCard}");

                previous = topDiscard;
                topDiscard = playerCard;
                _userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");
                topDiscard.PrintCard();

                playerCard = playersHolder.GetCardFromCurrentPlayer(
                    cardDecksHolder, isStackable, elseMessage: "or -1 to finish taki");
            }

            _userCommunicator.SendAlertMessage("Taki Closed!\n");

            if (!Equals(topDiscard))
            {
                topDiscard.Play(previous, cardDecksHolder, playersHolder);
                return;
            }

            base.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override string ToString()
        {
            return $"TAKI, {base.ToString()}";
        }
    }
}
