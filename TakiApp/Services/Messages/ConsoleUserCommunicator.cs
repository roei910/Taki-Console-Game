using TakiApp.Shared.Interfaces;

namespace Taki.Models.Messages
{
    public class ConsoleUserCommunicator : IUserCommunicator
    {
        public ConsoleUserCommunicator() 
        {
            Console.Clear();
            Console.WriteLine("Welcome to the taki game");
            Console.WriteLine();
        }

        public void SendErrorMessage(object? message)
        {
            SendColorMessageToUser(ConsoleColor.Red, message?.ToString() ?? "");
        }

        public void SendAlertMessage(object? message)
        {
            SendColorMessageToUser(ConsoleColor.Yellow, message?.ToString() ?? "");
        }

        public void SendMessageToUser(object? message)
        {
            Console.WriteLine(message);
        }

        public string? GetMessageFromUser(object? message)
        {
            if (message != null)
                SendMessageToUser(message);

            string? answer = Console.ReadLine();
            Console.WriteLine();

            return answer;
        }

        public int GetNumberFromUser(object? message = null)
        {
            if(message is not null)
                Console.WriteLine(message);

            int number;

            while (!int.TryParse(Console.ReadLine(), out number))
                SendErrorMessage($"Please enter a valid number");

            Console.WriteLine();

            return number;
        }

        private void SendColorMessageToUser(ConsoleColor color, object? message)
        {
            Console.ForegroundColor = color;
            SendMessageToUser(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public T UserPickItemFromList<T>(List<T> values, Func<T, string>? toString = null, bool printPrompt = false)
        {
            string message = string.Join('\n', values.Select((val, i) => $"{i + 1}. {toString?.Invoke(val) ?? val?.ToString()}"));

            if(printPrompt)
                SendAlertMessage("Please pick by index from list by index:");
            SendAlertMessage(message);

            int number = GetNumberFromUser();

            while(number > values.Count || number < 1)
                number = GetNumberFromUser("Please choose an index from the list");

            return values[number - 1];
        }

        public int GetNumberFromUser(object? message, int minNumber, int maxNumber)
        {
            int number = GetNumberFromUser($"{message}, number between {minNumber} and {maxNumber}");

            if (number < minNumber || number > maxNumber)
                number = GetNumberFromUser("Please enter a valid number", minNumber, maxNumber);

            return number;
        }
    }
}