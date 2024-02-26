using System.Drawing;
using Taki.Shared.Interfaces;

namespace Taki.Models.Cards.NumberCards
{
    internal class ThreeCard : NumberCard
    {
        public ThreeCard(Color color, IUserCommunicator userCommunicator) :
            base(3, color, userCommunicator)
        { }

        public override string[] GetStringArray()
        {
            return [
                "**********",
                "* ****** *",
                "*      * *",
                "*  ***** *",
                "*      * *",
                "* ****** *",
                "**********"];
        }
    }
}
