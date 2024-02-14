using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Deck;
using Taki.Game.Handlers;
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

        public override void Play(Card topDisacrd, IPlayersHandler playersHandler, IServiceProvider serviceProvider)
        {
            ICardDecksHolder cardsHandler = serviceProvider.GetRequiredService<ICardDecksHolder>();
            Card topDiscard = cardsHandler.GetTopDiscard();
            prevCard = (topDiscard is SwitchCardsWithDirection card) ? card.prevCard : topDiscard;

            Player currentPlayer = playersHandler.GetCurrentPlayer();
            List<Card> savedCards = currentPlayer.PlayerCards;
            currentPlayer.PlayerCards = [];

            playersHandler.NextPlayer();

            //TODO: work on players
            while (playersHandler.GetCurrentPlayer().Id != currentPlayer.Id)
            {
                (savedCards, playersHandler.GetCurrentPlayer().PlayerCards) = (playersHandler.GetCurrentPlayer().PlayerCards, savedCards);
                playersHandler.NextPlayer();
            }

            currentPlayer.PlayerCards = savedCards;
            base.Play(topDiscard, playersHandler, serviceProvider);
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
