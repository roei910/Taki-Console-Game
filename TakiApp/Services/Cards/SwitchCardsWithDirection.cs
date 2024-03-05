using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class SwitchCardsWithDirection : CardService
    {
        public SwitchCardsWithDirection(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository) : 
            base(discardPileRepository, playersRepository)
        {
        }

        public override List<Card> GenerateCardsForDeck()
        {
            throw new NotImplementedException();
        }

        public override Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            throw new NotImplementedException();
        }


        //Card card1 = new Card(ObjectId.GenerateNewId(), "8 card", Color.Blue.Name, []);

        //JObject jObject = new JObject();
        //jObject["previousId"] = card1.Id.ToString();
        //Card card2 = new Card(ObjectId.GenerateNewId(), "plus2", Color.Red.Name, jObject);

        //var cardsService = serviceProvider.GetRequiredService<IDal<Card>>();
        //await cardsService.CreateOneAsync(card2);
        //var cards = await cardsService.FindAsync();
    }
}
