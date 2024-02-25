using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Taki.Interfaces;

namespace Taki.Models.GameLogic
{
    internal class GameScore : IGameScore
    {
        private readonly Dictionary<string, int> scoresDictionary;
        private readonly string scoresPath;
        public const int NO_SCORE = -1;

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

        public int GetScoreByName(string name)
        {
            if (!scoresDictionary.TryGetValue(name, out int value))
                return NO_SCORE;
            return value;
        }

        public void UpdateScoresFile()
        {
            File.WriteAllText(scoresPath, JsonSerializer.Serialize(scoresDictionary));
        }

        public string GetAllScores()
        {
            var scores = scoresDictionary.ToList()
                .Select(keyValPair => $"Name: {keyValPair.Key}, Score: {keyValPair.Value}").ToList();
            return string.Join("\n", scores);
        }
    }
}
