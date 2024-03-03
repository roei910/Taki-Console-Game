using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.GameLogic
{
    internal class GameInitializer : IGameInitializer
    {
        private readonly IDal<GameSettings> _gameSettingsDal;
        private readonly IUserCommunicator _userCommunicator;
        private GameSettings? _gameSettings;

        public GameInitializer(IDal<GameSettings> gameSettingsDal,
            IUserCommunicator userCommunicator)
        {
            _gameSettingsDal = gameSettingsDal;
            _userCommunicator = userCommunicator;
        }

        public GameSettings GetGameSettings()
        {
            return _gameSettings!;
        }

        public void InitializeGame()
        {
            var gameSettings = Task.Run(async () => await _gameSettingsDal.FindAsync()).Result;

            if(gameSettings.Count == 0)
            {
                _userCommunicator.SendMessageToUser("Building a new Taki Game!");
                var isOnline = _userCommunicator.GetMessageFromUser("Please enter type of game: online or normal");
                var typeOfGame = _userCommunicator.GetMessageFromUser("Please enter pyramid or normal");
                var numberOfPlayerCards = _userCommunicator.GetNumberFromUser("Please enter number of player cards");

                _gameSettings = new GameSettings()
                {
                    HasGameStarted = false,
                    IsOnline = isOnline == "online",
                    NumberOfPlayerCards = numberOfPlayerCards,
                    TypeOfGame = typeOfGame
                };

                _gameSettingsDal.CreateOneAsync(_gameSettings);
                return;
            }

            //TODO: game already setting up in the background we need to check if we can enter or not.
            _gameSettings = gameSettings.First();
        }
    }
}
