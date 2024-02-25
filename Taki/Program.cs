using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Taki;
using MongoDB.Bson.Serialization;
using Taki.Database;
using Taki.Dto;
using Taki.Factories;
using Taki.Interfaces;
using Taki.Models.Algorithm;
using Taki.Models.Messages;
using Taki.Models.GameLogic;
using Taki.Models.Serializers;
using Taki.Repository;

//TODO: update saving while doing special cards (Taki etc)
//TODO: create classes to deal with restoring and maybe also saving to the database
//TODO: better saving and deleting from db:
//in player => allow the playersHolder to remove the player when he is done and add again
//in decks => decksHolder will remove the cards he removed and add them back where needed if needed

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
    .AddSingleton<IDal<PlayerDto>, PlayerDatabase>()
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
    .AddSingleton<TakiGameDatabaseHolder>()
    .BuildServiceProvider();

BsonSerializer.RegisterSerializer(new JObjectBsonSerializer());

TakiGameRunner gameRunner = serviceProvider.GetRequiredService<TakiGameRunner>();

gameRunner.StartGameLoop();