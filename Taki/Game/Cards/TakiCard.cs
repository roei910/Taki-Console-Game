using System.Drawing;
using Taki.Game.GameRules;
using Taki.Game.Handlers;
using Taki.Game.Interfaces;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class TakiCard : ColorCard
    {
        public TakiCard(Color color) : base(color) { }

        public override bool IsSimilarTo(Card other)
        {
            return base.IsSimilarTo(other) || other is TakiCard;
        }

        public override void Play(GameHandlers gameHandlers)
        {
            CardsHandler cardsHandler = gameHandlers.GetCardsHandler();
            PlayersHandler playersHandler = gameHandlers.GetPlayersHandler();
            IMessageHandler messageHandler = gameHandlers.GetMessageHandler();
            Player currentPlayer = playersHandler.CurrentPlayer;
            Card topDiscard = this;
            Card? playerCard = currentPlayer.PickCard(IsSimilarTo, gameHandlers);
            
            while (playerCard is not null)
            {
                messageHandler.SendAlertMessage($"{currentPlayer.GetName()} chose " +
                    $"{playerCard}");

                topDiscard = playerCard;
                currentPlayer.PlayerCards.Remove(playerCard);
                cardsHandler.AddDiscardCard(playerCard);
                playerCard = currentPlayer.PickCard(topDiscard.IsSimilarTo, gameHandlers);
            }

            if (Equals(topDiscard))
            {
                playersHandler.NextPlayer();
                return;
            }

            messageHandler.SendAlertMessage("Taki Closed!");
            cardsHandler.GetTopDiscard().Play(gameHandlers);

        }

        public override string ToString()
        {
            return $"TAKI, {base.ToString()}";
        }
    }
}
