using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace CompetitionGame
{
    public abstract class MatchResultFactory
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
    }

}