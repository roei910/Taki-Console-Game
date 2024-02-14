using System.Drawing;

namespace Taki.Game.Messages
{
    internal interface IUserCommunicator
    {
        void SendMessageToUser(object? message);
        string? GetMessageFromUser(object? message = null);
        void SendAlertMessage(object? message);
        string? AlertGetMessageFromUser(object? message);
        void SendErrorMessage(object? message);
        int GetNumberFromUser(object? message);
        int GetCharFromUser(object? message);
        EnumType GetEnumFromUser<EnumType>();
        Color GetColorFromUserEnum<EnumType>();
    }
}
