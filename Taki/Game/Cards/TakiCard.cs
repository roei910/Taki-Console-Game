using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using Taki.Game.Handlers;
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

        public override void Play(Card topDiscard, IPlayersHandler playersHandler, IServiceProvider serviceProvider)
        {
            Player currentPlayer = playersHandler.GetCurrentPlayer();
            Card? playerCard = currentPlayer.PickCard(IsStackableWith);
            IUserCommunicator userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            ICardsHandler cardsHandler = serviceProvider.GetRequiredService<ICardsHandler>();
            
            while (playerCard is not null)
            {
                userCommunicator.SendAlertMessage($"{currentPlayer.GetName()} chose " +
                    $"{playerCard}");

                topDiscard = playerCard;
                currentPlayer.PlayerCards.Remove(playerCard);
                cardsHandler.AddDiscardCard(playerCard);
                playerCard = currentPlayer.PickCard(topDiscard.IsStackableWith);
            }

            userCommunicator.SendAlertMessage("Taki Closed!\n");

            if (Equals(topDiscard))
            {
                base.Play(topDiscard, playersHandler, serviceProvider);
                return;
            }

            cardsHandler.GetTopDiscard().Play(topDiscard, playersHandler, serviceProvider);
        }

        public override string ToString()
        {
            return $"TAKI, {base.ToString()}";
        }
    }
}
