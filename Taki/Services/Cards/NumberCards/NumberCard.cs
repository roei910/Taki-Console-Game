﻿using System.Drawing;
using Taki.Shared.Abstract;
using Taki.Shared.Interfaces;

namespace Taki.Models.Cards.NumberCards
{
    public abstract class NumberCard : ColorCard
    {
        private readonly int _number;

        public NumberCard(int number, Color color, IUserCommunicator userCommunicator) :
            base(color, userCommunicator)
        {
            _number = number;
        }

        public override bool IsStackableWith(Card other)
        {
            if (other is not NumberCard card)
                return base.IsStackableWith(other);
            return base.IsStackableWith(other) || _number.Equals(card._number);
        }

        public override string ToString()
        {
            return $"NumberCard: {_number}, {base.ToString()}";
        }
    }
}
