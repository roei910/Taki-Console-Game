using Newtonsoft.Json.Linq;

namespace Taki.Game.Cards.DTOs
{
    internal class SwitchCardsDto : CardDto
    {
        public SwitchCardsDto(CardDto cardDto, Card? prevCard) : 
            base(cardDto)
        {
            if (prevCard is not null)
            {
                //IDictionary<string, object> keyValuePairs = new Dictionary<string, object>
                //{
                //    { "prevCard", prevCard.ToCardDto() }
                //};

                //CardConfigurations.AddRange(keyValuePairs);
                CardConfigurations.Add("prevCard", new JObject(prevCard.ToCardDto()));
            }

            
        }
    }
}
