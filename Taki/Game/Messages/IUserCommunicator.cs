using System.Drawing;

namespace Taki.Game.Messages
{
    internal interface IUserCommunicator
    {
        void SendMessageToUser(object? message = null);
        string? GetMessageFromUser(object? message = null);
        void SendAlertMessage(object? message);
        string? AlertGetMessageFromUser(object? message);
        void SendErrorMessage(object? message);
        int GetNumberFromUser(object? message = null);
        int GetCharFromUser(object? message);
        EnumType GetEnumFromUser<EnumType>();
        EnumType GetEnumFromUser<EnumType>(List<EnumType> excludedOptions);
        Color GetColorFromUserEnum<EnumType>();
    }
}
