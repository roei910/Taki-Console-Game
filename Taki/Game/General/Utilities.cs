using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Managers;

namespace Taki.Game.General
{
    internal class Utilities
    {
        public static void PrintEnumValues<T>()
        {
            if (Enum.GetValues(typeof(T)).Length <= 0)
                throw new ArgumentException("Not an enum");
            T[] actions = (T[])Enum.GetValues(typeof(T));
            Console.WriteLine("Please choose an action by index");
            for (int i = 0; i < actions.Length; i++)
                Console.WriteLine($"{i}. {actions[i]}");
        }

        public static T GetUserEnum<T>()
        {
            object ?action;
            while (!Enum.TryParse(typeof(T), Console.ReadLine(), out action) ||
                action == null || !Enum.IsDefined(typeof(T), action))
                Console.WriteLine("please enum again");
            return (T)action;
        }

        public static T GetEnumFromUser<T>(string message = "", int defaultIndex = -1)
        {
            T[] values = (T[])Enum.GetValues(typeof(T));

            if(message == "")
                Console.WriteLine("Please choose the type by index:");
            else
                Console.WriteLine($"Please choose the type {message} by index:");

            for (int i = 0; i < values.Length; i++)
                Console.WriteLine($"{i}. {values[i]}");

            int index;
            int.TryParse(Console.ReadLine(), out index);

            if (index >= values.Length || index < 0)
            {
                if (defaultIndex != -1)
                    return values[defaultIndex];
                else
                    return GetEnumFromUser<T>();
            }

            return values[index];
        }

        public static Color GetColorFromUserEnum<EnumType>(string message = "", int defaultIndex = -1)
        {
            string ?enumString = GetEnumFromUser<EnumType>(message, defaultIndex)?.ToString();
            if (enumString == null)
                throw new ArgumentNullException("error trying to get color");
            Color color = Color.FromName(enumString);
            return color;
        }

        public static void PrintConsoleError(string errorMessage)
        {
            PrintMessageColor(ConsoleColor.Red, errorMessage);
        }

        public static void PrintConsoleAlert(string inputMessage)
        {
            PrintMessageColor(ConsoleColor.Yellow, inputMessage);
        }

        private static void PrintMessageColor(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
