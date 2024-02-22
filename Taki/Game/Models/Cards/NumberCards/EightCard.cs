using System.Drawing;
using Taki.Game.Interfaces;

namespace Taki.Game.Models.Cards.NumberCards
{
    internal class EightCard : NumberCard
    {
        public EightCard(Color color, IUserCommunicator userCommunicator) :
            base(8, color, userCommunicator)
        { }

        public override string[] GetStringArray()
        {
            return [
                "**********",
                "*  ****  *",
                "* *    * *",
                "*  ****  *",
                "* *    * *",
                "*  ****  *",
                "**********"];
        }
    }
}
