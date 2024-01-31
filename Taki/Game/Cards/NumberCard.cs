using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.General;

namespace Taki.Game.Cards
{
    internal class NumberCard : Card
    {
        public NumberCard(string name, Color color) : base(name, color) { }

        public static bool IsNumberCard(Card card)
        {
            return IsCard(card) &&
                int.TryParse(card.Name, out int num) && 
                num <= 9 && num >= 3;
        }
    }
}
