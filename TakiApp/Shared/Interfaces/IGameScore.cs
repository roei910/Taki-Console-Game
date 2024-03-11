namespace TakiApp.Shared.Interfaces
{
    public interface IGameScore
    {
        string GetAllScores();
        int GetScoreByName(string name);
        void SetScoreByName(string name, int score);
        void UpdateScoresFile();
    }
}