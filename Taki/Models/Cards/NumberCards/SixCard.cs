using System.Drawing;
using Taki.Interfaces;

namespace Taki.Models.Cards.NumberCards
{
    internal class SixCard : NumberCard
    {
        public SixCard(Color color, IUserCommunicator userCommunicator) :
            base(6, color, userCommunicator)
        { }

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
