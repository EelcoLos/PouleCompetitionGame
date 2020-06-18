using Newtonsoft.Json;

namespace CompetitionGame.Data.Models
{
    public class HistoryLeagueStats
    {
        [JsonProperty("homegoals")]
        public int Homegoals { get; set; }
        [JsonProperty("homematches")]
        public int Homematches { get; set; }
        [JsonProperty("awaygoals")]
        public int Awaygoals { get; set; }
        [JsonProperty("awaymatches")]
        public int Awaymatches { get; set; }
    }
}