using System.Drawing;
using Taki.Game.Messages;

namespace Taki.Game.Cards
{
    enum CardColorsEnum
    {
        Green,
        Red,
        Yellow,
        Blue
    }

    internal abstract class ColorCard : Card
    {
        protected static readonly Color DEFAULT_COLOR = Color.Empty;
        protected Color _color;

        public static List<Color> Colors = [Color.Green, Color.Red, Color.Yellow, Color.Blue];

        public ColorCard(Color color, IUserCommunicator userCommunicator) :
            base(userCommunicator)
        {
            _color = color;
        }

        public Color GetColor()
        { 
            return _color; 
        }

        public override bool IsStackableWith(Card other)
        {
            if (other is not ColorCard card)
                return true;
            if (card._color.Equals(DEFAULT_COLOR))
                return true;
            return _color.Equals(card._color);
        }

        public override string ToString()
        {
            return _color.ToString();
        }

        public override void PrintCard()
        {
            string[] numberInArray = GetStringArray();

            _userCommunicator.SendColorMessageToUser(_color, string.Join("\n", numberInArray));
        }
    }
}
