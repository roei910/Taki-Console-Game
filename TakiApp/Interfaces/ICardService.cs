using TakiApp.Interfaces;
using TakiApp.Models;

public interface ICardService
{
    //TODO: all need to be async probably
    List<Card> GenerateCardsForDeck();
    bool CanStackOtherOnThis(Card topDiscard, Card otherCard);
    int CardsToDraw(Card cardPlayed);
    Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService);
    void FinishNoPlay(Card cardPlayed);
    Task ResetCard(Card cardToReset);
}