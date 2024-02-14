using System.Drawing;
using Taki.Game.Handlers;
using Taki.Game.Messages;

namespace Taki.Game.Cards
{
    internal class Plus : ColorCard
    {
        public Plus(Color color) : base(color) { }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is Plus;
        }

        public override void Play(IPlayersHandler playersHandler, 
            ICardsHandler cardsHandler, IUserCommunicator userCommunicator) { }

        public override string ToString()
        {
            return $"Plus {base.ToString()}";
        }
    }
}
