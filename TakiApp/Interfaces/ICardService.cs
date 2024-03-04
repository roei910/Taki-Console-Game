using TakiApp.Models;

public interface ICardService
{
    //TODO: all need to be async probably
    List<Card> GenerateCardsForDeck();
    bool CanStackOtherOnThis(Card topDiscard, Card otherCard);
    int CardsToDraw(Card cardPlayed);
    Task PlayAsync(Player player, Card cardPlayed, Card topDiscard);
    void FinishNoPlay(Card cardPlayed);
    void ResetCard(Card cardToReset);
}