﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CompetitionGame.Data.Models
{
    public class Team
    {
        [JsonProperty("teamName")]
        public string TeamName;
        [JsonProperty("homeStats")]
        public Stats HomeStats;
        [JsonProperty("awayStats")]
        public Stats AwayStats;
    }

}