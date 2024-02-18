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

//TODO: check exception ewhen restarting, after reset there was only 2 cards in deck: 1 null, cards are being deleted somehow
//TODO: check the plus card is okay
//TODO: check if shuffling works well
//TODO: when drawing / not drawing sometimes the message is not correct => should be -1 to finish (in taki e.g)
//TODO: check the id's of players => should not restart from high number
//TODO: sometimes writes twice the message for restart => check why
//TODO: add option to show all of the scores for every player in db
//TODO: check taki stops well and goes to next player

var serviceProvider = new ServiceCollection()
    .AddSingleton<IUserCommunicator, ConsoleUserCommunicator>()
    .AddSingleton<IPlayerAlgorithm, PlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerHateTakiAlgo>()
    .AddSingleton<List<IPlayerAlgorithm>>()
    .AddSingleton<ICardDecksHolder, CardDecksHolder>()
    .AddSingleton<IGameScore, GameScore>()
    .AddSingleton<ManualPlayerAlgorithm>()
    .AddSingleton<PlayersHolderFactory>()
    .AddSingleton<CardDeckFactory>()
    .AddSingleton<ProgramVariables>()
    .AddSingleton<Random>()
    .AddSingleton<IConfiguration>(x => new ConfigurationBuilder()
        .AddJsonFile("AppConfigurations.json", false, true)
        .Build())
    .BuildServiceProvider();

TakiGameRunner gameRunner = new (serviceProvider);

gameRunner.StartGameLoop();