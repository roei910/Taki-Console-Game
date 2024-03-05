using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using Taki.Models.Algorithm;
using Taki.Models.Messages;
using Taki.Serializers;
using TakiApp.Dal;
using TakiApp.Factories;
using TakiApp.Interfaces;
using TakiApp.Models;
using TakiApp.Repositories;
using TakiApp.Services.Cards;
using TakiApp.Services.GameLogic;
using TakiApp.Services.Players;

//TODO: check what happends when someone exists and doesnt come back to the game
//TODO: when someone connects to the game other people will get a message saying someone joined the game

//TODO: if a player finished his play we need to add in our screen what happened.

//TODO: if we stop a game in the middle we need to know what to do with it to continue the game, choose your name from the list

//TODO: the player will send CheckIns to see if he is still connected. if he doesnt reconnect for 10 secs it gets deleted, cards go back.

//TODO: change the way i keep the order. do order by everywhere, including cards



var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(x => new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .Build())
    .AddSingleton<ConstantVariables>()
    .AddSingleton<MongoDbConfig>()
    .AddSingleton<Random>()

    .AddSingleton<IPlayerService, PlayerService>()
    .AddSingleton<List<IPlayerAlgorithm>>()
    .AddSingleton<IPlayerAlgorithm, ManualPlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerAlgorithm>()

    .AddSingleton<ICardPlayService , CardPlayService>()
    .AddSingleton<List<ICardService>>()
    .AddSingleton<ICardService, ChangeColor>()
    .AddSingleton<ICardService, ChangeDirection>()
    .AddSingleton<ICardService, NumberCard>()
    .AddSingleton<ICardService, Plus>()
    .AddSingleton<ICardService, Plus2>()
    .AddSingleton<ICardService, TakiCard>()
    .AddSingleton<ICardService, Stop>()
    //.AddSingleton<ICardService, SuperTaki>()
    .AddSingleton<CardsFactory>()

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