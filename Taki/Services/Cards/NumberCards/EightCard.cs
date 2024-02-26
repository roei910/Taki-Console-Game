using System.Drawing;
using Taki.Shared.Interfaces;

namespace Taki.Models.Cards.NumberCards
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
