using System.Drawing;
using Taki.Shared.Interfaces;

namespace Taki.Models.Cards.NumberCards
{
    public class NineCard : NumberCard
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
