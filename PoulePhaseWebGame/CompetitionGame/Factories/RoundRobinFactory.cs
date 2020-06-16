
using CompetitionGame.Command;
using CompetitionGame.Models.Request;
using System.Collections.Generic;

namespace CompetitionGame.Factories
{
    public static class RoundRobinFactory
    {
        public static RoundRobinRequest CreateRoundRobinRequest(List<Team> Teams, HistoryLeagueStats historyStats) => new RoundRobinRequest() { teams = Teams, stats = historyStats };
    }
}
