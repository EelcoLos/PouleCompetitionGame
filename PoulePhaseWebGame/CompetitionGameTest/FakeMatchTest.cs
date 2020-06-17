using CompetitionGame;
using CompetitionGame.Command;
using CompetitionGame.Models.Request;
using CompetitionGame.Models.Result;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace CompetitionGameTest
{
    public class FakeMatchCommand : ICommandHandler<MatchRequest, MatchResult>
    {
        public MatchResult Handle(MatchRequest request)
        {
            var matchResult = new MatchResult
            {
                Scores = new Dictionary<Team, int>
                {
                    { request.teams[0], 1 },
                    { request.teams[1], 0 }
                },
                winner = request.teams[0],
                winRemarks = new LocalizedString("NoRemarks", "")
            };
            return matchResult;
        }
    }
}
