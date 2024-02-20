using System.Drawing;
using Taki.Game.Messages;

namespace Taki.Game.Cards.NumberCards
{
    internal class EightCard : NumberCard
    {
        public EightCard(Color color, IUserCommunicator userCommunicator) : 
            base(8, color, userCommunicator) { }

        public override string[] GetStringArray()
        {
            return [
                "************",
                "* ******** *",
                "* **    ** *",
                "* ******** *",
                "* **    ** *",
                "* ******** *",
                "************"];
        }
    }
}
