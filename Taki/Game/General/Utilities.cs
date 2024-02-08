using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using Taki.Game.Communicators;

namespace Taki.Game.General
{
    internal class Utilities
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageHandler _messageHandler;

        public Utilities(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _messageHandler = serviceProvider.GetRequiredService<IMessageHandler>();
        }
        public void PrintEnumValues<T>()
        {
            if (Enum.GetValues(typeof(T)).Length <= 0)
                throw new ArgumentException("Not an enum");
            T[] actions = (T[])Enum.GetValues(typeof(T));
            _messageHandler.SendMessageToUser("Please choose an action by index");
            for (int i = 0; i < actions.Length; i++)
                _messageHandler.SendMessageToUser($"{i}. {actions[i]}");
        }

        //public T GetUserEnum<T>()
        //{
        //    object? action;
        //    while (!Enum.TryParse(typeof(T), _messageHandler.GetMessageFromUser(), out action) ||
        //        action == null || !Enum.IsDefined(typeof(T), action))
        //        _messageHandler.SendMessageToUser("please enum again");
        //    return (T)action;
        //}

        public T GetEnumFromUser<T>(string message = "", int defaultIndex = -1)
        {
            T[] values = (T[])Enum.GetValues(typeof(T));

            if (message == "")
                _messageHandler.SendMessageToUser("Please choose the type by index:");
            else
                _messageHandler.SendMessageToUser($"Please choose the type {message} by index:");

            for (int i = 0; i < values.Length; i++)
                _messageHandler.SendMessageToUser($"{i}. {values[i]}");

            _ = int.TryParse(_messageHandler.GetMessageFromUser(), out int index);

            if (index >= values.Length || index < 0)
            {
                if (defaultIndex != -1)
                    return values[defaultIndex];
                else
                    return GetEnumFromUser<T>();
            }

            return values[index];
        }

        public Color GetColorFromUserEnum<EnumType>(string message = "", int defaultIndex = -1)
        {
            string? enumString = (GetEnumFromUser<EnumType>(message, defaultIndex)?.ToString()) ?? throw
                new ArgumentNullException("error trying to get color");
            Color color = Color.FromName(enumString);
            return color;
        }
    }
}
