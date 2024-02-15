using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.Messages;

namespace Taki.Game.Players
{
    //TODO: naming => players holder? or change to move maker or something similar to only make player moves
    internal class PlayersHolder : IPlayersHolder
    {
        private readonly Queue<Player> _winners;
        private bool _isDirectionNormal = true;
        private int _noPlayCounter = 0;
        protected readonly int _numberOfPlayerCards;
        public readonly LinkedList<Player> _players;

        public List<Player> Players { get => _players.ToList(); }

        public Player CurrentPlayer { get => _players.First(); }

        public PlayersHolder(List<Player> players, int numberOfPlayerCards)
        {
            _players = new(players);
            _winners = new Queue<Player>();
            _numberOfPlayerCards = numberOfPlayerCards;
        }

        public bool DrawCards(int numberOfCards, ICardDecksHolder cardsHolder, IUserCommunicator userCommunicator)
        {
            int cardsDraw = Enumerable.Range(0, numberOfCards).ToList()
                .Count(index =>
                {
                    Card? card = cardsHolder.DrawCard();

                    if (card == null)
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
                Player current = CurrentPlayer;
                _players.RemoveFirst();
                _players.AddLast(current);
            }
            else
            {
                Player current = _players.Last();
                _players.RemoveLast();
                _players.AddFirst(current);
            }
        }

        public Player GetWinner(IServiceProvider serviceProvider)
        {
            while (HasPlayerWon())
                CurrentPlayerPlay(serviceProvider);

            Player savedPlayer = CurrentPlayer;

            NextPlayer();

            if (!_players.Remove(savedPlayer))
                throw new Exception("error removing the player");

            _winners.Enqueue(savedPlayer);

            return savedPlayer;
        }

        private bool HasPlayerWon()
        {
            if (_players.Count == 1)
                return false;
            return !CurrentPlayer.IsHandEmpty();
        }

        public virtual void CurrentPlayerPlay(IServiceProvider serviceProvider)
        {
            IUserCommunicator userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            ICardDecksHolder cardsHolder = serviceProvider.GetRequiredService<ICardDecksHolder>();

            userCommunicator.SendAlertMessage($"Player[{CurrentPlayer.Id}]" +
                $" ({CurrentPlayer.Name}) is playing, " +
                $"{CurrentPlayer.PlayerCards.Count} cards in hand");

            Card topDiscard = cardsHolder.GetTopDiscard();
            userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");

            Card? playerCard = CurrentPlayer.PickCard(topDiscard.IsStackableWith);
            userCommunicator.SendAlertMessage($"Player picked: {playerCard?.ToString() ?? "no card"}\n");

            if (playerCard == null)
            {
                DrawCards(topDiscard.CardsToDraw(), cardsHolder, userCommunicator);
                topDiscard.FinishNoPlay();
                NextPlayer();
                _noPlayCounter++;

                if (_noPlayCounter >= _players.Count && _noPlayCounter % _players.Count == 0)
                {
                    string message = "Too many rounds without play, consider calling a tie ;)\n" +
                        "press any enter to continue";
                    userCommunicator.GetMessageFromUser(message);
                }

                return;
            }

            _noPlayCounter = 0;

            CurrentPlayer.PlayerCards.Remove(playerCard);
            cardsHolder.AddDiscardCard(playerCard);

            topDiscard.FinishPlay();
            playerCard.Play(topDiscard, this, serviceProvider);
        }

        public void ChangeDirection()
        {
            _isDirectionNormal = !_isDirectionNormal;
        }

        public virtual List<Card> ReturnCardsFromPlayers()
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

        public void DealCards(ICardDecksHolder cardsHolder)
        {
            Enumerable.Range(0, _numberOfPlayerCards)
                .Select(i =>
                {
                    _players.Select(p =>
                    {
                        Card? drawCard = cardsHolder.DrawCard();
                        if (drawCard != null)
                            p.AddCard(drawCard);

                        return p;
                    }).ToList();

                    return i;
                }).ToList();

            cardsHolder.DrawFirstCard();
        }
    }
}
