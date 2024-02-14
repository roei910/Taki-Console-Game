using System.Drawing;
using Taki.Game.Algorithm;
using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Messages;

namespace Taki.Game.Players
{
    internal class Player : IPlayer, IEquatable<Player>
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

        public Color ChooseColor(IPlayersHandler playersHandler, IUserCommunicator userCommunicator)
        {
            Color color = choosingAlgorithm.ChooseColor(playersHandler, userCommunicator);
            userCommunicator.SendErrorMessage($"Player chose the color: {color}\n");

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

        public Card? PickCard(Func<Card, bool> isSimilarTo, IPlayersHandler playersHandler, ICardsHandler cardsHandler, IUserCommunicator userCommunicator)
        {
            //TODO: not good
            return choosingAlgorithm.ChooseCard(isSimilarTo, this, playersHandler, cardsHandler, userCommunicator);
        }

        public string GetInformation()
        {
            return $"Player[{Id}] ({Name}), {PlayerCards.Count} Algo: {choosingAlgorithm}";
        }

        public string GetName()
        {
            return $"Player[{Id}] ({Name})";
        }
    }
}
