using Newtonsoft.Json.Linq;
using System.Drawing;
using Taki.Game.Cards.DTOs;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    //TODO: need to save the count and override the update from dto
    internal class Plus2 : ColorCard
    {
        private bool _isOnlyPlus2Allowed = false;
        private int _countPlus2 = 0;

        public Plus2(Color color, IUserCommunicator userCommunicator) : 
            base(color, userCommunicator) { }

        public override bool IsStackableWith(Card other)
        {
            if(_isOnlyPlus2Allowed)
                return other is Plus2;
            return base.IsStackableWith(other) || other is Plus2;
        }

        public override int CardsToDraw()
        {
            if (_countPlus2 == 0)
                return base.CardsToDraw();
            return _countPlus2 * 2;
        }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            if (topDiscard is Plus2 card)
            {
                _countPlus2 = card._countPlus2;
                card.FinishNoPlay();
            }

            _isOnlyPlus2Allowed = true;
            _countPlus2++;

            base.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override string ToString()
        {
            return $"Plus2 {base.ToString()}";
        }

        public override void FinishNoPlay()
        {
            _isOnlyPlus2Allowed = false;
            _countPlus2 = 0;
        }

        public override string[] GetStringArray()
        {
            return [
                "******************",
                "*         ****** *",
                "*    |         * *",
                "*  --+--  ****** *",
                "*    |    *      *",
                "*         ****** *",
                "******************"];
        }

        public override CardDto ToCardDto()
        {
            CardDto cardDto = base.ToCardDto();

            return new Plus2Dto(cardDto, _isOnlyPlus2Allowed, _countPlus2);
        }

        public override void UpdateFromDto(CardDto cardDTO, ICardDecksHolder cardDecksHolder)
        {
            base.UpdateFromDto(cardDTO, cardDecksHolder);

            object? isPlus2Allowed = cardDTO.CardConfigurations["isPlus2Allowed"];
            object? countPlus2 = cardDTO.CardConfigurations["countPlus2"];

            _isOnlyPlus2Allowed = isPlus2Allowed is bool ? (bool)isPlus2Allowed : false;
            _countPlus2 = (int)(countPlus2 is int ? countPlus2 : 0);

        }
    }
}
