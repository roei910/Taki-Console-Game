using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.Messages;

namespace Taki.Game.Players
{
    //TODO: check finish hand in pyramid taki => pyramid not working
    internal class PlayersHolder : IPlayersHolder
    {
        private readonly Queue<Player> _winners;
        private bool _isDirectionNormal = true;
        private int _noPlayCounter = 0;
        protected readonly int _numberOfPlayerCards;
        protected readonly IServiceProvider _serviceProvider;
        public readonly LinkedList<Player> _players;

        public List<Player> Players { get => _players.ToList(); }

        public Player CurrentPlayer { get => _players.First(); }

        public PlayersHolder(List<Player> players, int numberOfPlayerCards, IServiceProvider serviceProvider)
        {
            _players = new(players);
            _winners = new Queue<Player>();
            _numberOfPlayerCards = numberOfPlayerCards;
            _serviceProvider = serviceProvider;
        }

        public bool DrawCards(int numberOfCards, Player playerToDraw)
        {
            ICardDecksHolder cardsHolder = _serviceProvider.GetRequiredService<ICardDecksHolder>();
            IUserCommunicator userCommunicator = _serviceProvider.GetRequiredService<IUserCommunicator>();

            int cardsDraw = Enumerable.Range(0, numberOfCards).ToList()
                .Count(index =>
                {
                    Card? card = cardsHolder.DrawCard();

                    if (card == null)
                        return false;
                    playerToDraw.AddCard(card);

                    return true;
                });

            if (cardsDraw == 0)
                return false;

            userCommunicator.SendErrorMessage(
                $"Player[{playerToDraw.Id}]: drew {cardsDraw} card(s)\n");

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

        public Player GetWinner()
        {
            while (!HasPlayerFinishedHand())
                CurrentPlayerPlay();

            Player playerWon = _players.First(player => player.IsHandEmpty());

            if (!_players.Remove(playerWon))
                throw new Exception("error removing the player");

            _winners.Enqueue(playerWon);

            return playerWon;
        }

        protected virtual bool HasPlayerFinishedHand()
        {
            if (_players.Count == 1)
                return false;

            return _players.Any(player => player.IsHandEmpty());
        }

        public void CurrentPlayerPlay()
        {
            IUserCommunicator userCommunicator = _serviceProvider.GetRequiredService<IUserCommunicator>();
            ICardDecksHolder cardsHolder = _serviceProvider.GetRequiredService<ICardDecksHolder>();

            userCommunicator.SendAlertMessage($"Player[{CurrentPlayer.Id}]" +
                $" ({CurrentPlayer.Name}) is playing, " +
                $"{CurrentPlayer.PlayerCards.Count} cards in hand");

            Card topDiscard = cardsHolder.GetTopDiscard();
            userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");

            Card? playerCard = CurrentPlayer.PickCard(topDiscard.IsStackableWith);
            userCommunicator.SendAlertMessage($"Player picked: {playerCard?.ToString() ?? "no card"}\n");

            if (playerCard == null)
            {
                DrawCards(topDiscard.CardsToDraw(), CurrentPlayer);
                topDiscard.FinishNoPlay();
                NextPlayer();
                _noPlayCounter++;

                if (_noPlayCounter >= 2*_players.Count && _noPlayCounter % _players.Count == 0)
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
            playerCard.Play(topDiscard, this, _serviceProvider);
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
