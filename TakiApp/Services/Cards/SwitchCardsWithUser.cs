using TakiApp.Interfaces;

namespace TakiApp.Services.Cards
{
    public class SwitchCardsWithUser : SwitchCardsWithDirection
    {
        public SwitchCardsWithUser(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository) : 
            base(discardPileRepository, playersRepository) { }
    }
}
