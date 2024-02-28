using Newtonsoft.Json.Linq;
using System.Drawing;
using Taki.Models.Cards;

namespace Taki.Shared.Models.Dto
{
    internal class CardDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string CardColor { get; set; }
        public JObject CardConfigurations { get; set; } = [];

        [Newtonsoft.Json.JsonConstructor]
        public CardDto(int id, string type, string cardColor, JObject cardConfigurations)
        {
            Type = type;
            Id = id;
            CardColor = cardColor;
            CardConfigurations = cardConfigurations;
        }

        public CardDto(int id, string type)
        {
            Type = type;
            Id = id;
            CardColor = ColorCard.DEFAULT_COLOR.Name;
        }

        public CardDto(CardDto cardDto, Color color) :
            this(cardDto.Id, cardDto.Type)
        {
            CardColor = color.Name;
        }
    }
}
