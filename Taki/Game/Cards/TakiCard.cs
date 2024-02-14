using System.Drawing;
using Taki.Game.GameRules;
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

        public override void Play(IPlayersHandler playersHandler, ICardsHandler cardsHandler, IUserCommunicator userCommunicator)
        {
            Player currentPlayer = playersHandler.GetCurrentPlayer();
            Card topDiscard = this;
            Card? playerCard = currentPlayer.PickCard(IsStackableWith, playersHandler,
                cardsHandler, userCommunicator);
            
            while (playerCard is not null)
            {
                userCommunicator.SendAlertMessage($"{currentPlayer.GetName()} chose " +
                    $"{playerCard}");

                topDiscard = playerCard;
                currentPlayer.PlayerCards.Remove(playerCard);
                cardsHandler.AddDiscardCard(playerCard);
                playerCard = currentPlayer.PickCard(topDiscard.IsStackableWith, playersHandler,
                    cardsHandler, userCommunicator);
            }

            userCommunicator.SendAlertMessage("Taki Closed!\n");

            if (Equals(topDiscard))
            {
                base.Play(playersHandler, cardsHandler, userCommunicator);
                return;
            }

            cardsHandler.GetTopDiscard().Play(playersHandler, cardsHandler, userCommunicator);
        }

        public override string ToString()
        {
            return $"TAKI, {base.ToString()}";
        }
    }
}
