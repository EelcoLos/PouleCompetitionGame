using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CompetitionGame
{
    public class Team
    {
        [JsonProperty("teamName")]
        public string TeamName;
        [JsonProperty("homeStats")]
        public Stats HomeStats;
        [JsonProperty("awayStats")]
        public Stats AwayStats;

        //public (decimal AttackStrength, decimal DefenseStrength) CalculateAttackAndDefenseStrength(int homeOrAway)
        //{
        //    // As homeOrAway == 0 will be covered by the line below, only 1 or 'other' needs to be described here
        //    var awayOrOther = homeOrAway == 1 ? (AwayStats.AttackStrength, AwayStats.DefenseStrength) : (1, 1);

        //    var strengths = homeOrAway == 0 ? (HomeStats.AttackStrength, HomeStats.DefenseStrength) : awayOrOther;
        //    return strengths;
        //}
    }

}