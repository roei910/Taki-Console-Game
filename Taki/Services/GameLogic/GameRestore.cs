using Microsoft.Extensions.DependencyInjection;
using Taki.Data;
using Taki.Models.Deck;
using Taki.Models.Players;
using Taki.Shared.Interfaces;
using Taki.Shared.Models;
using Taki.Shared.Models.Dto;

namespace Taki.Models.GameLogic
{
    internal class GameRestore
    {
        private readonly IUserCommunicator _userCommunicator;
        private readonly List<IPlayerAlgorithm> _playerAlgorithms;
        private readonly IDal<PlayerDto> _playersDatabase;
        private readonly IDal<GameSettings> _gameSettingsDatabase;
        private readonly CardDeckDatabase _cardDeckDatabase;

        public GameRestore(IUserCommunicator userCommunicator, List<IPlayerAlgorithm> playerAlgorithms,
            ManualPlayerAlgorithm manualPlayerAlgorithm, IDal<PlayerDto> playersDatabase, CardDeckDatabase cardDeckDatabase,
            IServiceProvider serviceProvider)
        {
            _userCommunicator = userCommunicator;
            _playerAlgorithms = playerAlgorithms;
            _playersDatabase = playersDatabase;
            _cardDeckDatabase = cardDeckDatabase;
            _playerAlgorithms.Add(manualPlayerAlgorithm);
            _gameSettingsDatabase = serviceProvider.GetRequiredKeyedService<IDal<GameSettings>>("gameSettings");
        }

        public bool TryRestoreTakiGame(ICardDecksHolder cardDecksHolder, out IPlayersHolder? _playersHolder)
        {
            if(_playersDatabase.IsEmpty())
            {
                _playersHolder = null;
                return false;
            }

            var numberOfPlayerCards = _gameSettingsDatabase.FindAll().First().NumberOfPlayerCards;

            _playersHolder = GeneratePlayersHolder(cardDecksHolder, numberOfPlayerCards);
            UpdateCardDeckFromDatabase(cardDecksHolder);

            _userCommunicator.SendMessageToUser("users restored are:");
            var playersInormation = _playersHolder.Players.Select((p, i) => $"{i + 1}. {p.GetInformation()}")
                .ToList();
            _userCommunicator.SendMessageToUser(string.Join("\n", playersInormation));
            _userCommunicator.SendMessageToUser();

            return true;
        }

        public void DeleteAll()
        {
            _playersDatabase.DeleteAll();
            _cardDeckDatabase.DeleteAll();
        }

        public void CreateGameSettings(GameSettings gameSettings)
        {
            _gameSettingsDatabase.DeleteAll();
            _gameSettingsDatabase.Create(gameSettings);
        }

        private List<Player> GeneratePlayersFromDTO(List<PlayerDto> playerDTOs)
        {
            var players = playerDTOs.Select(player =>
            {
                IPlayerAlgorithm playerAlgorithm = _playerAlgorithms.Where(algo => algo.ToString() == player.ChoosingAlgorithm).First();

                return new Player(player.Name, playerAlgorithm, _userCommunicator)
                {
                    Score = player.Score
                };
            }).ToList();

            return players;
        }

        private IPlayersHolder GeneratePlayersHolder(ICardDecksHolder cardDecksHolder, int numberOfPlayerCards)
        {
            var playersDto = _playersDatabase.FindAll();
            var players = GeneratePlayersFromDTO(playersDto);
            IPlayersHolder playersHolder;

            if (playersDto.Where(p => p.CurrentNumberOfCards != -1).Any())
            {
                players = players.Select((player, index) =>
                    (Player)new PyramidPlayer(player, playersDto[index].CurrentNumberOfCards)).ToList();

                playersHolder = new PyramidPlayersHolder(players, numberOfPlayerCards, _userCommunicator, _playersDatabase);
                DealCardsToDtoPlayers(cardDecksHolder, playersDto, playersHolder);

                return playersHolder;
            }

            playersHolder = new PlayersHolder(players, numberOfPlayerCards, _userCommunicator, _playersDatabase);
            DealCardsToDtoPlayers(cardDecksHolder, playersDto, playersHolder);

            return playersHolder;
        }

        private static void DealCardsToDtoPlayers(ICardDecksHolder cardDecksHolder, List<PlayerDto> playersDto, IPlayersHolder playersHolder)
        {
            playersDto.ForEach(player =>
            {
                var cards = player.PlayerCards.Select(card =>
                    cardDecksHolder.RemoveCardByDTO(card)).ToList();

                playersHolder!.Players.Where(p => p.Name == player.Name).First()
                    .PlayerCards = cards;
            });
        }

        private void UpdateCardDeckFromDatabase(ICardDecksHolder cardDecksHolder)
        {
            List<CardDto> drawPileDto = _cardDeckDatabase.GetDrawCards();
            List<CardDto> discardPileDto = _cardDeckDatabase.GetDiscardCards();

            CardDeck drawCardDeck = cardDecksHolder.GetDrawCardDeck();
            CardDeck discardCardDeck = cardDecksHolder.GetDiscardCardDeck();

            var newDrawPile = drawPileDto.Select(drawCardDeck.RemoveFirstDTO).ToList();
            drawCardDeck.AddMany(newDrawPile);
            var newDiscardPile = discardPileDto.Select(drawCardDeck.RemoveFirstDTO).ToList();
            discardCardDeck.AddMany(newDiscardPile);

            cardDecksHolder.GetDiscardCardDeck().GetAllCards().Select((card, index) =>
            {
                card.UpdateFromDto(discardPileDto[index], cardDecksHolder);
                return card;
            }).ToList();
        }
    }
}
