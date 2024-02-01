using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.General;

namespace Taki.Game.Cards
{
    enum CardColorsEnum
    {
        Green,
        Red,
        Yellow,
        Blue
    }
    internal abstract class Card(string name, Color color)
    {
        private static int id = 0;
        public int Id { get; } = id++;
        public string Name { get; } = name;
        public Color Color { get; set; } = color;
        public static Dictionary<CardColorsEnum, Color> CardColors =
            new()
        {
            { CardColorsEnum.Green, Color.Green },
            { CardColorsEnum.Red, Color.Red },
            { CardColorsEnum.Yellow, Color.Yellow },
            { CardColorsEnum.Blue, Color.Blue },
        };

        public bool SimilarTo(Card other)
        {
            return Color.IsEmpty || 
                Name.Equals(other.Name) || 
                Color.Equals(other.Color);
        }

        public bool CheckColorMatch(Color other)
        {
            return Color == Color.Empty || 
                Color == Color.Empty ||
                Color.Equals(other);
        }

        public override string ToString()
        {
            if (Color.IsEmpty)
                return Name;
            return $"{Name}({Color})";
        }

        public override bool Equals(object? obj)
        {
            if(obj == null)
                return false;
            if(obj is not Card)
                return false;
            if (obj is not Card card)
                throw new ArgumentException("not card");
            return Name == card.Name && Color == card.Color && Id == card.Id;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Color.GetHashCode();
        }

        public static bool IsCard(Card card)
        {
            if (card == null)
                return false;
            return true;
        }
    }
}
