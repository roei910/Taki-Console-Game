using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Text.Json;

namespace Taki.Game.Cards.DTOs
{
    internal class CardDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string CardColor { get; set; }
        //TODO: change to JObject
        public Dictionary<string, JObject> CardConfigurations { get; set; } = [];

        public CardDto(int id, string type)
        {
            Type = type;
            Id = id;
            CardColor = ColorCard.DEFAULT_COLOR.Name;
        }

        public CardDto(int id, Type type) : 
            this(id, type.ToString()) { }

        public CardDto(CardDto cardDto, Color color) :
            this(cardDto.Id, cardDto.Type) 
        {
            CardColor = color.Name;
        }

        public CardDto(CardDto card)
        {
            Type = card.Type;
            Id = card.Id;
            CardColor = card.CardColor;
        }

        //TODO: remove
        public static string SerializeCardDto(CardDto card)
        {
            return JsonSerializer.Serialize(card);
        }

        public static CardDto? DeserializeCardDto(string json)
        {
            return JsonSerializer.Deserialize<CardDto>(json);
        }
    }
}
