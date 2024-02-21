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
//TODO: make it possible to print many cards in a row of same color for manual choose, use the get stringArray method and add the strings
//TODO: create mongo db collection to save every detail about the game => restart from the same place you stopped before!
//TODO: continue saving the players cards and card decks in database
//TODO: issue with restarting the top card doesnt know if there is anything before him, not being saved. save the previous if needed
//TODO: not saving which player is currently playing

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