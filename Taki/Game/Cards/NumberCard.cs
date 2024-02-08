using System.Drawing;
using Taki.Game.GameRules;
using Taki.Game.Handlers;

namespace Taki.Game.Cards
{
    internal class NumberCard : ColorCard
    {
        private readonly int _number;

        public NumberCard(int number, Color color) : base(color)
        {
            _number = number;
        }

        public override bool IsSimilarTo(Card other)
        {
            if(other is not NumberCard card)
                return base.IsSimilarTo(other);
            return base.IsSimilarTo(other) || _number.Equals(card._number);
        }

        public override void Play(Card topDiscard, GameHandlers gameHandlers)
        {
            gameHandlers.GetPlayersHandler().NextPlayer();
        }

        public override string ToString()
        {
            return $"NumberCard: {_number}, {base.ToString()}";
        }
    }
}
