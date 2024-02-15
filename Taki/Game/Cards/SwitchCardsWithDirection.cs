using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Deck;
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

        public override void Play(Card topDisacrd, IPlayersHolder playersHolder, IServiceProvider serviceProvider)
        {
            ICardDecksHolder cardsHolder = serviceProvider.GetRequiredService<ICardDecksHolder>();
            Card topDiscard = cardsHolder.GetTopDiscard();
            prevCard = (topDiscard is SwitchCardsWithDirection card) ? card.prevCard : topDiscard;

            Player currentPlayer = playersHolder.CurrentPlayer;
            List<Card> savedCards = currentPlayer.PlayerCards;
            currentPlayer.PlayerCards = [];

            playersHolder.NextPlayer();

            //TODO: work on players
            while (playersHolder.CurrentPlayer.Id != currentPlayer.Id)
            {
                (savedCards, playersHolder.CurrentPlayer.PlayerCards) = (playersHolder.CurrentPlayer.PlayerCards, savedCards);
                playersHolder.NextPlayer();
            }

            currentPlayer.PlayerCards = savedCards;
            base.Play(topDiscard, playersHolder, serviceProvider);
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
