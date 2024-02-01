using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Algorithm;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.GameRules;
using Taki.Game.General;
using Taki.Game.Players;

namespace Taki.Game.Managers
{
    //TODO: less spagetti
    //TODO: split manager
    //TODO: if i  want to add another type of card dont add in game manager.
    //TODO: add rule class
    //TODO: less function calls
    //TODO: more classes




    //game manager
        //players
        //card deck
        //rules
    internal class OldGameManager
    {
        private const int NUMBER_OF_TOTAL_WINNERS = 2;
        private readonly List<IPlayerAlgorithm> algorithms =
        [
            //new PlayerAlgorithm(),
            new ManualPlayerAlgorithm()
        ];
        readonly LinkedList<Player> players;
        private readonly CardDeck cardDeck;
        bool isDirectionNormal;

        public OldGameManager(int numberOfPlayers, int numberOfPlayerCards)
        {
            players = new LinkedList<Player>();
            cardDeck = CardDeckFactory.GenerateCardDeck();
            isDirectionNormal = true;
            CreatePlayers(numberOfPlayers);
            DealCards(numberOfPlayerCards);
            cardDeck.DrawFirstCard();
        }

        private void CreatePlayers(int numberOfPlayers)
        {
            Random random = new ();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                int index = random.Next(algorithms.Count);
                players.AddLast(new Player(i, algorithms.ElementAt(index)));
            }
        }

        private void DealCards(int numberOfPlayerCards)
        {
            for (int j = 0; j < numberOfPlayerCards; j++)
                foreach (Player player in players)
                    player.AddCard(cardDeck.DrawCard());
        }

        public void StartGame()
        {
            int[] winnerIds = new int[NUMBER_OF_TOTAL_WINNERS];
            for (int i = 0; i < winnerIds.Length; i++)
            {
                winnerIds[i] = GetWinnerById();
                players.Remove(players.ElementAt(winnerIds[i]));
            }
            PrintWinnersList(winnerIds);
        }

        private void NextPlayer()
        {
            Player currentPlayer = players.First();
            if (isDirectionNormal)
            {
                players.RemoveFirst();
                players.AddLast(currentPlayer);
            }
            else
            {
                players.RemoveLast();
                players.AddFirst(currentPlayer);
            }
        }

        private int GetWinnerById()
        {
            while (!players.First().IsHandEmpty())
            {
                Console.WriteLine("------------------------");
                PlayerPlay();
                NextPlayer();
            }
            return players.First().Id;
        }

        private void PlayerPlay()
        {
            Card topDiscard = cardDeck.GetTopDiscardPile();
            if (!players.First().AskPlayerToPickCard(topDiscard, out Card userCard))
                PlayerDrawCard();
            else if (!TryHandleCard(userCard))
                PlayerPlay();
        }

        private bool CheckCardIsTopDiscard(Card card)
        {
            //player would return the top of discard if he chooses not to draw a card to discard pile.
            return card.Equals(cardDeck.GetTopDiscardPile());
        }

        private bool TryHandleCard(Card card)
        {
            if (!TryAddCardToDiscardPile(card))
                return false;
            if (UniqueCard.IsUniqueCard(card))
                HandleUniqueCard(card);
            return true;
        }

        private bool TryAddCardToDiscardPile(Card card)
        {
            //TODO: ??? fix behavior where user takes out a card from hand when giving one
            //if (!cardDeck.CanPlayCard(card))
            //{
            //    players.First().AddCard(card);
            //    Utilities.PrintConsoleError("Please follow the card stacking rules");
            //    return false;
            //}
            cardDeck.AddCardToDiscardPile(card);
            return true;
        }

        private void HandleUniqueCard(Card card)
        {
            if (UniqueCard.IsPlus(card))
                PlayerPlay();
            else if (UniqueCard.IsPlus2(card))
                HandlePlus2Card();
            else if (UniqueCard.IsTaki(card))
                HandleTakiCard();
            else if (UniqueCard.IsSuperTaki(card))
                HandleSuperTakiCard();
            else if (UniqueCard.IsStop(card))
                NextPlayer();
            else if (UniqueCard.IsChangeColor(card))
                HandleChangeColorCard(Card.IsCard);
            else if (UniqueCard.IsChangeDirection(card))
                isDirectionNormal = !isDirectionNormal;
            else if (UniqueCard.IsSwitchCardsWithDirection(card))
                HandleSwitchCardsWithDirectionCard();
            else
                throw new Exception("something unexpected happened");
        }

        private Card GetCardFromPlayer()
        {
            players.First().AskPlayerToPickCard(cardDeck.GetTopDiscardPile(), out Card card);
            return card;
        }

        private void PlayerDrawCard()
        {
            players.First().AddCard(cardDeck.DrawCard());
        }

        private void HandlePlus2Card()
        {
            //TODO: plus 2 * number of plus 2 cards
            NextPlayer();
            Console.WriteLine("Please choose a plus2 card or draw 2 cards from deck");
            Card card = GetCardFromPlayer();
            if (UniqueCard.IsPlus2(card))
                HandlePlus2Card();
            else
                Enumerable.Range(0, 2).ToList().ForEach(_ => PlayerDrawCard());
        }

        private void HandleTakiCard()
        {
            //TODO: check how to add the last card as player next handle card
            Card card = GetCardFromPlayer();
            while (TryAddCardToDiscardPile(card))
            {
                card = GetCardFromPlayer();
                if (CheckCardIsTopDiscard(card))
                {
                    Console.WriteLine("Taki closed!");
                    return;
                }
            }
        }

        private void HandleSuperTakiCard()
        {
            HandleChangeColorCard(UniqueCard.IsTaki, skipCurrentPlayer: false);
            HandleTakiCard();
        }

        private void HandleChangeColorCard(Func<Card, bool> checkCardFunction, bool skipCurrentPlayer = true)
        {
            Color color = Utilities.GetColorFromUserEnum<CardColorsEnum>("of color");
            if (skipCurrentPlayer)
                NextPlayer();
            GetColorCardFromUser(checkCardFunction, color);
        }

        private void GetColorCardFromUser(Func<Card, bool> checkCardFunction, Color color)
        {
            Utilities.PrintConsoleAlert($"Please choose a card with the new color: {color}");
            Card playerCard = GetCardFromPlayer();
            if(CheckCardIsTopDiscard(playerCard))
            {
                PlayerDrawCard();
                NextPlayer();
                GetColorCardFromUser(checkCardFunction, color);
                return;
            }
            if(!(checkCardFunction(playerCard) && playerCard.CheckColorMatch(color)))
            {
                Utilities.PrintConsoleError("Wrong color, try again");
                GetColorCardFromUser(checkCardFunction, color);
                return;
            }
            cardDeck.AddCardToDiscardPile(playerCard);
        }

        private void HandleSwitchCardsWithDirectionCard()
        {
            Player first = players.First();
            List<Card> savedCards = first.PlayerCards;
            first.PlayerCards = [];
            NextPlayer();
            while (!players.First().Equals(first))
            {
                (savedCards, players.First().PlayerCards) = (players.First().PlayerCards, savedCards);
                NextPlayer();
            }
            first.PlayerCards = savedCards;
        }

        private static void PrintWinnersList(int[] winnerIds)
        {
            Console.WriteLine("The winners by order:");
            for (int i = 0; i < winnerIds.Length; i++)
            {
                Console.WriteLine($"{i + 1}. id {winnerIds[i]}");
            }
        }
    }
}
