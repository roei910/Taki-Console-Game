using System.Drawing;
using Taki.Shared.Interfaces;

namespace Taki.Models.Cards.NumberCards
{
    //TODO: can be moved to config file
    public class SevenCard : NumberCard
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
