using Taki.Game.Communicators;
using Taki.Game.Managers;
using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Algorithm;
using Taki.Game.Factories;
using Taki.Game.Messages;
using Microsoft.Extensions.Configuration;
using Taki;
using Taki.Game.Deck;
using Taki.Game.GameRunner;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IUserCommunicator, ConsoleUserCommunicator>()
    .AddSingleton<IPlayerAlgorithm, PlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerHateTakiAlgo>()
    .AddSingleton<ManualPlayerAlgorithm>()
    .AddSingleton<PlayersHolderFactory>()
    .AddSingleton<CardDeckFactory>()
    .AddSingleton<Random>()
    .AddSingleton<List<IPlayerAlgorithm>>()
    .AddSingleton<IConfiguration>(x => new ConfigurationBuilder()
        .AddJsonFile("AppConfigurations.json", false, true)
        .Build())
    .AddSingleton<ProgramVariables>()
    .AddSingleton<ICardDecksHolder, CardDecksHolder>()
    .AddSingleton<IGameScore, GameScore>()
    .BuildServiceProvider();

TakiGameRunner gameRunner = new TakiGameRunner(serviceProvider);

gameRunner.StartGameLoop();