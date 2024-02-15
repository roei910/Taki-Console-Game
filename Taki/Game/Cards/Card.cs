using Taki.Game.Messages;
using Taki.Game.Players;

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

        public abstract bool IsStackableWith(Card other);

        public virtual void Play(Card topDiscard, IPlayersHolder playersHolder, IServiceProvider serviceProvider) 
        {
            playersHolder.NextPlayer();
        }

        public virtual void FinishNoPlay() { }
        public virtual void FinishPlay() { }

        public bool Equals(Card? other)
        {
            return _id == other?._id;
        }
    }
}
