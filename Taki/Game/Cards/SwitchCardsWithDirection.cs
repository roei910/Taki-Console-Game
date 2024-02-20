using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class SwitchCardsWithDirection : Card
    {
        protected Card? prevCard = null;
        
        public SwitchCardsWithDirection(IUserCommunicator userCommunicator) : 
            base(userCommunicator) { }

        public override string[] GetStringArray()
        {
            return [
                "**********",
                "* SWITCH *",
                "*        *",
                "*        *",
                "*        *",
                "* CARDS  *",
                "**********"];
        }

        public override void PrintCard()
        {
            prevCard?.PrintCard();
            base.PrintCard();
        }

        public override bool IsStackableWith(Card other)
        {
            if (prevCard == null)
                return true;
            return prevCard.IsStackableWith(other);
        }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            _userCommunicator.SendErrorMessage("User used switch cards!\n");

            prevCard = (topDiscard is SwitchCardsWithDirection card) ? card.prevCard : topDiscard;

            var players = playersHolder.Players;
            List<Card> savedCards = players[0].PlayerCards;
            players[0].PlayerCards = [];

            for (int i = 1; i < players.Count; i++)
                (savedCards, players[i].PlayerCards) = (players[i].PlayerCards, savedCards);

            players[0].PlayerCards = savedCards;

            base.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override void ResetCard()
        {
            prevCard = null;
        }

        public override string ToString()
        {
            if (prevCard == null)
                return "Switch Cards";
            return $"Switch Cards, previous {prevCard}";
        }
    }
}
