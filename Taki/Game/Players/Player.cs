using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Interfaces;

namespace Taki.Game.Players
{
    internal class Player : IEquatable<Player>
    {
        private static int id = 0;
        private readonly IPlayerAlgorithm choosingAlgorithm;
        public string Name { get; }
        public int Id { get; }
        public List<Card> PlayerCards { get; set; }

        public Player(string personName, IPlayerAlgorithm playerAlgorithm)
        {
            PlayerCards = [];
            Id = id++;
            choosingAlgorithm = playerAlgorithm ?? throw new ArgumentNullException(nameof(playerAlgorithm));
            Name = personName;
        }

        public Player(Player other)
        {
            PlayerCards = new(other.PlayerCards);
            Id = other.Id;
            choosingAlgorithm = other.choosingAlgorithm;
            Name = other.Name;
        }

        public Color ChooseColor(GameHandlers gameHandlers)
        {
            Color color = choosingAlgorithm.ChooseColor(gameHandlers);
            gameHandlers.GetMessageHandler()
                .SendMessageToUser($"Player chose the color: {color}");
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
            string cardsInHand = string.Join("\n", PlayerCards.Select((x, i) => $"{i}.{x}"));
            string str = $"Player[{Id}] {Name}, {PlayerCards.Count} Cards:\n{cardsInHand}";
            return str;
        }

        public bool Equals(Player? other)
        {
            if (other is null)
                return false;
            return Id == other.Id;
        }

        public Card? PickCard(Func<Card, bool> isSimilarTo, GameHandlers gameHandlers)
        {
            return choosingAlgorithm.ChooseCard(isSimilarTo, this, gameHandlers);
        }

        internal string GetInformation()
        {
            return $"Player[{Id}] ({Name}), {PlayerCards.Count} Algo: {choosingAlgorithm}";
        }

        public string GetName()
        {
            return $"Player[{Id}] ({Name})";
        }
    }
}
