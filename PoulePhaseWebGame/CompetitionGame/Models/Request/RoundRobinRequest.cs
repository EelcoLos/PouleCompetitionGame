using System.Collections.Generic;

namespace CompetitionGame.Models.Request
{
    public class RoundRobinRequest
    {
        public List<Team> teams;
        public HistoryLeagueStats stats;
    }
}
