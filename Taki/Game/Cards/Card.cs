using System.Drawing;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal abstract class Card : ICard, IEquatable<Card>
    {
        private static int idsCounter = 0;
        protected readonly IUserCommunicator _userCommunicator;
        public int Id { get; set; } = idsCounter++;

        public Card(IUserCommunicator userCommunicator)
        {
            _userCommunicator = userCommunicator;
        }

        public virtual int CardsToDraw()
        {
            return 1;
        }

        public virtual void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder) 
        {
            playersHolder.NextPlayer();
        }

        public virtual void FinishNoPlay() { }

        public virtual void ResetCard() { }

        public bool Equals(Card? other)
        {
            return Id == other?.Id;
        }

        public virtual void PrintCard()
        {
            string[] numberInArray = GetStringArray();

            _userCommunicator.SendColorMessageToUser(Color.White, string.Join("\n", numberInArray));
        }

        public abstract bool IsStackableWith(Card other);
        
        public abstract string[] GetStringArray();
    }
}
