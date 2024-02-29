namespace Taki.Shared.Interfaces
{
    public interface IGameScore
    {
        int GetScoreByName(string name);
        void SetScoreByName(string name, int score);
        void UpdateScoresFile();
        string GetAllScores();
    }
}