using System.Drawing;
using Taki.Game.Interfaces;

namespace Taki.Game.Communicators
{
    internal class ConsoleMessageHandler : IMessageHandler
    {
        public string? GetMessageFromUser()
        {
            return Console.ReadLine();
        }

        private void PrintMessageColor(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            SendMessageToUser(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void SendErrorMessage(string message)
        {
            PrintMessageColor(ConsoleColor.Red, message);
        }

        public void SendAlertMessage(string message)
        {
            PrintMessageColor(ConsoleColor.Yellow, message);
        }

        public void SendMessageToUser(object? message)
        {
            Console.WriteLine(message);
        }

        public int GetNumberFromUser()
        {
            int numberOfPlayers;

            while (!int.TryParse(GetMessageFromUser(), out numberOfPlayers))
                SendMessageToUser($"Please enter a valid number");

            return numberOfPlayers;
        }

        public void SendMessageToUser()
        {
            SendMessageToUser("");
        }

        public T GetEnumFromUser<T>()
        {
            T[] values = (T[])Enum.GetValues(typeof(T));

            SendMessageToUser("Please choose the type by index:");

            Enumerable.Range(0, values.Length)
                .Select(i =>
                {
                    SendMessageToUser($"{i}. {values[i]}");
                    return i;
                }).ToList();

            if(!int.TryParse(GetMessageFromUser(), out int index) || 
                index >= values.Length || index < 0)
                return GetEnumFromUser<T>();

            return values[index];
        }

        public Color GetColorFromUserEnum<EnumType>()
        {
            string? enumString = (GetEnumFromUser<EnumType>()?.ToString()) ?? throw
                new ArgumentNullException("error trying to get color");
            Color color = Color.FromName(enumString);
            return color;
        }
    }
}
