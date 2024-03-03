using System.Drawing;

namespace TakiApp.Interfaces
{
    public interface IUserCommunicator
    {
        string? AlertGetMessageFromUser(object? message);
        int GetCharFromUser(object? message);
        Color GetColorFromUserEnum<EnumType>();
        EnumType GetEnumFromUser<EnumType>();
        T GetEnumFromUser<T>(List<T> excludedOptions);
        string? GetMessageFromUser(object? message);
        int GetNumberFromUser(object? message);
        void SendAlertMessage(object? message);
        void SendColorMessageToUser(Color color, object? message);
        void SendErrorMessage(object? message);
        void SendMessageToUser(object? message);
    }
}