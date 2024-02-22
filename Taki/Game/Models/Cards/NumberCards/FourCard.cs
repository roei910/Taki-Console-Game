using System.Drawing;
using Taki.Game.Interfaces;

namespace Taki.Game.Models.Cards.NumberCards
{
    internal class FourCard : NumberCard
    {
        public FourCard(Color color, IUserCommunicator userCommunicator) :
            base(4, color, userCommunicator)
        { }

        public override string[] GetStringArray()
        {
            return [
                "**********",
                "* *    * *",
                "* *    * *",
                "* ****** *",
                "*      * *",
                "*      * *",
                "**********"];
        }
    }
}
