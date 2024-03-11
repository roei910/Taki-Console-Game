using Newtonsoft.Json.Linq;
using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class Plus2 : ColorCard
    {
        public Plus2(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository,
            IUserCommunicator userCommunicator) : 
            base(discardPileRepository, playersRepository, userCommunicator) { }

        public override List<Card> GenerateCardsForDeck()
        {
            var cardConfigurations = new JObject();
            cardConfigurations["isOnlyPlus2Allowed"] = false;
            cardConfigurations["countPlus2"] = 0;

            var cards = new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color => new Card(typeof(Plus2).ToString(), color.ToString()) 
                { 
                    CardConfigurations = cardConfigurations 
                }).ToList();

            return cards;
        }

        public override int CardsToDraw(Card cardPlayed)
        {
            var countPlus2 = (int)cardPlayed.CardConfigurations["countPlus2"]!;

            if (countPlus2 == 0)
                return base.CardsToDraw(cardPlayed);

            return countPlus2 * 2;
        }

        public override bool CanStackOtherOnThis(Card topDiscard, Card otherCard, ICardPlayService cardPlayService)
        {
            var isOnlyPlus2Allowed = (bool)topDiscard.CardConfigurations["isOnlyPlus2Allowed"]!;

            if (isOnlyPlus2Allowed)
            {
                var ans = topDiscard.Type == otherCard.Type;

                _userCommunicator.SendErrorMessage("Only plus 2 cards are allowed!");

                return ans;
            }


            return base.CanStackOtherOnThis(topDiscard, otherCard, cardPlayService);
        }

        public override async Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            var cards = await _discardPileRepository.GetCardsOrderedAsync();
            Card? previousCard = cards.Count > 1 ? cards[1] : null;

            if (previousCard is not null && previousCard.Type == cardPlayed.Type)
            {
                cardPlayed.CardConfigurations["countPlus2"] = (int)previousCard.CardConfigurations["countPlus2"]!;
                previousCard.CardConfigurations["countPlus2"] = 0;
                previousCard.CardConfigurations["isOnlyPlus2Allowed"] = false;
                await _discardPileRepository.UpdateCardAsync(previousCard);
            }

            cardPlayed.CardConfigurations["countPlus2"] = (int)cardPlayed.CardConfigurations["countPlus2"]! + 1;
            cardPlayed.CardConfigurations["isOnlyPlus2Allowed"] = true;

            await _discardPileRepository.UpdateCardAsync(cardPlayed);

            await base.PlayAsync(player, cardPlayed, cardPlayService);
        }

        public override async Task FinishNoPlay(Card cardPlayed)
        {
            cardPlayed.CardConfigurations["isOnlyPlus2Allowed"] = false;
            cardPlayed.CardConfigurations["countPlus2"] = 0;

            await _discardPileRepository.UpdateCardAsync(cardPlayed);

            await base.FinishNoPlay(cardPlayed);
        }
    }
}
