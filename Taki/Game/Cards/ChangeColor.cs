using System.Drawing;
using Taki.Game.Cards.NumberCards;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class ChangeColor : Card
    {
        private static readonly Color DEFAULT_COLOR = Color.Empty;
        private Color _color = DEFAULT_COLOR;

        public ChangeColor(IUserCommunicator userCommunicator) : 
            base(userCommunicator) { }

        public override bool IsStackableWith(Card other)
        {
            if (_color == DEFAULT_COLOR)
                return true;
            return other.IsStackableWith(new NoNumberCard(_color, _userCommunicator));
        }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            while (!ColorCard.Colors.Contains(_color))
                _color = playersHolder.CurrentPlayer.ChooseColor();
            
            base.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override void PrintCard()
        {
            string[] numberInArray = [
                "**********",
                "* CHANGE *",
                "*        *",
                "*        *",
                "*        *",
                "* COLOR  *",
                "**********"];

            _userCommunicator.SendColorMessageToUser(_color, string.Join("\n", numberInArray));
        }

        public override void ResetCard()
        {
            _color = DEFAULT_COLOR;
        }

        public override string ToString()
        {
            return $"ChangeColor {_color}";
        }
    }
}
