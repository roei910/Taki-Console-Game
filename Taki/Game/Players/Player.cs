using System.Drawing;
using Taki.Game.Cards;
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

        //public bool AskPlayerToPickCard(Card topDiscardPileCard, out Card chosenCard)
        //{
        //    chosenCard = choosingAlgorithm.ChooseCard(topDiscardPileCard, this);
        //    if (chosenCard.Id == topDiscardPileCard.Id)
        //        return false;
        //    if (!PlayerCards.Remove(chosenCard))
        //        throw new Exception("card not found in player cards");
        //    return true;
        //}

        public Color ChooseColor()
        {
            Color color = choosingAlgorithm.ChooseColor(this);
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

        //public bool AskPlayerToPickCardPlus2(Card topDiscardPileCard, out Card chosenCard)
        //{
        //    chosenCard = choosingAlgorithm.ChoosePlus2Card(topDiscardPileCard, this);
        //    //TODO: ifs need to go
            
        //    if (chosenCard.Id == topDiscardPileCard.Id)
        //        return false;
        //    if (!PlayerCards.Remove(chosenCard))
        //        throw new Exception("card not found in player cards");
        //    return true;
        //}

        public override string ToString()
        {
            string cardsInHand = string.Join("\n", PlayerCards.Select((x, i) => $"{i}.{x}"));
            string str = $"Player[{Id}] {Name}, {PlayerCards.Count} Cards:\n{cardsInHand}";
            return str;
        }

        public bool Equals(Player? other)
        {
            if(other is null)
                return false;
            return Id == other.Id;
        }

        internal Card? PickCard(Func<Card, bool> isSimilarTo)
        {
            return PlayerCards.FirstOrDefault(card => isSimilarTo(card), null);
        }
    }
}
