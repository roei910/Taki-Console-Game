using System.Drawing;

namespace Taki.Game.Interfaces
{
    internal interface IMessageHandler
    {
        void SendErrorMessage(string message);
        void SendAlertMessage(string message);
        void SendMessageToUser(object? message);
        int GetNumberFromUser();
        string? GetMessageFromUser();
        void SendMessageToUser();
        EnumType GetEnumFromUser<EnumType>();
        Color GetColorFromUserEnum<EnumType>();
    }
}
