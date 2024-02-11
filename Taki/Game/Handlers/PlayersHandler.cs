using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Interfaces;
using Taki.Game.Players;

namespace Taki.Game.GameRules
{
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

        public Player RemoveWinner()
        {
            Player savedPlayer = CurrentPlayer;

            NextPlayer();

            if(!_players.Remove(savedPlayer))
                throw new Exception("error removing the player");

            _winners.Enqueue(savedPlayer);

            return savedPlayer;
        }

        public List<Player> GetAllPlayers()
        {
            return _players.ToList();
        }

        public bool CanCurrentPlayerPlay()
        {
            if (_players.Count == 1)
                return false;
            return !CurrentPlayer.IsHandEmpty();
        }

        public virtual void CurrentPlayerPlay(GameHandlers gameHandlers)
        {
            IMessageHandler messageHandler = gameHandlers.GetMessageHandler();

            messageHandler.SendAlertMessage($"Player[{CurrentPlayer.Id}]" +
                $" ({CurrentPlayer.Name}) is playing, " +
                $"{CurrentPlayer.PlayerCards.Count} cards in hand");

            Card topDiscard = gameHandlers.GetCardsHandler().GetTopDiscard();
            messageHandler.SendAlertMessage($"Top discard: {topDiscard}");

            Card? playerCard = CurrentPlayer.PickCard(topDiscard.IsSimilarTo, 
                gameHandlers);
            messageHandler.SendAlertMessage($"Player picked: {playerCard?.ToString() ?? "no card"}");
            messageHandler.SendMessageToUser();

            if (playerCard == null)
            {
                DrawCards(topDiscard.CardsToDraw(), gameHandlers.GetCardsHandler());
                topDiscard.FinishNoPlay();
                NextPlayer();
                return;
            }

            CurrentPlayer.PlayerCards.Remove(playerCard);
            gameHandlers.GetCardsHandler().AddDiscardCard(playerCard);

            topDiscard.FinishPlay();
            playerCard.Play(gameHandlers);
        }

        public void ChangeDirection()
        {
            isDirectionNormal = !isDirectionNormal;
        }

        public void ResetPlayers(CardsHandler cardsHandler)
        {
            List<Card> cards = [];

            _ = _players.Select(player =>
            {
                cards.AddRange(player.PlayerCards);
                player.PlayerCards.Clear();
                return player;
            }).ToList();

            _ = cards.Select(card =>
            {
                cardsHandler.AddDiscardCard(card);
                return card;
            }).ToList();
        }
    }
}
