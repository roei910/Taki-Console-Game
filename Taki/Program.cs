using Taki.Game.Communicators;
using Taki.Game.Managers;
using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Algorithm;
using Taki.Game.Interfaces;
using Taki.Game.Factories;
using Taki.Game.GameRules;
using Taki.Game.Handlers;
using Taki.Game.General;

//TODO: naming conventions and logics
//TODO: check what happends when finishing hand with plus card

var serviceProvider = new ServiceCollection()
    .AddSingleton<IMessageHandler, ConsoleMessageHandler>()
    .AddSingleton<Utilities>()
    .BuildServiceProvider();

List<IPlayerAlgorithm> algorithms =
[
    new PlayerAlgorithm(),
    new PlayerHateTakiAlgo(),
    new ManualPlayerAlgorithm()
];

TakiGameFactory gameFactory = new(serviceProvider);
PlayersHandlerFactory playersHandlerFactory = new(serviceProvider, algorithms);
CardsHandlerFactory cardsHandlerFactory = new();

PlayersHandler playerHandler = playersHandlerFactory.GeneratePlayersHandler();
CardsHandler cardsHandler = cardsHandlerFactory.GenerateCardsHandler();

GameHandlers gameHandlers = new(playerHandler, cardsHandler, serviceProvider);

TakiGameRunner takiGameRunner = gameFactory.ChooseTypeOfGame(gameHandlers);
takiGameRunner.StartGame();