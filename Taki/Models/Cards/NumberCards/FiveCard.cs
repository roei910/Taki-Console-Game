using System.Drawing;
using Taki.Interfaces;

namespace Taki.Models.Cards.NumberCards
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
