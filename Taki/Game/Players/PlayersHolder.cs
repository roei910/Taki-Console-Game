using Taki.Game.Cards;
using Taki.Game.Database;
using Taki.Game.Deck;
using Taki.Game.Messages;

namespace Taki.Game.Players
{
    internal class PlayersHolder : IPlayersHolder
    {
        protected readonly IUserCommunicator _userCommunicator;
        private readonly TakiGameDatabaseHolder _takiGameDatabaseHolder;
        protected readonly Queue<Player> _winners;
        private int _noPlayCounter = 0;
        private List<Player> players = new List<Player>();
        protected readonly int _numberOfPlayerCards;
        public readonly LinkedList<Player> _players;

        public List<Player> Players { get => _players.ToList(); }

        public Player CurrentPlayer { get => _players.First(); }

        public PlayersHolder(List<Player> players, int numberOfPlayerCards, 
            IUserCommunicator userCommunicator, TakiGameDatabaseHolder takiGameDatabaseHolder)
        {
            _players = new(players);
            _winners = new Queue<Player>();
            _numberOfPlayerCards = numberOfPlayerCards;
            _userCommunicator = userCommunicator;
            _takiGameDatabaseHolder = takiGameDatabaseHolder;
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
                $"Player: {playerToDraw.Name}, drew {cardsDraw} card(s)\n");

            return true;
        }

        public void NextPlayer()
        {
            Player current = CurrentPlayer;
            _players.RemoveFirst();
            _players.AddLast(current);
        }

        public Player GetWinner(ICardDecksHolder cardDecksHolder, TakiGameDatabaseHolder takiGameDatabaseHolder)
        {
            while (!HasPlayerFinishedHand(cardDecksHolder))
            {
                CurrentPlayerPlay(cardDecksHolder);
                takiGameDatabaseHolder.UpdateDatabase(this, cardDecksHolder);
            }

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
            _userCommunicator.SendAlertMessage($"{CurrentPlayer.Name} is playing, " +
                $"{CurrentPlayer.PlayerCards.Count} cards in hand");

            Card topDiscard = cardDecksHolder.GetTopDiscard();
            _userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");
            topDiscard.PrintCard();

            Card? playerCard = CurrentPlayer.PickCard(topDiscard.IsStackableWith);
            _userCommunicator.SendAlertMessage($"Player picked: {playerCard?.ToString() ?? "draw card(s)"}\n");
            
            if (playerCard == null)
            {
                DrawCards(topDiscard.CardsToDraw(), CurrentPlayer, cardDecksHolder);
                topDiscard.FinishNoPlay();
                NextPlayer();
                _noPlayCounter++;

                if (_noPlayCounter >= 2*_players.Count && _noPlayCounter % _players.Count == 0)
                {
                    string message = "Too many rounds without play, consider calling a tie ;)\n" +
                        "press enter to continue";
                    _userCommunicator.GetMessageFromUser(message);
                }

                return;
            }

            playerCard.PrintCard();
            _noPlayCounter = 0;
            CurrentPlayer.PlayerCards.Remove(playerCard);
            cardDecksHolder.AddDiscardCard(playerCard);

            playerCard.Play(topDiscard, cardDecksHolder, this);
        }

        public void ChangeDirection()
        {
            var savedPlayers = _players.ToList();
            _players.Clear();
            savedPlayers.ForEach(player => _players.AddFirst(player));

            _takiGameDatabaseHolder.DeleteAllPlayers();
            _takiGameDatabaseHolder.CreateAllPlayers(Players);
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
