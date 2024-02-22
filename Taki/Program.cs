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
using Taki.Game.Cards.DTOs;
using MongoDB.Bson.Serialization;
using Taki.Game.Serializers;

//TODO: add check to see names are unique
//TODO: update saving when doing special cards (Taki etc)
//TODO: create classes to deal with restoring and maybe also saving to the database, works with switchCards
//TODO: create a way to save the DTO from the Card
//TODO: the game allows to use SUPERTAKI on TAKI => check why

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
    .AddKeyedSingleton<IDatabase<CardDto>, CardDatabase>("drawPile", (serviceProvider, x) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return new CardDatabase(configuration, "DrawPile");
        })
    .AddKeyedSingleton<IDatabase<CardDto>, CardDatabase>(
        "discardPile", (serviceProvider, x) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return new CardDatabase(configuration, "DiscardPile");
        })
    .AddSingleton<TakiGameDatabaseHolder>()
    .BuildServiceProvider();

BsonSerializer.RegisterSerializer(new JObjectBsonSerializer());

TakiGameRunner gameRunner = serviceProvider.GetRequiredService<TakiGameRunner>();

gameRunner.StartGameLoop();