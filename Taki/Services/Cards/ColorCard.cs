using System.Drawing;
using Taki.Shared.Abstract;
using Taki.Shared.Interfaces;
using Taki.Shared.Models.Dto;

namespace Taki.Models.Cards
{
    public abstract class ColorCard : Card
    {
        public static readonly Color DEFAULT_COLOR = Color.Empty;
        protected Color _color;
        public static List<Color> Colors = [Color.Green, Color.Red, Color.Yellow, Color.Blue];

        public ColorCard(Color color, IUserCommunicator userCommunicator) :
            base(userCommunicator)
        {
            _color = color;
        }

        public Color GetColor()
        {
            return _color;
        }

        public override bool IsStackableWith(Card other)
        {
            if (other is not ColorCard card)
                return true;
            if (card._color.Equals(DEFAULT_COLOR))
                return true;
            return _color.Equals(card._color);
        }

        public override string ToString()
        {
            return _color.ToString();
        }

        public override void PrintCard()
        {
            string[] numberInArray = GetStringArray();

            _userCommunicator.SendColorMessageToUser(_color, string.Join("\n", numberInArray));
        }

        public override CardDto ToCardDto()
        {
            CardDto cardDto = base.ToCardDto();
            return new CardDto(cardDto, _color);
        }

        public override void UpdateFromDto(CardDto cardDTO, ICardDecksHolder cardDecksHolder)
        {
            _color = Color.FromName(cardDTO.CardColor);
        }
    }
}
