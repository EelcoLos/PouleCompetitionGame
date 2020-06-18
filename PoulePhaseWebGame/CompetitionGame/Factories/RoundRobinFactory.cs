using CompetitionGame.Data.Models;
using CompetitionGame.Models.Request;
using CompetitionGame.Models.Result;
using System.Collections.Generic;
using System.Linq;

namespace CompetitionGame.Factories
{
    public static class RoundRobinFactory
    {
        public static RoundRobinRequest CreateRoundRobinRequest(List<Team> Teams, HistoryLeagueStats historyStats) => new RoundRobinRequest() { teams = Teams, stats = historyStats };

        public static RoundRobinResult CreateResult(List<MatchResult> MatchResults, RoundRobinRequest request)
        {
            var result = new RoundRobinResult()
            {
                matchResults = MatchResults,
                competitionScore = new Dictionary<Team, CompetitionNumbers>()
            };
            foreach (var match in MatchResults)
            {
                Team homeTeam = match.Scores.First().Key;
                Team awayTeam = match.Scores.Last().Key;
                if (!result.competitionScore.ContainsKey(homeTeam))
                    result.competitionScore.Add(homeTeam, new CompetitionNumbers());
                if (!result.competitionScore.ContainsKey(awayTeam))
                    result.competitionScore.Add(awayTeam, new CompetitionNumbers());

                if (match.winner == null)
                {
                    result.competitionScore[homeTeam].pouleStance += 1;
                    result.competitionScore[awayTeam].pouleStance += 1;
                }
                else
                {
                    result.competitionScore[match.winner].pouleStance += 3;
                }

                result.competitionScore[homeTeam].goalsFor += match.Scores[homeTeam];
                result.competitionScore[homeTeam].goalsAgainst += match.Scores[awayTeam];
                result.competitionScore[awayTeam].goalsFor += match.Scores[awayTeam];
                result.competitionScore[awayTeam].goalsAgainst += match.Scores[homeTeam];
                result.competitionScore.OrderByDescending(x => x.Value.pouleStance).ToList();
            }
            return result;
        }
    }
}
