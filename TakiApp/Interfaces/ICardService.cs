using TakiApp.Models;

internal interface ICardService
{
    List<Card> GenerateCardsForDeck();
}