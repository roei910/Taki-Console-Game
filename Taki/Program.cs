using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.General;
using Taki.Game.Managers;

GameManagerFactory factory = new();
GameManager manager;
GameTypeEnum typeOfGame = GameManagerFactory.GetGameType();

switch (typeOfGame)
{
    case GameTypeEnum.Normal:
        manager = factory.CreateNormal();
        communicator.PrintMessage(Communicator.MessageType.Normal, )
        Console.WriteLine();
        break;
    case GameTypeEnum.Pyramid:
        manager = factory.CreatePyramid();
        Console.WriteLine("Starting a new game of TAKI pyramid edition!");
        break;
    default:
        throw new Exception("type enum was wrong");
}

manager.StartGame();
