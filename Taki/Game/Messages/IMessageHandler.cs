namespace Taki.Game.Communicators
{
    internal interface IMessageHandler
    {
        void SendErrorMessage(string message);
        void SendAlertMessage(string message);
        void SendMessageToUser(string message);
        int GetNumberFromUser();
        string? GetMessageFromUser();
        void SendMessageToUser();
    }
}
