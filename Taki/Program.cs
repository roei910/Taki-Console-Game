using Taki.Game.Communicators;
using Taki.Game.Managers;
using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Algorithm;
using Taki.Game.Interfaces;
using Taki.Game.Factories;
using Taki.Game.GameRules;
using Taki.Game.Handlers;
using Taki.Game.General;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IMessageHandler, ConsoleMessageHandler>()
    .AddSingleton<Utilities>()
    .BuildServiceProvider();

//var messageHandler = serviceProvider.GetRequiredService<MessageHandler>();

List<IPlayerAlgorithm> algorithms =
[
    new PlayerAlgorithm(),
    new PlayerHateTakiAlgo()
];

TakiGameFactory gameFactory = new(serviceProvider);
PlayersHandlerFactory playersHandlerFactory = new(serviceProvider, algorithms);
CardsHandlerFactory cardsHandlerFactory = new();

PlayersHandler playerHandler = playersHandlerFactory.GeneratePlayersHandler();
CardsHandler cardsHandler = cardsHandlerFactory.GenerateCardsHandler();

GameHandlers gameHandlers = new(playerHandler, cardsHandler, serviceProvider);

TakiGameRunner takiGameRunner = gameFactory.ChooseTypeOfGame(gameHandlers);
takiGameRunner.StartGame();