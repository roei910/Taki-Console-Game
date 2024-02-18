using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Taki.Game.GameRunner
{
    internal class GameScore : IGameScore
    {
        private readonly Dictionary<string, int> scoresDictionary;
        private readonly string scoresPath;

        public GameScore(IConfiguration configuration)
        {
            scoresPath = configuration.GetSection("GameScorePath").Value ??
                throw new ArgumentNullException("Please define scores path");

            if (File.Exists(scoresPath))
            {
                var scoresString = File.ReadAllText(scoresPath);
                var scoresDict = JsonSerializer.Deserialize<Dictionary<string, int>>(scoresString);
                scoresDictionary = scoresDict ?? new Dictionary<string, int>();
                return;
            }
            scoresDictionary = new Dictionary<string, int>();
        }

        public void SetScoreByName(string name, int score)
        {
            scoresDictionary[name] = score;
        }

        //TODO: maybe connect with get score
        public bool DoesUserExist(string name)
        {
            return scoresDictionary.ContainsKey(name);
        }

        public int GetScoreByName(string name)
        {
            return scoresDictionary[name];
        }

        public void UpdateScores()
        {
            File.WriteAllText(scoresPath, JsonSerializer.Serialize(scoresDictionary));
        }
    }
}
