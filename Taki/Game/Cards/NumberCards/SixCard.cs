using System.Drawing;
using Taki.Game.Messages;

namespace Taki.Game.Cards.NumberCards
{
    internal class SixCard : NumberCard
    {
        public SixCard(Color color, IUserCommunicator userCommunicator) : 
            base(6, color, userCommunicator) { }

        public override string[] GetStringArray()
        {
            return [
                "**********",
                "* ****** *",
                "* *      *",
                "* ****** *",
                "* *    * *",
                "* ****** *",
                "**********"];
        }
    }
}
