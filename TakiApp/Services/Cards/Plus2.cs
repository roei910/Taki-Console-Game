using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class Plus2 : ColorCard
    {
        public Plus2(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository) : 
            base(discardPileRepository, playersRepository) { }

        //TODO: check if prev was plus 2
        public override List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(Plus2).ToString(), color.ToString())).ToList();

            return cards;
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
