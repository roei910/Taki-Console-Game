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

        public override void Play(GameHandlers gameHandlers)
        {
            Card topDiscard = gameHandlers.GetCardsHandler().GetTopDiscard();
            prevCard = (topDiscard is SwitchCardsWithDirection card) ? card.prevCard : topDiscard;

            Player currentPlayer = gameHandlers.GetPlayersHandler().CurrentPlayer;
            List<Card> savedCards = currentPlayer.PlayerCards;
            currentPlayer.PlayerCards = [];

            gameHandlers.GetPlayersHandler().NextPlayer();

            while (gameHandlers.GetPlayersHandler().CurrentPlayer.Id != currentPlayer.Id)
            {
                (savedCards, gameHandlers.GetPlayersHandler().CurrentPlayer.PlayerCards) = (gameHandlers.GetPlayersHandler().CurrentPlayer.PlayerCards, savedCards);
                gameHandlers.GetPlayersHandler().NextPlayer();
            }

            currentPlayer.PlayerCards = savedCards;
            base.Play(gameHandlers);
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
