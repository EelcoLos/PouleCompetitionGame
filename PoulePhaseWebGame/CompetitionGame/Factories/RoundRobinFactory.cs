
using CompetitionGame.Command;
using CompetitionGame.Models.Request;
using CompetitionGame.Models.Result;
using System;
using System.Collections.Generic;

namespace CompetitionGame.Factories
{
    public static class RoundRobinFactory
    {
        public static RoundRobinRequest CreateRoundRobinRequest(List<Team> Teams, HistoryLeagueStats historyStats) => new RoundRobinRequest() { teams = Teams, stats = historyStats };

        public static RoundRobinResult CreateResult(List<MatchResult> MatchResults) => new RoundRobinResult() { matchResults = MatchResults };
    }
}
