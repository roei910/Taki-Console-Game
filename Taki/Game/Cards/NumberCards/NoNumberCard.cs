using System.Drawing;
using Taki.Game.Messages;

namespace Taki.Game.Cards.NumberCards
{
    internal class NoNumberCard : NumberCard
    {
        public NoNumberCard(Color color, IUserCommunicator userCommunicator) : 
            base(0, color, userCommunicator) { }

        public override string[] GetStringArray()
        {
            throw new NotImplementedException();
        }
    }
}
