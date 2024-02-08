using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Communicators;
using Taki.Game.Deck;
using Taki.Game.Handlers;
using Taki.Game.Players;

namespace Taki.Game.GameRules
{
    //TODO: separate drawing cards from player handler to card deck handler
    internal class PlayersHandler
    {
        private readonly LinkedList<Player> _players;
        private readonly Queue<Player> _winners;
        protected bool isDirectionNormal = true;
        public Player CurrentPlayer { get; private set; }

        public PlayersHandler(List<Player> players)
        {
            _players = new(players);
            CurrentPlayer = players.First();
            _winners = new Queue<Player>();
        }

        public bool DrawCards(int numberOfCards, CardsHandler _cardsHandler)
        {
            int cardsDraw = 0;
            Enumerable.Range(0, numberOfCards).ToList()
                .ForEach(x =>
                {
                    CurrentPlayer.AddCard(_cardsHandler.DrawCard());
                    cardsDraw++;
                });
            if (cardsDraw == 0)
                return false;
            //Communicator.PrintMessage($"Player[{CurrentPlayer.Id}]: drew {cardsDraw} card(s)",
            //    Communicator.MessageType.Error);
            return true;
        }

        public void NextPlayer()
        {
            if (isDirectionNormal)
            {
                _players.RemoveFirst();
                _players.AddLast(CurrentPlayer);
                CurrentPlayer = _players.First();
            }
            else
            {
                CurrentPlayer = _players.Last();
                _players.RemoveLast();
                _players.AddFirst(CurrentPlayer);
            }
        }

        //public void ReturnUnhandledCard(Card playerCard)
        //{
        //    CurrentPlayer.AddCard(playerCard);
        //}

        //public Color GetColorFromPlayer()
        //{
        //    Color color = CurrentPlayer.ChooseColor();
        //    MessageHandler.SendMessageToUser($"Player chose color {color}", 
        //        MessageHandler.MessageType.Alert);
        //    return color;
        //}

        //public void SwitchCardsWithDirectionCard(bool isDirectionNormal)
        //{
        //    Player first = CurrentPlayer;
        //    List<Card> savedCards = first.PlayerCards;
        //    first.PlayerCards = [];
        //    NextPlayer(isDirectionNormal);
        //    while (CurrentPlayer.Id != first.Id)
        //    {
        //        (savedCards, CurrentPlayer.PlayerCards) = (CurrentPlayer.PlayerCards, savedCards);
        //        NextPlayer(isDirectionNormal);
        //    }
        //    first.PlayerCards = savedCards;
        //}

        public Player RemoveWinner()
        {
            Player savedPlayer = CurrentPlayer;
            NextPlayer();
            if(!_players.Remove(savedPlayer))
                throw new Exception("error removing the player");
            _winners.Enqueue(savedPlayer);
            return savedPlayer;
        }

        public bool PlayerFinishedHand()
        {
            return CurrentPlayer.IsHandEmpty();
        }

        public int GetNumberOfPlayers()
        {
            return _players.Count;
        }

        public List<Player> GetAllPlayers()
        {
            return _players.ToList();
        }

        public bool CanCurrentPlayerPlay()
        {
            if (_players.Count == 1)
                return false;
            return true;
        }

        internal Card? GetCardFromCurrentPlayer(Func<Card, bool> isSimilarTo)
        {
            return CurrentPlayer.PlayerCards.FirstOrDefault(isSimilarTo);
        }

        internal void AddCardToCurrentPlayer(Card drawCard)
        {
            throw new NotImplementedException();
        }

        internal void CurrentPlayerPlay(GameHandlers gameHandlers)
        {
            Card topDiscard = gameHandlers.GetCardsHandler().GetTopDiscard();
            Card? playerCard = CurrentPlayer.PickCard(topDiscard.IsSimilarTo);

            if(playerCard == null)
            {
                DrawCards(topDiscard.CardsToDraw(), gameHandlers.GetCardsHandler());
                topDiscard.FinishNoPlay();
                NextPlayer();
                return;
            }

            CurrentPlayer.PlayerCards.Remove(playerCard);
            gameHandlers.GetCardsHandler().AddDiscardCard(playerCard);

            topDiscard.FinishPlay();
            playerCard.Play(topDiscard, gameHandlers);
        }

        internal void ChangeDirection()
        {
            isDirectionNormal = !isDirectionNormal;
        }
    }
}
