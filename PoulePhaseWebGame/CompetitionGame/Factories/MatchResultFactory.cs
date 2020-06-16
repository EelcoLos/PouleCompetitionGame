using CompetitionGame.Models.Request;
using CompetitionGame.Models.Result;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace CompetitionGame.Factories
{
    public abstract class MatchFactory
    {
        public virtual MatchResult CreateResult((Team hometeam, int score, Team otherteam, int awayscore) outcome, LocalizedString winRemarks)
        {
            var matchResult = new MatchResult
            {
                Scores = new Dictionary<Team, int>()
            };

            matchResult.winRemarks = winRemarks;

            return matchResult;
        }

        public virtual MatchRequest CreateRequest(List<Team> Teams, HistoryLeagueStats LeagueStats) =>
            new MatchRequest { teams = Teams , leagueStats = LeagueStats};
    }

}