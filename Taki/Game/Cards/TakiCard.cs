using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class TakiCard : ColorCard
    {
        public TakiCard(Color color) : base(color) { }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is TakiCard;
        }

        public override void Play(Card topDiscard, IPlayersHolder playersHolder, IServiceProvider serviceProvider)
        {
            IUserCommunicator userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            ICardDecksHolder cardsHolder = serviceProvider.GetRequiredService<ICardDecksHolder>();

            Player currentPlayer = playersHolder.CurrentPlayer;
            Func<Card, bool> isStackable = card => card is ColorCard && base.IsStackableWith(card);
            Card previous = topDiscard;
            topDiscard = this;
            Card? playerCard = currentPlayer.PickCard(isStackable);

            while (playerCard is not null)
            {
                userCommunicator.SendAlertMessage($"{currentPlayer.GetName()} chose " +
                    $"{playerCard}");

                previous = topDiscard;
                topDiscard = playerCard;
                currentPlayer.PlayerCards.Remove(playerCard);
                cardsHolder.AddDiscardCard(playerCard);
                playerCard = currentPlayer.PickCard(isStackable);
            }

            userCommunicator.SendAlertMessage("Taki Closed!\n");

            if (!Equals(topDiscard))
            {
                topDiscard.Play(previous, playersHolder, serviceProvider);
                return;
            }

            base.Play(topDiscard, playersHolder, serviceProvider);
        }

        public override string ToString()
        {
            return $"TAKI, {base.ToString()}";
        }
    }
}
