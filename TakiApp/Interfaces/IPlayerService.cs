using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IPlayerService
    {
        Card PickCard(Player currentPlayer, Card topDiscard);
        void AddCard(Card card);
        void ChooseCard();
        Player PickOtherPlayer(List<Player> players);
    }
}