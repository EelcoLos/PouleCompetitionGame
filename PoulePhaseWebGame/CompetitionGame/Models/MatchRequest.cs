using System.Collections.Generic;

namespace CompetitionGame
{
    public class MatchRequest
    {
        public List<Team> teams;
        public bool OneTeam => teams?.Count == 1;

        public HistoryLeagueStats leagueStats;
    }
}