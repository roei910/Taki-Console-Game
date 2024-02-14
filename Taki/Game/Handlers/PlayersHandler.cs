using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.GameRules
{
    internal class PlayersHandler : IPlayersHandler
    {
        private readonly Queue<Player> _winners;
        private bool _isDirectionNormal = true;
        private int _noPlayCounter = 0;

        protected readonly int _numberOfPlayerCards;

        public readonly LinkedList<Player> _players;

        public Player CurrentPlayer { get; private set; }

        public PlayersHandler(List<Player> players, int numberOfPlayerCards)
        {
            _players = new(players);
            CurrentPlayer = players.First();
            _winners = new Queue<Player>();
            _numberOfPlayerCards = numberOfPlayerCards;
        }

        public bool DrawCards(int numberOfCards, ICardsHandler cardsHandler, IUserCommunicator userCommunicator)
        {
            int cardsDraw = Enumerable.Range(0, numberOfCards).ToList()
                .Count(index =>
                {
                    Card? card = cardsHandler.DrawCard();

                    if(card == null)
                        return false;
                    CurrentPlayer.AddCard(card);

                    return true;
                });

            if (cardsDraw == 0)
                return false;

            userCommunicator.SendErrorMessage(
                $"Player[{CurrentPlayer.Id}]: drew {cardsDraw} card(s)\n");

            return true;
        }

        public void NextPlayer()
        {
            if (_isDirectionNormal)
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

        public bool HasPlayerWon()
        {
            if (_players.Count == 1)
                return false;
            return !CurrentPlayer.IsHandEmpty();
        }

        public virtual void CurrentPlayerPlay(IServiceProvider serviceProvider)
        {
            IUserCommunicator userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            ICardsHandler cardsHandler = serviceProvider.GetRequiredService<ICardsHandler>();

            userCommunicator.SendAlertMessage($"Player[{CurrentPlayer.Id}]" +
                $" ({CurrentPlayer.Name}) is playing, " +
                $"{CurrentPlayer.PlayerCards.Count} cards in hand");

            Card topDiscard = cardsHandler.GetTopDiscard();
            userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");

            Card? playerCard = CurrentPlayer.PickCard(topDiscard.IsStackableWith, 
                this, serviceProvider);
            userCommunicator.SendAlertMessage($"Player picked: {playerCard?.ToString() ?? "no card"}\n");

            if (playerCard == null)
            {
                DrawCards(topDiscard.CardsToDraw(), cardsHandler, userCommunicator);
                topDiscard.FinishNoPlay();
                NextPlayer();
                _noPlayCounter++;

                if (_noPlayCounter >= _players.Count && _noPlayCounter%_players.Count == 0)
                {
                    string message = "Too many rounds without play, consider calling a tie ;)\n" +
                        "press any key to continue";
                    userCommunicator.GetMessageFromUser(message);
                }

                return;
            }

            _noPlayCounter = 0;

            CurrentPlayer.PlayerCards.Remove(playerCard);
            cardsHandler.AddDiscardCard(playerCard);

            topDiscard.FinishPlay();
            playerCard.Play(topDiscard, this, serviceProvider);
        }

        public void ChangeDirection()
        {
            _isDirectionNormal = !_isDirectionNormal;
        }

        public virtual List<Card> GetAllCardsFromPlayers()
        {
            List<Card> cards = [];

            _ = _players.Select(player =>
            {
                cards.AddRange(player.PlayerCards);
                player.PlayerCards.Clear();
                
                return player;
            }).ToList();

            return cards;
        }

        public void DealCards(ICardsHandler cardsHandler)
        {
            Enumerable.Range(0, _numberOfPlayerCards).ToList()
                .Select(i =>
                {
                    _players.Select(p =>
                    {
                        Card? drawCard = cardsHandler.DrawCard();
                        if(drawCard != null)
                            p.AddCard(drawCard);

                        return p;
                    }).ToList();

                    return i;
                }).ToList();

            cardsHandler.DrawFirstCard();
        }

        public Player GetCurrentPlayer()
        {
            return CurrentPlayer;
        }

        public int CountPlayers()
        {
            return _players.Count;
        }
    }
}
