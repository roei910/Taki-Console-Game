using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using Taki.Game.Dto;
using Taki.Game.Interfaces;

namespace Taki.Game.Models.Cards
{
    internal class SwitchCardsWithDirection : Card
    {
        public Card? prevCard = null;

        public SwitchCardsWithDirection(IUserCommunicator userCommunicator) :
            base(userCommunicator)
        { }

        public override string[] GetStringArray()
        {
            return [
                "**********",
                "* SWITCH *",
                "*        *",
                "*        *",
                "*        *",
                "* CARDS  *",
                "**********"];
        }

        public override void PrintCard()
        {
            prevCard?.PrintCard();
            base.PrintCard();
        }

        public override bool IsStackableWith(Card other)
        {
            if (prevCard == null)
                return true;
            return prevCard.IsStackableWith(other);
        }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            _userCommunicator.SendErrorMessage("User used switch cards!\n");

            prevCard = topDiscard is SwitchCardsWithDirection card ? card.prevCard : topDiscard;

            var players = playersHolder.Players;
            List<Card> savedCards = players[0].PlayerCards;
            players[0].PlayerCards = [];

            for (int i = 1; i < players.Count; i++)
                (savedCards, players[i].PlayerCards) = (players[i].PlayerCards, savedCards);

            players[0].PlayerCards = savedCards;

            base.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override void ResetCard()
        {
            prevCard = null;
        }

        public override string ToString()
        {
            if (prevCard == null)
                return "Switch Cards";
            return $"Switch Cards, previous {prevCard}";
        }

        public override CardDto ToCardDto()
        {
            CardDto cardDto = new CardDto(base.ToCardDto());

            if (prevCard is not null)
                cardDto.CardConfigurations["prevCard"] = JsonConvert.SerializeObject(prevCard.ToCardDto());

            return cardDto;
        }

        public override void UpdateFromDto(CardDto cardDTO, ICardDecksHolder cardDecksHolder)
        {
            var prev = cardDTO.CardConfigurations["prevCard"];

            if (prev == null)
                return;

            CardDto? prevCardDto = JsonConvert.DeserializeObject<CardDto>(prev.ToString());

            if (prevCardDto is not null)
                prevCard = cardDecksHolder.GetDiscardPile().GetAllCards()
                    .Where(card => card.Id == prevCardDto.Id).First();
        }
    }
}
