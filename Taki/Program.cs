using Taki.Game.Communicators;
using Taki.Game.Managers;
using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Algorithm;
using Taki.Game.Interfaces;
using Taki.Game.Factories;

//TODO: maybe save instances of all handlers inside the code????

var serviceProvider = new ServiceCollection()
    .AddSingleton<IMessageHandler, ConsoleMessageHandler>()
    .AddSingleton<IPlayerAlgorithm, ManualPlayerAlgorithm>()
    .BuildServiceProvider();

List<IPlayerAlgorithm> algorithms =
[
    new PlayerAlgorithm(),
    new PlayerHateTakiAlgo(),
];

PlayersHandlerFactory playersHandlerFactory = new(serviceProvider, algorithms);
CardsHandlerFactory cardsHandlerFactory = new();

TakiGameFactory gameFactory = new(serviceProvider);
TakiGameRunner gameRunner = gameFactory.ChooseTypeOfGame(playersHandlerFactory, cardsHandlerFactory, serviceProvider);

gameRunner.StartGame();