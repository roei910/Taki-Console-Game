using Taki.Game.Communicators;
using Taki.Game.Managers;
using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Algorithm;
using Taki.Game.Factories;
using Taki.Game.Messages;
using Microsoft.Extensions.Configuration;
using Taki;

//TODO: organizine blanks and general code 

//TODO: change handler names if doesnt fit
//TODO: error with when drawing after finishing hand in pyramid => check
//TODO: error with switch cards when putting another color on top is allowed (shouldnt work)
//TODO: maybe move players to game runner and change players handler to something else (move handler???)
//TODO: maybe save instances of all handlers inside the code????

var serviceProvider = new ServiceCollection()
    .AddSingleton<IUserCommunicator, ConsoleUserCommunicator>()
    .AddScoped<IPlayerAlgorithm, PlayerAlgorithm>()
    .AddScoped<IPlayerAlgorithm, PlayerHateTakiAlgo>()
    .AddScoped<List<IPlayerAlgorithm>>()
    .AddSingleton<ManualPlayerAlgorithm>()
    .AddSingleton<PlayersHandlerFactory>()
    .AddSingleton<CardsHandlerFactory>()
    .AddScoped<IConfiguration>(x => new ConfigurationBuilder()
        .AddJsonFile("AppConfigurations.json", false, true)
        .Build())
    .AddScoped<ProgramVariables>()
    .AddScoped<TakiGameGenerator>()
    .BuildServiceProvider();

TakiGameGenerator gameFactory = new(serviceProvider);
TakiGameRunner gameRunner = gameFactory.ChooseTypeOfGame();

gameRunner.StartGame();