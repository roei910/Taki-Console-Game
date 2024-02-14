﻿using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Algorithm
{
    internal class PlayerHateTakiAlgo : PlayerAlgorithm
    {
        bool IsTaki = false;

        public override Card? ChooseCard(Func<Card, bool> isSimilarTo,
            Player player, IPlayersHandler playersHandler, ICardsHandler cardsHandler, IUserCommunicator userCommunicator)
        {
            if (IsTaki)
            {
                IsTaki = false;
                return null;
            }

            Card? playerCard = base.ChooseCard(isSimilarTo, player, playersHandler, cardsHandler, userCommunicator);
            if (playerCard is TakiCard)
                IsTaki = true;

            return playerCard;
        }

        public override string ToString()
        {
            return "Player Hate Taki Algo";
        }
    }
}
