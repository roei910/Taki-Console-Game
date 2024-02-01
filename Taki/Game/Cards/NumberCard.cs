using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.General;

namespace Taki.Game.Cards
{
    internal class NumberCard(string name, Color color) : Card(name, color)
    {
        private const int MIN_NUMBER_CARD = 3;
        private const int MAX_NUMBER_CARD = 3;

        public static bool IsNumberCard(Card card)
        {
            return IsCard(card) &&
                int.TryParse(card.Name, out int num) && 
                num <= MAX_NUMBER_CARD && num >= MIN_NUMBER_CARD;
        }
    }
}
