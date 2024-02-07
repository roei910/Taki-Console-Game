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

        public void PrintMessage(MessageType type, string message)
        {
            Console.WriteLine(message);
        }

        public string? ReadMessage()
        {
            return Console.ReadLine();
        }
    }
}
