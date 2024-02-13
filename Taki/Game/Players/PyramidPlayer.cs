﻿using System.Diagnostics;
using Taki.Game.Handlers;

namespace Taki.Game.Players
{
    internal class PyramidPlayer : Player
    {
        private int _currentNumberOfCards;

        public PyramidPlayer(Player player, int numberOfPlayerCards) : base(player)
        {
            _currentNumberOfCards = numberOfPlayerCards;
        }

        public int CurrentNumberOfCards()
        {
            return _currentNumberOfCards;
        }

        public int GetNextPlayerHand(GameHandlers gameHandlers)
        {
            gameHandlers.GetMessageHandler().SendErrorMessage($"Player[{Id}]: finished hand {_currentNumberOfCards}");
            return --_currentNumberOfCards;
        }

        public void ResetPyramidPlayerCards(int numberOfPlyerCards)
        {
            _currentNumberOfCards = numberOfPlyerCards;
        }

        public override string ToString()
        {
            return $"Pyramid player: current hand is {_currentNumberOfCards}\n" + base.ToString();
        }
    }
}
