using System.Drawing;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class ChangeDirection : ColorCard
    {
        public ChangeDirection(Color color, IUserCommunicator userCommunicator) : 
            base(color, userCommunicator) { }

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

        public override void PrintCard()
        {
            string[] numberInArray = [
                "*************",
                "*  CHANGE   *",
                "*           *",
                "*           *",
                "*           *",
                "* DIRECTION *",
                "*************"];

            _userCommunicator.SendColorMessageToUser(_color, string.Join("\n", numberInArray));
        }

        public override string ToString()
        {
            return $"ChangeDirection, {base.ToString()}";
        }
    }
}
