namespace Taki.Game.GameRunner
{
    internal interface IGameScore
    {
        bool DoesUserExist(string name);
        int GetScoreByName(string name);
        void SetScoreByName(string name, int score);
        void UpdateScores();
        string GetAllScores();
    }
}