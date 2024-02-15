﻿using Taki.Game.Communicators;
using Taki.Game.Managers;
using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Algorithm;
using Taki.Game.Factories;
using Taki.Game.Messages;
using Microsoft.Extensions.Configuration;
using Taki;
using Taki.Game.Deck;

//TODO: organizine blanks and general code 

//TODO: error when drawing after finishing hand in pyramid => check
//TODO: error with switch cards when putting another color on top is allowed (shouldnt work)

//TODO: check why singleton or scoped!!!

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
    .BuildServiceProvider();

TakiGameGenerator gameFactory = new(serviceProvider);
TakiGameRunner gameRunner = gameFactory.ChooseTypeOfGame();

gameRunner.StartGame();