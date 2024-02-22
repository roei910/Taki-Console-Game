using System.Drawing;
using Taki.Game.Interfaces;

namespace Taki.Game.Models.Cards.NumberCards
{
    internal class FiveCard : NumberCard
    {
        public FiveCard(Color color, IUserCommunicator userCommunicator) :
            base(5, color, userCommunicator)
        { }

        public override string[] GetStringArray()
        {
            return [
                "**********",
                "* ****** *",
                "* *      *",
                "* ****** *",
                "*      * *",
                "* ****** *",
                "**********"];
        }
    }
}
