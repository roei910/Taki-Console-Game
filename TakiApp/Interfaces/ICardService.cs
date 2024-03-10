using TakiApp.Interfaces;
using TakiApp.Models;

public interface ICardService
{
    List<Card> GenerateCardsForDeck();
    int CardsToDraw(Card cardPlayed);
    Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService);
    Task FinishNoPlay(Card cardPlayed);
    Task FinishPlayAsync(Card cardToReset);
    bool CanStackOtherOnThis(Card topDiscard, Card otherCard, ICardPlayService cardPlayService);
}