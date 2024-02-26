using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Taki;
using MongoDB.Bson.Serialization;
using Taki.Factories;
using Taki.Models.Algorithm;
using Taki.Models.Messages;
using Taki.Models.GameLogic;
using Taki.Serializers;
using Taki.Data;
using Taki.Models.Deck;
using Taki.Dal;
using Taki.Shared.Interfaces;
using Taki.Shared.Models;
using Taki.Shared.Models.Dto;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(x => new ConfigurationBuilder()
        .AddJsonFile("AppConfigurations.json", false, true)
        .Build())
    .AddSingleton<IUserCommunicator, ConsoleUserCommunicator>()
    .AddSingleton<IManualPlayerAlgorithm, ManualPlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerHateTakiAlgo>()
    .AddTransient<List<IPlayerAlgorithm>>()
    .AddSingleton<IGameScore, GameScore>()
    .AddSingleton<PlayersHolderFactory>()
    .AddSingleton<CardDeckFactory>()
    .AddSingleton<ProgramVariables>()
    .AddSingleton<Random>()
    .AddSingleton<TakiGameRunner>()
    .AddSingleton<IDal<PlayerDto>, PlayerDal>()
    //TODO: move names to config
    .AddKeyedSingleton<IDal<CardDto>, CardDal>("drawPile", (serviceProvider, x) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return new CardDal(configuration, "DrawPile");
        })
    .AddKeyedSingleton<IDal<CardDto>, CardDal>(
        "discardPile", (serviceProvider, x) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return new CardDal(configuration, "DiscardPile");
        })
    .AddKeyedSingleton<IDal<GameSettings>, GameSettingsDal>(
        "gameSettings", (serviceProvider, x) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return new GameSettingsDal(configuration, "GameSettings");
        })
    .AddSingleton<CardDeckDatabase>()
    .AddSingleton<GameRestore>()
    .AddSingleton<CardDecksHolder>()
    .BuildServiceProvider();

BsonSerializer.RegisterSerializer(new JObjectBsonSerializer());

TakiGameRunner gameRunner = serviceProvider.GetRequiredService<TakiGameRunner>();

gameRunner.StartGameLoop();