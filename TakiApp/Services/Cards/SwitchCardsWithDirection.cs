﻿using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class SwitchCardsWithDirection : CardService
    {
        public SwitchCardsWithDirection(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository) : 
            base(discardPileRepository, playersRepository)
        {
        }

        public override List<Card> GenerateCardsForDeck()
        {
            throw new NotImplementedException();
        }

        public override Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            throw new NotImplementedException();
        }
    }
}
