using CompetitionGame.Command;
using CompetitionGame.Models.Request;
using CompetitionGame.Models.Result;

namespace CompetitionGameTest
{
    public class FakeMatchCommand : ICommandHandler<MatchRequest, MatchResult>
    {
        public MatchResult Handle(MatchRequest request)
        {
            return new MatchResult();
        }
    }
}
