using System.Drawing;
using Taki.Game.Handlers;
using Taki.Game.Messages;

namespace Taki.Game.Cards
{
    internal class ChangeDirection : ColorCard
    {
        public ChangeDirection(Color color) : base(color) { }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is ChangeDirection;
        }

        public override void Play(IPlayersHandler playersHandler, ICardsHandler cardsHandler, IUserCommunicator userCommunicator)
        {
            playersHandler.ChangeDirection();
            base.Play(playersHandler, cardsHandler, userCommunicator);
        }

        public override string ToString()
        {
            return $"ChangeDirection, {base.ToString()}";
        }
    }
}
