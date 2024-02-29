using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taki.Models.Algorithm;
using TakiApp.Dal;
using TakiApp.Interfaces;
using TakiApp.Models;
using TakiApp.Services.Cards;
using TakiApp.Services.GameLogic;
using TakiApp.Services.Players;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(x => new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .Build())
    .AddSingleton<ConstantVariables>()
    .AddSingleton<MongoDbConfig>()

    .AddSingleton<List<IPlayerAlgorithm>>()
    .AddSingleton<IPlayerAlgorithm, ManualPlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerAlgorithm>()

    .AddSingleton<List<ICardService>>()
    .AddSingleton<ICardService, ChangeColor>()
    .AddSingleton<ICardService, ChangeDirection>()
    .AddSingleton<ICardService, NumberCard>()
    .AddSingleton<ICardService, Plus>()
    .AddSingleton<ICardService, Plus2>()
    .AddSingleton<ICardService, SuperTaki>()
    .AddSingleton<ICardService, TakiCard>()

    .AddSingleton<IPlayerService, PlayerService>()

    .AddSingleton<IDal<Player>, PlayersDal>()
    .AddSingleton<IDal<Card>, DiscardPileDal>()
    .AddSingleton<IDal<Card>, DrawPileDal>()
    .AddSingleton<IDal<GameSettings>, GameSettingsDal>()

    .AddSingleton<IUserConnectionService, UserConnectionService>()
    .AddSingleton<IGameInitializer, GameInitializer>()
    .AddSingleton<ITakiGameRunner, TakiGameRunner>()
    .BuildServiceProvider();

var gameRunner = serviceProvider.GetRequiredService<ITakiGameRunner>();

gameRunner.Run();