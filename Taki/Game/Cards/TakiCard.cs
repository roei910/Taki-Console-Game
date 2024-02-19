﻿using System.Drawing;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class TakiCard : ColorCard
    {
        public TakiCard(Color color, IUserCommunicator userCommunicator) : 
            base(color, userCommunicator) { }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is TakiCard;
        }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            Player currentPlayer = playersHolder.CurrentPlayer;
            Func<Card, bool> isStackable = card => card is ColorCard && base.IsStackableWith(card);
            Card previous = topDiscard;
            topDiscard = this;
            _userCommunicator.SendAlertMessage("Taki Open!\n");
            _userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");
            topDiscard.PrintCard();

            Card? playerCard = currentPlayer.PickCard(isStackable, elseMessage: "or -1 to finish taki");

            while (playerCard is not null)
            {
                _userCommunicator.SendAlertMessage($"{currentPlayer.Name} chose " +
                    $"{playerCard}");

                previous = topDiscard;
                topDiscard = playerCard;
                currentPlayer.PlayerCards.Remove(playerCard);
                cardDecksHolder.AddDiscardCard(playerCard);
                _userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");
                topDiscard.PrintCard();

                playerCard = currentPlayer.PickCard(isStackable, elseMessage: "or -1 to finish taki");
            }

            _userCommunicator.SendAlertMessage("Taki Closed!\n");

            if (!Equals(topDiscard))
            {
                topDiscard.Play(previous, cardDecksHolder, playersHolder);
                return;
            }

            base.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override void PrintCard()
        {
            string[] numberInArray = [
                "**********",
                "*  TAKI  *",
                "*        *",
                "*        *",
                "*        *",
                "*        *",
                "**********"];

            _userCommunicator.SendColorMessageToUser(_color, string.Join("\n", numberInArray));
        }

        public override string ToString()
        {
            return $"TAKI, {base.ToString()}";
        }
    }
}
