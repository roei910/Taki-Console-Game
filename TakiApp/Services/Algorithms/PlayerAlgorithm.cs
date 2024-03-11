﻿using System.Drawing;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace Taki.Models.Algorithm
{
    public class PlayerAlgorithm : IPlayerAlgorithm
    {
        public Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, string? elseMessage = null)
        {
            if (playerCards.Count == 0)
                return null;

            Task.Delay(5000).Wait();

            Card? playerCard = playerCards.FirstOrDefault(card => isSimilarTo(card!)); 

            return playerCard;
        }

        public Color ChooseColor(List<Card> playerCards)
        {
            var colors = playerCards
                .Select(card => Color.FromName(card.CardColor))
                .Where(color => color.Name != Color.Empty.Name)
                .GroupBy(c => c);

            if (colors.Count() == 0)
                return Color.Blue;

            return colors.OrderByDescending(color => color.Count()).First().FirstOrDefault(Color.Blue);
        }

        public Player ChoosePlayer(List<Player> players)
        {
            return players.OrderBy(val => Guid.NewGuid().ToString()).First();
        }
    }
}
