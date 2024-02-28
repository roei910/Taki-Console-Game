using Taki.Shared.Models.Dto;
using Taki.Shared.Models;

namespace Taki.Extensions
{
    public static class PlayerExtension
    {
        public static PlayerDto ToPlayerDto(this Player player)
        {
            List<CardDto> cards = player.PlayerCards.Select(card => card.ToCardDto()).ToList();

            return new PlayerDto(player.Score, player.Name!, player.Id, cards, player.ChoosingAlgorithm!);
        }
    }
}
