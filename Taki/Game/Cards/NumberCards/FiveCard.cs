using System.Drawing;
using Taki.Game.Messages;

namespace Taki.Game.Cards.NumberCards
{
    internal class FiveCard : NumberCard
    {
        public FiveCard(Color color, IUserCommunicator userCommunicator) : 
            base(5, color, userCommunicator) { }

        public override void PrintCard()
        {
            string[] numberInArray = [
                "************",
                "* ******** *",
                "* **       *",
                "* ******** *",
                "*       ** *",
                "* ******** *",
                "************"];

            _userCommunicator.SendColorMessageToUser(_color ,string.Join("\n", numberInArray));
        }
    }
}
