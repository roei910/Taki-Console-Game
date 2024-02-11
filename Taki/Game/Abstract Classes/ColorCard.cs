using System.Drawing;

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
        private readonly Color _color;

        public static List<Color> Colors = [Color.Green, Color.Red, Color.Yellow, Color.Blue];

        public ColorCard(Color color) : base()
        {
            _color = color;
        }

        public Color GetColor()
        { 
            return _color; 
        }

        public override bool IsSimilarTo(Card other)
        {
            if (other is not ColorCard card)
                return true;
            return _color.Equals(card._color);
        }

        public override string ToString()
        {
            return _color.ToString();
        }
    }
}
