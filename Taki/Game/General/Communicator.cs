using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taki.Game.General
{
    internal class Communicator
    {
        private static Communicator? communicator;

        public enum MessageType
        {
            Normal,
            Alert,
            Error
        }

        private Communicator() { }

        public static Communicator GetCommunicator()
        {
            communicator ??= new Communicator();
            return communicator;
        }

        public void PrintMessage(string message = "", MessageType type = MessageType.Normal)
        {
            switch (type)
            {
                case MessageType.Normal:
                    Console.WriteLine(message);
                    break;
                case MessageType.Alert:
                    PrintMessageColor(ConsoleColor.Yellow, message);
                    break;
                case MessageType.Error:
                    PrintMessageColor(ConsoleColor.Red, message);
                    break;
                default:
                    throw new Exception("Unknown message type");
            }
        }

        public void PrintMessage(object message, MessageType type = MessageType.Normal)
        {
            PrintMessage(message.ToString() ?? "", type);
        }

        public string? ReadMessage()
        {
            return Console.ReadLine();
        }

        private static void PrintMessageColor(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
