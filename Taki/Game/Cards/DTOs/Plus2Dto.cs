using Newtonsoft.Json.Linq;

namespace Taki.Game.Cards.DTOs
{
    internal class Plus2Dto : CardDto
    {
        public Plus2Dto(CardDto cardDto, bool isPlus2Allowed, int countPlus2) :
            base(cardDto)
        {
            //IDictionary<string, object> keyValuePairs = new Dictionary<string, object>
            //{
            //    { "isPlus2Allowed", isPlus2Allowed },
            //    { "countPlus2", countPlus2 }
            //};
            //CardConfigurations.Add(keyValuePairs);
            CardConfigurations.Add("isPlus2Allowed", new JObject(isPlus2Allowed));
            CardConfigurations.Add("countPlus2", new JObject(countPlus2));
        }
    }
}
