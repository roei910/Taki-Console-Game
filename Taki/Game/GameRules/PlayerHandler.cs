using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.General;
using Taki.Game.Players;

namespace Taki.Game.GameRules
{
    internal class PlayerHandler(LinkedList<Player> players)
    {
        private readonly LinkedList<Player> players = players;
        public Player CurrentPlayer { get; private set; } = players.First();

        public bool DrawCards(int numberOfCards, CardDeck cardDeck)
        {
            int cardsDraw = 0;
            Enumerable.Range(0, numberOfCards).ToList()
                .ForEach(x =>
                {
                    if(!cardDeck.TryDrawCard(out Card ?card))
                        return;
                    if (card == null)
                        throw new NullReferenceException("card is null");
                    CurrentPlayer.AddCard(card);
                    cardsDraw++;
                });
            if(cardsDraw == 0)
                return false;
            Utilities.PrintConsoleError($"Player[{CurrentPlayer.Id}]: drew {cardsDraw} card(s)");
            return true;
        }

        public void NextPlayer(bool isDirectionNormal)
        {
            if (isDirectionNormal)
            {
                players.RemoveFirst();
                players.AddLast(CurrentPlayer);
                CurrentPlayer = players.First();
            }
            else
            {
                CurrentPlayer = players.Last();
                players.RemoveLast();
                players.AddFirst(CurrentPlayer);
            }
        }

        public void ReturnUnhandledCard(Card playerCard)
        {
            CurrentPlayer.AddCard(playerCard);
        }

        public Color GetColorFromPlayer()
        {
            Color color = CurrentPlayer.ChooseColor();
            Utilities.PrintConsoleAlert($"Player chose color {color}");
            return color;
        }

        public void SwitchCardsWithDirectionCard(bool isDirectionNormal)
        {
            Player first = CurrentPlayer;
            List<Card> savedCards = first.PlayerCards;
            first.PlayerCards = [];
            NextPlayer(isDirectionNormal);
            while (CurrentPlayer.Id != first.Id)
            {
                (savedCards, CurrentPlayer.PlayerCards) = (CurrentPlayer.PlayerCards, savedCards);
                NextPlayer(isDirectionNormal);
            }
            first.PlayerCards = savedCards;
        }

        public Player RemoveWinner(bool isDirectionNormal)
        {
            Player savedPlayer = CurrentPlayer;
            NextPlayer(isDirectionNormal);
            if(!players.Remove(savedPlayer))
                throw new Exception("error removing the player");
            return savedPlayer;
        }

        public bool PlayerFinishedHand()
        {
            return CurrentPlayer.IsHandEmpty();
        }

        public int NumberOfPlayersLeft()
        {
            return players.Count;
        }
    }
}
