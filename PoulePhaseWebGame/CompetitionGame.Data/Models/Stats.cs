using Newtonsoft.Json;
using System.Text.Json;

namespace CompetitionGame.Data.Models
{
    public class Stats
    {
        [JsonProperty("gamesPlayed")]
        public int GamesPlayed;
        [JsonProperty("goalsMade")]
        public int GoalsMade;
        [JsonProperty("goalsConceded")]
        public int GoalsConceded;

        public float AttackStrength => (float)GoalsMade / (float)GamesPlayed;
        public float DefenseStrength => (float)GoalsConceded / (float)GamesPlayed;
    }

}