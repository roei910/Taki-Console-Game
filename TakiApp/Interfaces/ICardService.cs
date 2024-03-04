using TakiApp.Models;

public interface ICardService
{
    List<Card> GenerateCardsForDeck();
    bool CanStackOnOther(Card topDiscard, Card otherCard);
}