using System.Drawing;
using Taki.Shared.Interfaces;
using Taki.Shared.Models.Dto;

namespace Taki.Shared.Abstract
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

        public virtual CardDto ToCardDto()
        {
            return new CardDto(Id, GetType());
        }

        public virtual void UpdateFromDto(CardDto cardDTO, ICardDecksHolder cardDecksHolder) { }

        public abstract bool IsStackableWith(Card other);

        public abstract string[] GetStringArray();
    }
}
