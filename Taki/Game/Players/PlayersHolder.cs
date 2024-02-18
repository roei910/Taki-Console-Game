using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.Messages;

namespace Taki.Game.Players
{
    internal class PlayersHolder : IPlayersHolder
    {
        protected readonly IUserCommunicator _userCommunicator;
        protected readonly Queue<Player> _winners;
        private bool _isDirectionNormal = true;
        private int _noPlayCounter = 0;
        private List<Player> players = new List<Player>();
        //TODO: check why never used
        private int numberOfPlayerCards;
        protected readonly int _numberOfPlayerCards;
        public readonly LinkedList<Player> _players;

        public List<Player> Players { get => _players.ToList(); }

        public Player CurrentPlayer { get => _players.First(); }

        public PlayersHolder(List<Player> players, int numberOfPlayerCards, 
            IUserCommunicator userCommunicator)
        {
            _players = new(players);
            _winners = new Queue<Player>();
            _numberOfPlayerCards = numberOfPlayerCards;
            _userCommunicator = userCommunicator;
        }

        public bool DrawCards(int numberOfCards, Player playerToDraw, ICardDecksHolder cardDecksHolder)
        {
            int cardsDraw = Enumerable.Range(0, numberOfCards).ToList()
                .Count(index =>
                {
                    Card? card = cardDecksHolder.DrawCard();

                    if (card == null)
                        return false;
                    playerToDraw.AddCard(card);

                    return true;
                });

            if (cardsDraw == 0)
                return false;

            _userCommunicator.SendErrorMessage(
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

        public Player GetWinner(ICardDecksHolder cardDecksHolder)
        {
            while (!HasPlayerFinishedHand(cardDecksHolder))
                CurrentPlayerPlay(cardDecksHolder);

            Player playerWon = _players.FirstOrDefault(player => player.IsHandEmpty(), _players.First());

            if (!_players.Remove(playerWon))
                throw new Exception("error removing the player");

            _winners.Enqueue(playerWon);

            return playerWon;
        }

        protected virtual bool HasPlayerFinishedHand(ICardDecksHolder cardDecksHolder)
        {
            if (_players.Count == 1)
                return true;

            return _players.Any(player => player.IsHandEmpty());
        }

        public void CurrentPlayerPlay(ICardDecksHolder cardDecksHolder)
        {
            _userCommunicator.SendAlertMessage($"Player[{CurrentPlayer.Id}]" +
                $" ({CurrentPlayer.Name}) is playing, " +
                $"{CurrentPlayer.PlayerCards.Count} cards in hand");

            Card topDiscard = cardDecksHolder.GetTopDiscard();
            _userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");

            Card? playerCard = CurrentPlayer.PickCard(topDiscard.IsStackableWith);
            _userCommunicator.SendAlertMessage($"Player picked: {playerCard?.ToString() ?? "no card"}\n");

            if (playerCard == null)
            {
                DrawCards(topDiscard.CardsToDraw(), CurrentPlayer, cardDecksHolder);
                topDiscard.FinishNoPlay();
                NextPlayer();
                _noPlayCounter++;

                if (_noPlayCounter >= 2*_players.Count && _noPlayCounter % _players.Count == 0)
                {
                    string message = "Too many rounds without play, consider calling a tie ;)\n" +
                        "press any enter to continue";
                    _userCommunicator.GetMessageFromUser(message);
                }

                return;
            }

            _noPlayCounter = 0;

            CurrentPlayer.PlayerCards.Remove(playerCard);
            cardDecksHolder.AddDiscardCard(playerCard);

            topDiscard.FinishPlay();
            playerCard.Play(topDiscard, cardDecksHolder, this);
        }

        public void ChangeDirection()
        {
            _isDirectionNormal = !_isDirectionNormal;
        }

        public List<Card> ReturnCardsFromPlayers()
        {
            List<Card> cards = [];

            _ = _players.Select(player =>
            {
                cards.AddRange(player.PlayerCards);
                player.PlayerCards.Clear();

                return player;
            }).ToList();

            _ = _winners.Select(player =>
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

        public virtual void ResetPlayers()
        {
            _ = Enumerable.Range(0, _winners.Count)
            .Select(i =>
            {
                _players.AddLast(_winners.Dequeue());
                return i;
            }).ToList();
        }
    }
}
