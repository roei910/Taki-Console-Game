using System.Drawing;
using Taki.Game.Algorithm;
using Taki.Game.Cards;
using Taki.Game.Messages;

namespace Taki.Game.Players
{
    internal class Player : IPlayer, IEquatable<Player>
    {
        private static int id = 0;

        private readonly IPlayerAlgorithm _choosingAlgorithm;
        private readonly IUserCommunicator _userCommunicator;
        public int Score { get; set; } = 0;
        public string Name { get; }
        public int Id { get; }
        public List<Card> PlayerCards { get; set; }

        public Player(string personName, IPlayerAlgorithm playerAlgorithm, IUserCommunicator userCommunicator)
        {
            PlayerCards = [];
            Id = id++;
            _choosingAlgorithm = playerAlgorithm ?? throw new ArgumentNullException(nameof(playerAlgorithm));
            Name = personName;
            _userCommunicator = userCommunicator;
        }

        public Player(Player other)
        {
            PlayerCards = new(other.PlayerCards);
            Id = other.Id;
            _choosingAlgorithm = other._choosingAlgorithm;
            Name = other.Name;
            _userCommunicator = other._userCommunicator;
        }

        public Color ChooseColor()
        {
            Color color = _choosingAlgorithm.ChooseColor(PlayerCards);
            _userCommunicator.SendErrorMessage($"Player chose the color: {color}\n");

            return color;
        }

        public void AddCard(Card card)
        {
            PlayerCards.Add(card);
        }

        public bool IsHandEmpty()
        {
            return PlayerCards.Count == 0;
        }

        public override string ToString()
        {
            string cardsInHand = string.Join("\n", PlayerCards.Select((x, i) => $"{i}.{x}").ToList());
            string str = $"Player[{Id}] {Name}, {PlayerCards.Count} Cards:\n{cardsInHand}";
            return str;
        }

        public bool Equals(Player? other)
        {
            if (other is null)
                return false;
            return Id == other.Id;
        }

        public Card? PickCard(Func<Card, bool> IsStackableWith, string? elseMessage = null)
        {
            return _choosingAlgorithm.ChooseCard(IsStackableWith, PlayerCards, elseMessage);
        }

        public string GetInformation()
        {
            return $"Name: {Name}, Algo: {_choosingAlgorithm}";
        }

        public bool IsManualPlayer()
        {
            return _choosingAlgorithm is ManualPlayerAlgorithm;
        }
    }
}
