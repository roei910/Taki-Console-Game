using System.Drawing;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class SuperTaki : Card
    {
        private TakiCard? takiInstance;

        public SuperTaki(IUserCommunicator userCommunicator) :
            base(userCommunicator) { }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            Color color = Color.Empty;

            while (!ColorCard.Colors.Contains(color))
                color = playersHolder.CurrentPlayer.ChooseColor();

            takiInstance = new TakiCard(color, _userCommunicator);
            takiInstance.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override bool IsStackableWith(Card other)
        {
            if (takiInstance is null)
                return true;
            return takiInstance.IsStackableWith(other);
        }

        public override string ToString()
        {
            return "SUPER-TAKI";
        }

        public override void PrintCard()
        {
            string[] numberInArray = [
                "**********",
                "*  SUPER  *",
                "*         *",
                "*         *",
                "*         *",
                "*  TAKI   *",
                "**********"];

            _userCommunicator.SendColorMessageToUser(Color.White, string.Join("\n", numberInArray));
        }
    }
}
