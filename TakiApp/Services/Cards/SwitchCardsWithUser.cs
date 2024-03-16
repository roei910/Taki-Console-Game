using System.Drawing;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Services.Cards
{
    public class SwitchCardsWithUser : SwitchCardsWithDirection
    {
        private readonly IAlgorithmService _algorithmService;

        public SwitchCardsWithUser(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository, 
            IAlgorithmService algorithmService, IUserCommunicator userCommunicator) : 
            base(discardPileRepository, playersRepository, userCommunicator)
        {
            _algorithmService = algorithmService;
        }

        public override List<Card> GenerateCardsForDeck()
        {
            return Enumerable.Range(0, 2)
                .Select(j => new Card(typeof(SwitchCardsWithUser).ToString(), Color.Empty.Name)).ToList();
        }

        public override async Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            var players = await _playersRepository.GetAllAsync();
            players = players.Where(p => p.Id != player.Id && p.Cards.Count > 0).ToList();

            Player playerToSwitch = _algorithmService.PickOtherPlayer(player, players);

            (player.Cards, playerToSwitch.Cards) = (playerToSwitch.Cards, player.Cards);

            await UpdatePreviousCardAsync(cardPlayed);
            await _playersRepository.UpdateManyAsync(new List<Player>() { player, playerToSwitch });

            await _playersRepository.SendMessagesToPlayersAsync(
                player.Name!, $"Player: {player.Name} used switch cards with Player: {playerToSwitch.Name}!\n", player);

            await _playersRepository.NextPlayerAsync();
        }
    }
}