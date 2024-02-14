﻿using System.Drawing;

namespace Taki.Game.Cards
{
    internal class NumberCard : ColorCard
    {
        private readonly int _number;

        public NumberCard(int number, Color color) : base(color)
        {
            _number = number;
        }

        public override bool IsStackableWith(Card other)
        {
            if(other is not NumberCard card)
                return base.IsStackableWith(other);
            return base.IsStackableWith(other) || _number.Equals(card._number);
        }

        public override string ToString()
        {
            return $"NumberCard: {_number}, {base.ToString()}";
        }
    }
}
