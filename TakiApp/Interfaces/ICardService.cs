using TakiApp.Models;

public interface ICardService
{
    List<Card> GenerateCardsForDeck();
    bool CanStackOtherOnThis(Card topDiscard, Card otherCard);
    int CardsToDraw(Card cardPlayed);
    void Play(Player player, Card cardPlayed, Card topDiscard);
    void FinishNoPlay(Card cardPlayed);
    void ResetCard(Card cardToReset);
}