using Taki.Game.Communicators;
using Taki.Game.Managers;
using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Algorithm;
using Taki.Game.Factories;
using Taki.Game.Messages;
using Microsoft.Extensions.Configuration;
using Taki;
using Taki.Game.GameRunner;
using Taki.Game.Database;
using Taki.Game.Players;
using Taki.Game.Cards;

//TODO: add check to see names are unique
//TODO: issue with restarting the top card doesnt know if there is anything before him, not being saved. save the previous if needed
//TODO: update saving when doing special cards (Taki etc)
//TODO: create classes to deal with restoring and maybe also saving to the database

var serviceProvider = new ServiceCollection()
    .AddSingleton<IUserCommunicator, ConsoleUserCommunicator>()
    .AddSingleton<IPlayerAlgorithm, PlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerHateTakiAlgo>()
    .AddSingleton<List<IPlayerAlgorithm>>()
    .AddSingleton<IGameScore, GameScore>()
    .AddSingleton<ManualPlayerAlgorithm>()
    .AddSingleton<PlayersHolderFactory>()
    .AddSingleton<CardDeckFactory>()
    .AddSingleton<ProgramVariables>()
    .AddSingleton<Random>()
    .AddSingleton<IConfiguration>(x => new ConfigurationBuilder()
        .AddJsonFile("AppConfigurations.json", false, true)
        .Build())
    .AddSingleton<TakiGameRunner>()
    .AddSingleton<IDatabase<PlayerDTO>, PlayerDatabase>()
    .AddKeyedSingleton<IDatabase<CardDTO>, CardDatabase>("drawPile", (serviceProvider, x) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return new CardDatabase(configuration, "DrawPile");
        })
    .AddKeyedSingleton<IDatabase<CardDTO>, CardDatabase>(
        "discardPile", (serviceProvider, x) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return new CardDatabase(configuration, "DiscardPile");
        })
    .AddSingleton<TakiGameDatabaseHolder>()
    .BuildServiceProvider();

TakiGameRunner gameRunner = serviceProvider.GetRequiredService<TakiGameRunner>();

gameRunner.StartGameLoop();