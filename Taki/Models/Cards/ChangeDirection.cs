using System.Drawing;
using Taki.Interfaces;

namespace Taki.Models.Cards
{
    internal class ChangeDirection : ColorCard
    {
        public ChangeDirection(Color color, IUserCommunicator userCommunicator) :
            base(color, userCommunicator)
        { }

        public override string[] GetStringArray()
        {
            return [
                "*************",
                "*  CHANGE   *",
                "*           *",
                "*           *",
                "*           *",
                "* DIRECTION *",
                "*************"];
        }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is ChangeDirection;
        }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            _userCommunicator.SendErrorMessage("User used change direction card!\n");

            playersHolder.ChangeDirection();
            base.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override string ToString()
        {
            return $"ChangeDirection, {base.ToString()}";
        }
    }
}
