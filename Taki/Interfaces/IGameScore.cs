namespace Taki.Interfaces
{
    internal interface IGameScore
    {
        int GetScoreByName(string name);
        void SetScoreByName(string name, int score);
        void UpdateScoresFile();
        string GetAllScores();
    }
}