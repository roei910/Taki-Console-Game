using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using Taki.Models.Algorithm;
using Taki.Models.Messages;
using Taki.Serializers;
using TakiApp.Dal;
using TakiApp.Interfaces;
using TakiApp.Models;
using TakiApp.Repositories;
using TakiApp.Services.Algorithms;
using TakiApp.Services.Cards;
using TakiApp.Services.GameLogic;

//TODO: if a player finished his play we need to add in our screen what happened.

//TODO: message when player chooses a card

//TODO: the player will send CheckIns to see if he is still connected. if he doesnt reconnect for 10 secs it gets deleted, cards go back.

//TODO: add pyramid functionality

//TODO: add normal game not just online

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(x => new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .Build())
    .AddSingleton<ConstantVariables>()
    .AddSingleton<MongoDbConfig>()
    .AddSingleton<Random>()

    .AddSingleton<IAlgorithmService, AlgorithmService>()
    .AddSingleton<List<IPlayerAlgorithm>>()
    .AddSingleton<IPlayerAlgorithm, ManualPlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerAlgorithm>()

    .AddSingleton<ICardPlayService, CardPlayService>()
    .AddSingleton<List<ICardService>>()
    .AddSingleton<ICardService, NumberCard>()
    .AddSingleton<ICardService, Stop>()
    .AddSingleton<ICardService, ChangeDirection>()
    .AddSingleton<ICardService, ChangeColor>()
    .AddSingleton<ICardService, Plus>()
    .AddSingleton<ICardService, Plus2>()
    .AddSingleton<ICardService, TakiCard>()
    .AddSingleton<ICardService, SuperTaki>()
    .AddSingleton<ICardService, SwitchCardsWithDirection>()
    .AddSingleton<ICardService, SwitchCardsWithUser>()

    .AddSingleton<IDiscardPileDal, DiscardPileDal>()
    .AddSingleton<IDrawPileDal, DrawPileDal>()
    .AddSingleton<IDal<Player>, PlayersDal>()
    .AddSingleton<IDal<GameSettings>, GameSettingsDal>()

    .AddSingleton<IGameSettingsRepository, GameSettingsRepository>()
    .AddSingleton<IDrawPileRepository, DrawPileRepository>()
    .AddSingleton<IDiscardPileRepository, DiscardPileRepository>()
    .AddSingleton<IPlayersRepository, PlayersRepository>()

    .AddSingleton<IUserCommunicator, ConsoleUserCommunicator>()
    .AddSingleton<IGameInitializer, GameInitializer>()
    .AddSingleton<IGameTurnService, GameTurnService>()
    .AddSingleton<ITakiGameRunner, TakiGameRunner>()
    .BuildServiceProvider();

BsonSerializer.RegisterSerializer(new JObjectBsonSerializer());

var gameRunner = serviceProvider.GetRequiredService<ITakiGameRunner>();

await gameRunner.Run();