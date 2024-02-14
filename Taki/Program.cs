using Taki.Game.Communicators;
using Taki.Game.Managers;
using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Algorithm;
using Taki.Game.Factories;
using Taki.Game.Messages;
using Microsoft.Extensions.Configuration;
using Taki;
using Taki.Game.Deck;

//TODO: organizine blanks and general code 

//TODO: change handler names if doesnt fit
//TODO: error when drawing after finishing hand in pyramid => check
//TODO: error with switch cards when putting another color on top is allowed (shouldnt work)
//TODO: maybe move players to game runner and change players handler to something else (move handler???)
//TODO: maybe save instances of all handlers inside the code????

//TODO: check why singleton or scoped!!!

var serviceProvider = new ServiceCollection()
    .AddSingleton<IUserCommunicator, ConsoleUserCommunicator>()
    .AddSingleton<IPlayerAlgorithm, PlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerHateTakiAlgo>()
    .AddSingleton<ManualPlayerAlgorithm>()
    .AddSingleton<PlayersHandlerFactory>()
    .AddSingleton<CardDeckFactory>()
    .AddSingleton<Random>()
    .AddScoped<List<IPlayerAlgorithm>>()
    .AddScoped<IConfiguration>(x => new ConfigurationBuilder()
        .AddJsonFile("AppConfigurations.json", false, true)
        .Build())
    .AddScoped<ProgramVariables>()
    .AddScoped<TakiGameGenerator>()
    .AddScoped<ICardDecksHolder, CardDecksHolder>()
    .BuildServiceProvider();

TakiGameGenerator gameFactory = new(serviceProvider);
TakiGameRunner gameRunner = gameFactory.ChooseTypeOfGame();

gameRunner.StartGame();