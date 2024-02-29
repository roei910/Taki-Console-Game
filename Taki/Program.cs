using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
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

//TODO: task from amit: need to add a way to communicate from other computers.

//TODO: from tomer: extract models from services

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(x => new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .Build())
    .AddSingleton<ConstantVariables>()
    .AddSingleton<MongoDbConfig>()

    .AddSingleton<IUserCommunicator, ConsoleUserCommunicator>()
    .AddSingleton<IManualPlayerAlgorithm, ManualPlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerHateTakiAlgo>()
    .AddTransient<List<IPlayerAlgorithm>>()
    .AddSingleton<IGameScore, GameScore>()
    .AddSingleton<PlayersHolderFactory>()
    .AddSingleton<CardDeckFactory>()
    .AddSingleton<Random>()
    .AddSingleton<TakiGameRunner>()
    .AddSingleton<IDal<PlayerDto>, PlayerDal>()
    .AddSingleton<DrawPileDal>()
    .AddSingleton<DiscardPileDal>()
    .AddSingleton<IDal<GameSettings>, GameSettingsDal>()
    .AddSingleton<ICardDeckRepository, CardDeckRepository>()
    .AddSingleton<GameRestore>()
    .AddSingleton<CardDecksHolder>()
    .BuildServiceProvider();

BsonSerializer.RegisterSerializer(new JObjectBsonSerializer());

TakiGameRunner gameRunner = serviceProvider.GetRequiredService<TakiGameRunner>();

gameRunner.StartGameLoop();