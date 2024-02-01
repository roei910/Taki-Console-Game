using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Deck;

namespace Taki.Game.GameRules
{
    internal class CardDeckHandler(CardDeck cardDeck)
    {
        //we can save all of the important things like color change,
        //previous unique card
        //more
        private readonly CardDeck cardDeck = cardDeck;
    }
}
