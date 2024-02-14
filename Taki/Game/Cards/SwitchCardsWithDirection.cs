using Taki.Game.Handlers;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class SwitchCardsWithDirection : Card
    {
        private Card? prevCard = null;

        public SwitchCardsWithDirection() { }

        public override bool IsStackableWith(Card other)
        {
            if(prevCard == null)
                return true;
            return prevCard.IsStackableWith(other);
        }

        public override void Play(IPlayersHandler playersHandler, ICardsHandler cardsHandler, IUserCommunicator userCommunicator)
        {
            Card topDiscard = cardsHandler.GetTopDiscard();
            prevCard = (topDiscard is SwitchCardsWithDirection card) ? card.prevCard : topDiscard;

            Player currentPlayer = playersHandler.GetCurrentPlayer();
            List<Card> savedCards = currentPlayer.PlayerCards;
            currentPlayer.PlayerCards = [];

            playersHandler.NextPlayer();

            //TODO: work on players
            while (playersHandler.GetCurrentPlayer().Id != currentPlayer.Id)
            {
                //TODO: recheck
                (savedCards, playersHandler.GetCurrentPlayer().PlayerCards) = (playersHandler.GetCurrentPlayer().PlayerCards, savedCards);
                playersHandler.NextPlayer();
            }

            currentPlayer.PlayerCards = savedCards;
            base.Play(playersHandler, cardsHandler, userCommunicator);
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
