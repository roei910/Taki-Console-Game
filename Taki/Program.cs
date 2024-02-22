using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Taki;
using MongoDB.Bson.Serialization;
using Taki.Game.Serializers;
using Taki.Database;
using Taki.Dto;
using Taki.GameRunner;
using Taki.Factories;
using Taki.Interfaces;
using Taki.Models.Algorithm;
using Taki.Models.Messages;

//TODO: update saving while doing special cards (Taki etc)
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