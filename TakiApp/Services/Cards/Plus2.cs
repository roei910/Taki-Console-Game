using System.Drawing;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    internal class Plus2 : ICardService
    {
        public List<Card> GenerateCardsForDeck()
        {
            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(Plus2).ToString(), color.Name)).ToList();

            return cards;
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
