﻿using Taki.Game.Communicators;
using Taki.Game.Managers;
using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Algorithm;
using Taki.Game.Factories;
using Taki.Game.Messages;
using Microsoft.Extensions.Configuration;
using Taki;
using Taki.Game.GameRunner;

//TODO: replace print cards with the new print cards function
//TODO: check if restart game keeps the scores
//TODO: make it possible to print many cards in a row of same color

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
    .BuildServiceProvider();

TakiGameRunner gameRunner = serviceProvider.GetRequiredService<TakiGameRunner>();

gameRunner.StartGameLoop();