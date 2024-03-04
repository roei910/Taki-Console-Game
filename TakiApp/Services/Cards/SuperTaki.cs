﻿using System.Drawing;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class SuperTaki : TakiCard
    {
        public override List<Card> GenerateCardsForDeck()
        {
            return Enumerable.Range(0, 2)
                .Select(j => new Card(typeof(SuperTaki).ToString(), Color.Empty.Name)).ToList();
        }
    }
}
