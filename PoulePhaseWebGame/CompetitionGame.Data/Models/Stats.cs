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

        public decimal AttackStrength =>  (decimal)GoalsMade / (decimal)GamesPlayed;
        public decimal DefenseStrength => (decimal)GoalsConceded / (decimal)GamesPlayed;
    }

}