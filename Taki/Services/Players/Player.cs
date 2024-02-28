using System.Drawing;
using Taki.Models.Algorithm;
using Taki.Shared.Abstract;
using Taki.Shared.Interfaces;
using Taki.Shared.Models.Dto;

namespace Taki.Models.Players
{
    //TODO: extract models from services
    public class Player : IPlayer, IEquatable<Player>
    {
        private static int id = 0;

        protected readonly IPlayerAlgorithm _choosingAlgorithm;
        private readonly IUserCommunicator _userCommunicator;
        public int Score { get; set; } = 0;
        public string Name { get; set; }
        public int Id { get; }
        public List<Card> PlayerCards { get; set; }
        public string ChoosingAlgorithm { get; set; }

        public Player(string personName, IPlayerAlgorithm playerAlgorithm, IUserCommunicator userCommunicator)
        {
            PlayerCards = [];
            Id = id++;
            _choosingAlgorithm = playerAlgorithm;
            Name = personName;
            _userCommunicator = userCommunicator;
            ChoosingAlgorithm = _choosingAlgorithm.ToString() ?? "no algorithm";
        }

        public Player(Player other)
        {
            PlayerCards = new(other.PlayerCards);
            Id = other.Id;
            _choosingAlgorithm = other._choosingAlgorithm;
            Name = other.Name;
            _userCommunicator = other._userCommunicator;
            ChoosingAlgorithm = other.ChoosingAlgorithm;
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

        public virtual string GetInformation()
        {
            return $"Name: {Name}, {PlayerCards.Count} cards in hand, Algo: {_choosingAlgorithm}";
        }

        public bool IsManualPlayer()
        {
            return _choosingAlgorithm is ManualPlayerAlgorithm;
        }

        public Player PickOtherPlayer(IPlayersHolder playersHolder)
        {
            return _choosingAlgorithm.ChoosePlayer(this, playersHolder);
        }

        public virtual PlayerDto ToPlayerDto()
        {
            List<CardDto> cards = PlayerCards.Select(card => card.ToCardDto()).ToList();

            return new PlayerDto(Score, Name, Id, cards, ChoosingAlgorithm);
        }
    }
}
