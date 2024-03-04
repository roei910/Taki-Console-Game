using System.Drawing;

namespace TakiApp.Interfaces
{
    public interface IUserCommunicator
    {
        string? AlertGetMessageFromUser(object? message);
        int GetCharFromUser(object? message);
        T GetTypeFromUser<T>(List<T> values, Func<T, string>? toString = null);
        string? GetMessageFromUser(object? message = null);
        int GetNumberFromUser(object? message);
        void SendAlertMessage(object? message);
        void SendColorMessageToUser(Color color, object? message);
        void SendErrorMessage(object? message);
        void SendMessageToUser(object? message);
    }
}