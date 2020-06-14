using Microsoft.Extensions.Localization;

namespace CompetitionGame
{
    public class FootballMatchResultFactory : MatchResultFactory
    {
        public override MatchResult CreateResult((Team hometeam, int score, Team otherteam, int awayscore) outcome, LocalizedString winRemarks)
        {
            var matchResult = base.CreateResult(outcome, winRemarks);
            matchResult.Scores.Add(outcome.hometeam, outcome.score);
            if (outcome.otherteam != null) 
                matchResult.Scores.Add(outcome.otherteam, outcome.awayscore);
            matchResult.winner = outcome.score > outcome.awayscore ? outcome.hometeam : outcome.score == outcome.awayscore ? null : outcome.otherteam;

            return matchResult;
        }
    }

}