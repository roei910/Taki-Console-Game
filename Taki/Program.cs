using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using Taki.Factories;
using Taki.Models.Algorithm;
using Taki.Models.Messages;
using Taki.Models.GameLogic;
using Taki.Serializers;
using Taki.Data;
using Taki.Models.Deck;
using Taki.Dal;
using Taki.Shared.Interfaces;
using Taki.Shared.Models;
using Taki.Shared.Models.Dto;

//TODO: task from amit: need to add a way to communicate from other computers.
//TODO: create an option to connect many players using the local mongo connection, if a game is already on connect to it?


//TODO: check if a game exists in mongodb locally
//TODO: if the game exists then choose a name and connect the user.
//TODO: check what happends when someone exists and doesnt come back to the game
//TODO: if there isnt a game already open the computer asks to create a new local or multi computer game
//TODO: when someone connects to the game other people will get a message saying someone joined the game

//TODO: Class to handle turns
//TODO: we need to handle the turns for the players, check the first person in the list to know if it is our turn or not.
//TODO: if a player finished his play we need to add in our screen what happened.
//TODO: when someone plays the mongo gets updated from that person and he goes to the next player.
//TODO: handle the game only from mongodb, we dont need to keep models anymore


//TODO: extract functionality from MODELS

//TODO: class to handle screen updates

//TODO: if we stop a game in the middle we need to know what to do with it to continue the game, choose your name from the list

//TODO: handle the cards with services that get the card from the db and know how to handle the next turn
//TODO: the card service will get the information to know what to do next, if taki is open or something.
//TODO: create a function to know which card it needs to use (MatchCardService??)

//TODO: mongo acces will be async now
//TODO: the GameSettings need to be updated to know what is happening in the game

//TODO: in the cardService we need to add how many cards of the type in a single deck for the creation of the decks

//TODO: check to see if mongo can update me back => Watch method on the collection

//TODO: the player will send CheckIns to see if he is still connected. if he doesnt reconnect for 10 secs it gets deleted, cards go back.




var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(x => new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .Build())
    .AddSingleton<ConstantVariables>()
    .AddSingleton<MongoDbConfig>()

    .AddSingleton<IUserCommunicator, ConsoleUserCommunicator>()
    .AddSingleton<IManualPlayerAlgorithm, ManualPlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerAlgorithm>()
    .AddSingleton<IPlayerAlgorithm, PlayerHateTakiAlgo>()
    .AddTransient<List<IPlayerAlgorithm>>()
    .AddSingleton<IGameScore, GameScore>()
    .AddSingleton<PlayersHolderFactory>()
    .AddSingleton<CardDeckFactory>()
    .AddSingleton<Random>()
    .AddSingleton<TakiGameRunner>()
    .AddSingleton<IDal<PlayerDto>, PlayerDal>()
    .AddSingleton<DrawPileDal>()
    .AddSingleton<DiscardPileDal>()
    .AddSingleton<IDal<GameSettings>, GameSettingsDal>()
    .AddSingleton<ICardDeckRepository, CardDeckRepository>()
    .AddSingleton<GameRestore>()
    .AddSingleton<CardDecksHolder>()
    .BuildServiceProvider();

BsonSerializer.RegisterSerializer(new JObjectBsonSerializer());

TakiGameRunner gameRunner = serviceProvider.GetRequiredService<TakiGameRunner>();

gameRunner.StartGameLoop();