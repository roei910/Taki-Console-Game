using MongoDB.Bson;
using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class SwitchCardsWithDirection : CardService
    {
        public SwitchCardsWithDirection(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository,
            IUserCommunicator userCommunicator) : 
            base(discardPileRepository, playersRepository, userCommunicator) { }

        public override List<Card> GenerateCardsForDeck()
        {
            return Enumerable.Range(0, 2)
                .Select(j => new Card(typeof(SwitchCardsWithDirection).ToString(), Color.Empty.ToString())).ToList();
        }

        public override async Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            await UpdatePreviousCard(cardPlayed);

            await SwitchPlayers();

            await _playersRepository.SendMessagesToPlayersAsync(
                player.Name!, "User used switch cards with direction\n", player);

            await base.PlayAsync(player, cardPlayed, cardPlayService);
        }

        public override bool CanStackOtherOnThis(Card topDiscard, Card otherCard, ICardPlayService cardPlayService)
        {//TODO: check why sometimes cant parse object id
            var prevCardId = topDiscard.CardConfigurations["prevCardId"];

            if (prevCardId == null)
                return true;

            if (!ObjectId.TryParse(prevCardId!.ToString(), out ObjectId objectId))
                throw new Exception("couldnt parse the object id!");

            var cardTask = Task.Run(async () =>
            {
                var card = await _discardPileRepository.GetCardByIdAsync(objectId);

                return card;
            });

            topDiscard = cardTask.Result;

            return cardPlayService.CanStack(topDiscard)(otherCard);
        }

        public override async Task FinishPlayAsync(Card cardToReset)
        {
            Card topDiscard = await _discardPileRepository.GetTopDiscardAsync();

            if (topDiscard.Id != cardToReset.Id)
            {
                cardToReset.CardConfigurations["prevCardId"] = null;
                await _discardPileRepository.UpdateCardAsync(cardToReset);
            }
        }
        
        protected async Task UpdatePreviousCard(Card cardToUpdate)
        {
            var cards = await _discardPileRepository.GetCardsOrderedAsync();

            if (cards.Count > 1)
            {
                var previousCard = cards[1];
                cardToUpdate.CardConfigurations["prevCardId"] =
                    previousCard.Type == typeof(SwitchCardsWithDirection).ToString() ?
                        previousCard.CardConfigurations["prevCardId"] : previousCard.Id.ToString();

                await _discardPileRepository.UpdateCardAsync(cardToUpdate);
            }
        }

        private async Task SwitchPlayers()
        {
            var players = await _playersRepository.GetAllAsync();
            players = players.Where(p => p.Cards.Count > 0).ToList();

            List<Card> savedCards = players[0].Cards;
            players[0].Cards = [];

            for (int i = 1; i < players.Count; i++)
                (savedCards, players[i].Cards) = (players[i].Cards, savedCards);

            players[0].Cards = savedCards;

            await _playersRepository.UpdateManyAsync(players);
        }
    }
}
