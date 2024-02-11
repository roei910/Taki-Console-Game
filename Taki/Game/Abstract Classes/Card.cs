using Taki.Game.Handlers;
using Taki.Game.Interfaces;

namespace Taki.Game.Cards
{

    internal abstract class Card : ICard, IEquatable<Card>
    {
        private static int idsCounter = 0;
        private readonly int _id;

        public Card()
        {
            _id = idsCounter++;
        }

        public virtual int CardsToDraw()
        {
            return 1;
        }

        public abstract bool IsSimilarTo(Card other);
        public virtual void Play(GameHandlers gameHandlers) { }
        public virtual void FinishNoPlay() { }
        public virtual void FinishPlay() { }

        public bool Equals(Card? other)
        {
            return _id == other?._id;
        }
    }
}
