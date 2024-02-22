using System.Drawing;
using Taki.Game.Interfaces;

namespace Taki.Game.Models.Cards.NumberCards
{
    internal class NineCard : NumberCard
    {
        public NineCard(Color color, IUserCommunicator userCommunicator) :
            base(9, color, userCommunicator)
        { }

        public override string[] GetStringArray()
        {
            return [
                "**********",
                "* ****** *",
                "* *    * *",
                "* ****** *",
                "*      * *",
                "*      * *",
                "**********"];
        }
    }
}
