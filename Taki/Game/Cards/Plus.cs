using System.Drawing;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class Plus : ColorCard
    {
        //TODO: CHECK IF WORKING WELL
        public Plus(Color color, IUserCommunicator userCommunicator) : 
            base(color, userCommunicator) { }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is Plus;
        }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder) 
        {
            _userCommunicator.SendAlertMessage("please choose one more card or draw");

            Player currentPlayer = playersHolder.CurrentPlayer;
            Card? playerCard = currentPlayer.PickCard(IsStackableWith);

            if(playerCard == null)
            {
                playersHolder.DrawCards(CardsToDraw(), currentPlayer, cardDecksHolder);
                base.Play(topDiscard, cardDecksHolder, playersHolder);

                return;
            }

            currentPlayer.PlayerCards.Remove(playerCard);
            cardDecksHolder.AddDiscardCard(playerCard);
            base.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override string ToString()
        {
            return $"Plus {base.ToString()}";
        }
    }
}
