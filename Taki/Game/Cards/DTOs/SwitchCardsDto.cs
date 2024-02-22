using Newtonsoft.Json;

namespace Taki.Game.Cards.DTOs
{
    internal class SwitchCardsDto : CardDto
    {
        //TODO: can be moved in the card
        public SwitchCardsDto(CardDto cardDto, Card? prevCard) : 
            base(cardDto)
        {
            if (prevCard is not null)
                CardConfigurations["prevCard"] = JsonConvert.SerializeObject(prevCard.ToCardDto());
        }
    }
}
