using System.Drawing;

namespace TakiApp.Interfaces
{
    public interface IUserCommunicator
    {
        string? AlertGetMessageFromUser(object? message);
        int GetCharFromUser(object? message);
        T UserPickItemFromList<T>(List<T> values, Func<T, string>? toString = null, bool printPrompt = false);
        string? GetMessageFromUser(object? message = null);
        int GetNumberFromUser(object? message);
        int GetNumberFromUser(object? message, int minNumber, int maxNumber);
        void SendAlertMessage(object? message);
        void SendColorMessageToUser(Color color, object? message);
        void SendErrorMessage(object? message);
        void SendMessageToUser(object? message);
    }
}