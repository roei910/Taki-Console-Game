using Taki.Shared.Abstract;
using Taki.Shared.Interfaces;
using Taki.Shared.Models.Dto;

namespace Taki.Models.Players
{
    //TODO: add action from cards instead of using the playersHolder
    public class PlayersHolder : IPlayersHolder
    {
        public readonly LinkedList<Player> _players;

        protected readonly IUserCommunicator _userCommunicator;
        protected readonly Queue<Player> _winners;
        protected readonly int _numberOfPlayerCards;

        private readonly IDal<PlayerDto> _playersDatabase;
        private int _noPlayCounter = 0;
        
        public List<Player> Players { get => _players.ToList(); }

        public Player CurrentPlayer { get => _players.First(); }

        public int NumberOfPlayerCards => _numberOfPlayerCards;

        public PlayersHolder(List<Player> players, int numberOfPlayerCards,
            IUserCommunicator userCommunicator, IDal<PlayerDto> playersDatabase)
        {
            _players = new(players);
            _winners = new Queue<Player>();
            _numberOfPlayerCards = numberOfPlayerCards;
            _userCommunicator = userCommunicator;
            _playersDatabase = playersDatabase;
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

            _playersDatabase.UpdateOne(playerToDraw.ToPlayerDto());

            return true;
        }

        public void NextPlayer()
        {
            Player current = CurrentPlayer;
            _players.RemoveFirst();
            _players.AddLast(current);
            _playersDatabase.Delete(current.Id);
            _playersDatabase.Create(current.ToPlayerDto());
        }

        public Player GetWinner(ICardDecksHolder cardDecksHolder)
        {
            while (!HasPlayerFinishedHand(cardDecksHolder))
                CurrentPlayerPlay(cardDecksHolder);

            Player playerWon = _players.FirstOrDefault(player => player.IsHandEmpty(), _players.First());

            if (!_players.Remove(playerWon))
                throw new Exception("error removing the player");

            _winners.Enqueue(playerWon);

            _playersDatabase.Delete(playerWon.Id);
            _playersDatabase.Create(playerWon.ToPlayerDto());

            return playerWon;
        }

        public void ChangeDirection()
        {
            var savedPlayers = _players.ToList();
            _players.Clear();
            savedPlayers.ForEach(player => _players.AddFirst(player));

            _playersDatabase.DeleteAll();
            _playersDatabase.CreateMany(_players.Select(p => p.ToPlayerDto()).ToList());
            _playersDatabase.CreateMany(_winners.ToList().Select(w => w.ToPlayerDto()).ToList());
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

            _players.ToList().ForEach(p => _playersDatabase.UpdateOne(p.ToPlayerDto()));
        }

        public virtual void ResetPlayers()
        {
            _ = Enumerable.Range(0, _winners.Count)
            .Select(i =>
            {
                _players.AddLast(_winners.Dequeue());
                return i;
            }).ToList();

            _playersDatabase.CreateMany(_players.Select(p => p.ToPlayerDto()).ToList());
        }

        public Card? GetCardFromCurrentPlayer(ICardDecksHolder cardDecksHolder, Func<Card,bool> isStackableWith,
            string? elseMessage = null)
        {
            _userCommunicator.SendAlertMessage($"{CurrentPlayer.Name} is playing, " +
                $"{CurrentPlayer.PlayerCards.Count} cards in hand");

            Card? playerCard = CurrentPlayer.PickCard(isStackableWith, elseMessage);
            _userCommunicator.SendAlertMessage($"Player picked: {playerCard?.ToString() ?? "draw card(s)"}\n");

            if (playerCard != null)
            {
                _noPlayCounter = 0;
                CurrentPlayer.PlayerCards.Remove(playerCard);
                cardDecksHolder.AddDiscardCard(playerCard);
                _playersDatabase.UpdateOne(CurrentPlayer.ToPlayerDto());
            }

            return playerCard;
        }

        protected virtual bool HasPlayerFinishedHand(ICardDecksHolder cardDecksHolder)
        {
            if (_players.Count == 1)
                return true;

            return _players.Any(player => player.IsHandEmpty());
        }

        private void CurrentPlayerPlay(ICardDecksHolder cardDecksHolder)
        {
            _userCommunicator.SendAlertMessage($"{CurrentPlayer.Name} is playing, " +
                $"{CurrentPlayer.PlayerCards.Count} cards in hand");

            Card topDiscard = cardDecksHolder.GetTopDiscard();
            _userCommunicator.SendAlertMessage($"Top discard: {topDiscard}");
            topDiscard.PrintCard();

            Card? playerCard = GetCardFromCurrentPlayer(cardDecksHolder, topDiscard.IsStackableWith);

            if (playerCard == null)
            {
                DrawCards(topDiscard.CardsToDraw(), CurrentPlayer, cardDecksHolder);
                topDiscard.FinishNoPlay();
                NextPlayer();
                _noPlayCounter++;

                if (_noPlayCounter >= 2 * _players.Count && _noPlayCounter % _players.Count == 0)
                {
                    string message = "Too many rounds without play, consider calling a tie ;)\n" +
                        "press enter to continue";
                    _userCommunicator.GetMessageFromUser(message);
                }

                return;
            }

            playerCard.Play(topDiscard, cardDecksHolder, this);
        }

        public void UpdateWinnersFromDb()
        {
            //TODO: check this is wrong! should get from db...
            var winners = Players.Where(p => p.PlayerCards.Count == 0).ToList();

            if (winners.Any())
            {
                winners.ForEach(p =>
                {
                    var winner = _players.Where(player => player.Id == p.Id).First();
                    _winners.Enqueue(winner);
                });
            }
        }
    }
}
