﻿using System.Drawing;
using Taki.Game.Messages;

namespace Taki.Game.Communicators
{
    internal class ConsoleUserCommunicator : IUserCommunicator
    {
        private void PrintMessageColor(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            SendMessageToUser(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void SendErrorMessage(object? message)
        {
            PrintMessageColor(ConsoleColor.Red, message?.ToString() ?? "");
        }

        public void SendAlertMessage(object? message)
        {
            PrintMessageColor(ConsoleColor.Yellow, message?.ToString() ?? "");
        }

        public void SendMessageToUser(object? message)
        {
            Console.WriteLine(message);
        }

        public T GetEnumFromUser<T>()
        {
            T[] values = (T[])Enum.GetValues(typeof(T));

            SendMessageToUser("Please choose the type by index:");

            _ = values
                .Select((i, value) =>
                {
                    SendMessageToUser($"{i}. {value}");
                    return i;
                }).ToList();

            if(!int.TryParse(Console.ReadLine(), out int index) || 
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

        public string? GetMessageFromUser(object? message)
        {
            SendMessageToUser(message);
            return Console.ReadLine();
        }

        public int GetCharFromUser(object? message)
        {
            Console.WriteLine(message);
            return Console.Read();
        }

        public int GetNumberFromUser(object? message)
        {
            Console.WriteLine(message);

            int number;

            while (!int.TryParse(Console.ReadLine(), out number))
                SendErrorMessage($"Please enter a valid number");

            return number;
        }

        public string? AlertGetMessageFromUser(object? message)
        {
            SendAlertMessage(message);
            return Console.ReadLine();
        }
    }
}