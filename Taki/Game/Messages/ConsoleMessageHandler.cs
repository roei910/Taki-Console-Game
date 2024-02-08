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

        public void SendMessageToUser(string message)
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
            SendMessageToUser();
        }
    }
}
