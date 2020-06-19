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
                result.competitionScore[homeTeam].goalDifference += (match.Scores[homeTeam] - match.Scores[awayTeam]);
                result.competitionScore[awayTeam].goalsFor += match.Scores[awayTeam];
                result.competitionScore[awayTeam].goalsAgainst += match.Scores[homeTeam];
                result.competitionScore[awayTeam].goalDifference += (match.Scores[awayTeam] - match.Scores[homeTeam]);
            }
            // whether toDictionary should be x.value or x.key is not relevant, either should suffice. Using linq, you have to recreate a dictionary all over again, sadly.
            var sortedDict = result.competitionScore.OrderByDescending(x => x.Value.pouleStance).ThenByDescending(x => x.Value.goalDifference).ToDictionary(x => x.Value).Values;           
            var newSortedDict = new Dictionary<Team, CompetitionNumbers>();
            foreach (var item in sortedDict)
            {
                // mutual result check: teams with same goaldifference and poulestance should check their own match. winner is to be on top
                foreach (var score2 in from score2 in sortedDict
                                       where (item.Key != score2.Key) && ((item.Value.pouleStance == score2.Value.pouleStance) && (item.Value.goalDifference == score2.Value.goalDifference))
                                       let matches = result.matchResults.Where(x => x.Scores.ContainsKey(item.Key) && x.Scores.ContainsKey(score2.Key))
                                       from _ in
                                           from match in matches
                                           where match.winner != null
                                           where item.Key != match.winner
                                           select new { }
                                       select score2)
                {
                    newSortedDict.Add(score2.Key, score2.Value);
                    newSortedDict.Add(item.Key, item.Value);
                }
                // A dirty way to check if they are not already added because of the mutual result check above
                if (!newSortedDict.ContainsKey(item.Key))
                { 
                    newSortedDict.Add(item.Key, item.Value); 
                }
            }
            result.competitionScore = newSortedDict;
            return result;
        }
    }
}
