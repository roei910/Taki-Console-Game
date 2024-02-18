using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class Plus : ColorCard
    {
        public Plus(Color color) : base(color) { }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is Plus;
        }

        public override void Play(Card topDiscard, IPlayersHolder playersHolder, 
            IServiceProvider serviceProvider) 
        {
            IUserCommunicator userCommunicator = serviceProvider
                .GetRequiredService<IUserCommunicator>();
            ICardDecksHolder cardDecksHolder = serviceProvider
                .GetRequiredService<ICardDecksHolder>();

            userCommunicator.SendAlertMessage("please choose one more card or draw");

            Player currentPlayer = playersHolder.CurrentPlayer;
            Card? playerCard = currentPlayer.PickCard(IsStackableWith);

            if(playerCard == null)
            {
                playersHolder.DrawCards(CardsToDraw(), currentPlayer);

                return;
            }

            currentPlayer.PlayerCards.Remove(playerCard);
            cardDecksHolder.AddDiscardCard(playerCard);
        }

        public override string ToString()
        {
            return $"Plus {base.ToString()}";
        }
    }
}
