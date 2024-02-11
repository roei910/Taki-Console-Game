using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using Taki.Game.Interfaces;

namespace Taki.Game.General
{
    internal class Utilities
    {
        private readonly IMessageHandler _messageHandler;

        public Utilities(IServiceProvider serviceProvider)
        {
            _messageHandler = serviceProvider.GetRequiredService<IMessageHandler>();
        }

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
