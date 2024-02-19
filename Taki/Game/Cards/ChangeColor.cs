using System.Drawing;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class ChangeColor : Card
    {
        static readonly Color DEFAULT_COLOR = Color.Empty;
        Color color = DEFAULT_COLOR;

        public ChangeColor(IUserCommunicator userCommunicator) : 
            base(userCommunicator) { }

        public override bool IsStackableWith(Card other)
        {
            if (color == DEFAULT_COLOR)
                return true;
            return other.IsStackableWith(new NumberCard(0, color, _userCommunicator));
        }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            while (!ColorCard.Colors.Contains(color))
                color = playersHolder.CurrentPlayer.ChooseColor();
            
            base.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override void ResetCard()
        {
            color = DEFAULT_COLOR;
        }

        public override string ToString()
        {
            return $"ChangeColor {color}";
        }
    }
}
