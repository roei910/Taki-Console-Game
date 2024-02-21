namespace Taki.Game.Cards
{
    internal class CardDTO
    {
        public string Type { get; set; }
        public string Color { get; set; }
        public int Id { get; set; }
        internal delegate CardDTO CardToDTODelegate(Card card);

        public CardDTO(int id, string type, string color)
        {
            Type = type;
            Color = color;
            Id = id;
        }

        public static CardDTO CardToDTO(Card card)
        {
            string type = card.GetType().ToString();
            string color = card is not ColorCard colorCard ? ColorCard.DEFAULT_COLOR.ToString() : colorCard.CardColor;
            int id = card.Id;

            return new CardDTO(id, type, color);
        }
    }
}
