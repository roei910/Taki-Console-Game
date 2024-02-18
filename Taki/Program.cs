using Taki.Game.Communicators;
using Taki.Game.Managers;
using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Algorithm;
using Taki.Game.Factories;
using Taki.Game.Messages;
using Microsoft.Extensions.Configuration;
using Taki;
using Taki.Game.Deck;
using Taki.Game.GameRunner;

//TODO: extract the players choosing and add as function

/*
 * TODO: add a section in the code where it is adding the user and searching
 * in a json file to see if the user already exists under the same name and ask 
 * if he wants to take his current score,
 * when someone wins the scores will update in the json file(add!!! not remove anything)
 */

/*
 * TODO: make loop to restart game if the player wants to
 * add generic message to ask for something from user
 * maybe add to the game runner => restart functionality
 */

var serviceProvider = new ServiceCollection()
    .AddSingleton<IUserCommunicator, ConsoleUserCommunicator>()
    .AddSingleton<IPlayerAlgorithm, PlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerHateTakiAlgo>()
    .AddSingleton<ManualPlayerAlgorithm>()
    .AddSingleton<PlayersHolderFactory>()
    .AddSingleton<CardDeckFactory>()
    .AddSingleton<Random>()
    .AddSingleton<List<IPlayerAlgorithm>>()
    .AddSingleton<IConfiguration>(x => new ConfigurationBuilder()
        .AddJsonFile("AppConfigurations.json", false, true)
        .Build())
    .AddSingleton<ProgramVariables>()
    .AddSingleton<TakiGameGenerator>()
    .AddSingleton<ICardDecksHolder, CardDecksHolder>()
    .AddSingleton<TakiGameGenerator>()
    .AddSingleton<IGameScore, GameScore>()
    .BuildServiceProvider();

IUserCommunicator userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
TakiGameGenerator gameGenerator = serviceProvider.GetRequiredService<TakiGameGenerator>();

TakiGameRunner gameRunner = gameGenerator.ChooseTypeOfGame();

while (true)
{
    gameRunner.StartGame();

    var answer = userCommunicator.AlertGetMessageFromUser("y to restart the game");
    if (answer != "y")
        break;
}


