using Taki.Game.Communicators;
using Taki.Game.Managers;
using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Algorithm;
using Taki.Game.Factories;
using Taki.Game.Messages;
using Microsoft.Extensions.Configuration;
using Taki;

//TODO: organizine blanks and general code 
//TODO: Avoid static as possible

//TODO: remove game handlers => put all in service provider

//TODO: change handler names if doesnt fit
//TODO: error with when drawing after finishing hand in pyramid => check
//TODO: error with switch cards when putting another color on top is allowed (shouldnt work)
//TODO: check Iconfiguration and get configuration using JSON file
//TODO: maybe move players to game runner and change players handler to something else (move handler???)
//TODO: maybe save instances of all handlers inside the code????

//TODO: remove
//IConfiguration configuration = new ConfigurationBuilder()
//    .AddJsonFile("AppConfigurations.json", false, true)
//    .Build();

var serviceProvider = new ServiceCollection()
    .AddSingleton<IUserCommunicator, ConsoleUserCommunicator>()
    .AddScoped<IPlayerAlgorithm, PlayerAlgorithm>()
    .AddScoped<IPlayerAlgorithm, PlayerHateTakiAlgo>()
    .AddScoped<List<IPlayerAlgorithm>>()
    .AddSingleton<ManualPlayerAlgorithm>()
    .AddSingleton<PlayersHandlerFactory>()
    .AddSingleton<CardsHandlerFactory>()
    .AddScoped<Random>()
    .AddScoped<IConfiguration>(x => new ConfigurationBuilder()
        .AddJsonFile("AppConfigurations.json", false, true)
        .Build())
    .AddScoped<ProgramVariables>()
    .AddScoped<TakiGameGenerator>()
    .BuildServiceProvider();

TakiGameGenerator gameFactory = new(serviceProvider);
TakiGameRunner gameRunner = gameFactory.ChooseTypeOfGame();

gameRunner.StartGame();