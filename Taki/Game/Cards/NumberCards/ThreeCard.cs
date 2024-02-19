using System.Drawing;
using Taki.Game.Messages;

namespace Taki.Game.Cards.NumberCards
{
    internal class ThreeCard : NumberCard
    {
        public ThreeCard(Color color, IUserCommunicator userCommunicator) : 
            base(3, color, userCommunicator) { }

        public override void PrintCard()
        {
            string[] numberInArray = [
                "************", 
                "* *******  *",
                "*      **  *",
                "*  ******  *",
                "*      **  *",
                "* *******  *",
                "************"];

            _userCommunicator.SendColorMessageToUser(_color, string.Join("\n", numberInArray));
        }
    }
}
