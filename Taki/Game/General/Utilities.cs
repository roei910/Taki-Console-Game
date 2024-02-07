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
        protected static Communicator communicator = Communicator.GetCommunicator();
        public static void PrintEnumValues<T>()
        {
            if (Enum.GetValues(typeof(T)).Length <= 0)
                throw new ArgumentException("Not an enum");
            T[] actions = (T[])Enum.GetValues(typeof(T));
            Communicator.PrintMessage("Please choose an action by index");
            for (int i = 0; i < actions.Length; i++)
                Communicator.PrintMessage($"{i}. {actions[i]}");
        }

        public static T GetUserEnum<T>()
        {
            object ?action;
            while (!Enum.TryParse(typeof(T), Communicator.ReadMessage(), out action) ||
                action == null || !Enum.IsDefined(typeof(T), action))
                Communicator.PrintMessage("please enum again");
            return (T)action;
        }

        public static T GetEnumFromUser<T>(string message = "", int defaultIndex = -1)
        {
            T[] values = (T[])Enum.GetValues(typeof(T));

            if(message == "")
                Communicator.PrintMessage("Please choose the type by index:");
            else
                Communicator.PrintMessage($"Please choose the type {message} by index:");

            for (int i = 0; i < values.Length; i++)
                Communicator.PrintMessage($"{i}. {values[i]}");

            _ = int.TryParse(Communicator.ReadMessage(), out int index);

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
            string? enumString = (GetEnumFromUser<EnumType>(message, defaultIndex)?.ToString()) ?? throw 
                new ArgumentNullException("error trying to get color");
            Color color = Color.FromName(enumString);
            return color;
        }
    }
}
