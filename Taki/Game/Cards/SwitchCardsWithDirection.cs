using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class SwitchCardsWithDirection : Card
    {
        private Card? prevCard = null;

        public SwitchCardsWithDirection() { }

        public override bool IsStackableWith(Card other)
        {
            if (prevCard == null)
                return true;
            return prevCard.IsStackableWith(other);
        }

        public override void Play(Card topDiscard, IPlayersHolder playersHolder, IServiceProvider serviceProvider)
        {
            prevCard = (topDiscard is SwitchCardsWithDirection card) ? card.prevCard : topDiscard;

            var players = playersHolder.Players;
            List<Card> savedCards = players[0].PlayerCards;
            players[0].PlayerCards = [];

            for (int i = 1; i < players.Count; i++)
                (savedCards, players[i].PlayerCards) = (players[i].PlayerCards, savedCards);

            players[0].PlayerCards = savedCards;
            base.Play(topDiscard, playersHolder, serviceProvider);
        }

        public override void FinishPlay()
        {
            prevCard = null;
        }

        public override string ToString()
        {
            if (prevCard == null)
                return "SwitchCards";
            return $"SwitchCards, previous {prevCard}";
        }
    }
}
