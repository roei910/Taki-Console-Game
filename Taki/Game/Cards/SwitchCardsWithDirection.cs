using Taki.Game.Handlers;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class SwitchCardsWithDirection : Card
    {
        private Card? prevCard = null;

        public SwitchCardsWithDirection() { }

        public override bool IsSimilarTo(Card other)
        {
            if(prevCard == null)
                return true;
            return prevCard.IsSimilarTo(other);
        }

        public override void Play(Card topDiscard, GameHandlers gameHandlers)
        {
            prevCard = (topDiscard is SwitchCardsWithDirection card) ? card.prevCard : topDiscard;

            Player first = gameHandlers.GetPlayersHandler().CurrentPlayer;
            List<Card> savedCards = first.PlayerCards;
            first.PlayerCards = [];

            gameHandlers.GetPlayersHandler().NextPlayer();

            while (gameHandlers.GetPlayersHandler().CurrentPlayer.Id != first.Id)
            {
                (savedCards, gameHandlers.GetPlayersHandler().CurrentPlayer.PlayerCards) = (gameHandlers.GetPlayersHandler().CurrentPlayer.PlayerCards, savedCards);
                gameHandlers.GetPlayersHandler().NextPlayer();
            }

            first.PlayerCards = savedCards;
        }

        public override void FinishPlay()
        {
            prevCard = null;
        }

        public override string ToString()
        {
            return $"SwitchCardsWithDirection";
        }
    }
}
