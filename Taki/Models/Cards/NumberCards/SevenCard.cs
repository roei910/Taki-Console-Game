using System.Drawing;
using Taki.Interfaces;

namespace Taki.Models.Cards.NumberCards
{
    internal class SevenCard : NumberCard
    {
        public SevenCard(Color color, IUserCommunicator userCommunicator) :
            base(7, color, userCommunicator)
        { }

        public override string[] GetStringArray()
        {
            return [
                "**********",
                "* ****** *",
                "*     *  *",
                "*    *   *",
                "*   *    *",
                "*  *     *",
                "**********"];
        }
    }
}
