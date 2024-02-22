using System.Drawing;
using Taki.Interfaces;

namespace Taki.Models.Messages
{
    internal class ConsoleUserCommunicator : IUserCommunicator
    {
        private void SendColorMessageToUser(ConsoleColor color, object? message)
        {
            Console.ForegroundColor = color;
            SendMessageToUser(message);
            Console.ForegroundColor = ConsoleColor.White;
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

        public T GetEnumFromUser<T>()
        {
            T[] values = (T[])Enum.GetValues(typeof(T));

            return GetUserEnumFromArray(values);
        }

        public Color GetColorFromUserEnum<EnumType>()
        {
            string? enumString = (GetEnumFromUser<EnumType>()?.ToString()) ?? throw
                new ArgumentNullException("error trying to get color");
            Color color = Color.FromName(enumString);
            return color;
        }

        public string? GetMessageFromUser(object? message)
        {
            if (message != null)
                SendMessageToUser(message);

            string? answer = Console.ReadLine();
            Console.WriteLine();

            return answer;
        }

        public int GetCharFromUser(object? message)
        {
            Console.WriteLine(message);
            int answer = Console.Read();
            Console.WriteLine();

            return answer;
        }

        public int GetNumberFromUser(object? message)
        {
            Console.WriteLine(message);

            int number;

            while (!int.TryParse(Console.ReadLine(), out number))
                SendErrorMessage($"Please enter a valid number");

            Console.WriteLine();

            return number;
        }

        public string? AlertGetMessageFromUser(object? message)
        {
            SendAlertMessage(message);
            return Console.ReadLine();
        }

        public T GetEnumFromUser<T>(List<T> excludedOptions)
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            return GetUserEnumFromArray(values.Where(value => !excludedOptions.Contains(value)).ToArray());
        }

        private T GetUserEnumFromArray<T>(T[] values)
        {
            SendMessageToUser("Please choose the type by index:");

            _ = values
                .Select((i, value) =>
                {
                    SendMessageToUser($"{i}. {value}");

                    return i;
                }).ToList();

            if (!int.TryParse(Console.ReadLine(), out int index) ||
                index >= values.Length || index < 0)
            {
                SendErrorMessage("please choose valid index\n");
                return GetUserEnumFromArray(values);
            }

            Console.WriteLine();
            return values[index];
        }

        public void SendColorMessageToUser(Color color, object? message)
        {
            SendColorMessageToUser(ColorToConsoleColor(color), message);
            Console.WriteLine();
        }

        private ConsoleColor ColorToConsoleColor(Color color)
        {
            if (color.Equals(Color.Red))
                return ConsoleColor.Red;
            if (color.Equals(Color.Green))
                return ConsoleColor.Green;
            if (color.Equals(Color.Blue))
                return ConsoleColor.Blue;
            if (color.Equals(Color.Yellow))
                return ConsoleColor.Yellow;
            return ConsoleColor.White;
        }
    }
}
