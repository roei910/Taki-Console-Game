namespace Taki.Game.Cards.DTOs
{
    internal class Plus2Dto : CardDto
    {
        //TODO: can be moved in the card
        public Plus2Dto(CardDto cardDto, bool isPlus2Allowed, int countPlus2) :
            base(cardDto)
        {
            CardConfigurations["isPlus2Allowed"] = isPlus2Allowed;
            CardConfigurations["countPlus2"] = countPlus2;
        }
    }
}
