﻿using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class NumberCard : ColorCard
    {
        public NumberCard(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository) : 
            base(discardPileRepository, playersRepository) { }

        public override List<Card> GenerateCardsForDeck()
        {
            var cards = Enumerable.Range(3, 7)
                .SelectMany(number => new List<Color>() { Color.Blue, Color.Yellow, Color.Green, Color.Red }
                .Select(color =>
                    new Card($"{typeof(NumberCard)}:{number}", color.ToString()))).ToList();
                
            return cards;
        }
    }
}
