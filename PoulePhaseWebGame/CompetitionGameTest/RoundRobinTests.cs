using CompetitionGame;
using CompetitionGame.Command;
using CompetitionGame.Factories;
using CompetitionGame.Models.Request;
using CompetitionGame.Models.Result;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace CompetitionGameTest
{
    [TestClass]
    public class RoundRobinTests
    {
        ICommandHandler<RoundRobinRequest, RoundRobinResult> _RRSelector;
        ICommandHandler<MatchRequest, MatchResult> _matchExecutioner;
        MatchFactory _matchFactory;
        [TestMethod]
        public void TestLeague()
        {
            Initialize();
            MatchTests.SetupFootballMatchTests(out List<Team> teams, out _, out HistoryLeagueStats leagueStats);


            var request = RoundRobinFactory.CreateRoundRobinRequest(teams.Take(4).ToList(), leagueStats);
            var result = _RRSelector.Handle(request);

            Assert.IsTrue(result.GetType() == typeof(RoundRobinResult));
            Assert.IsNotNull(result.matchResults);
            Assert.IsTrue(result.matchResults[0].Scores[request.teams[1]] == 1);
            Assert.IsTrue(result.matchResults[0].Scores[request.teams[0]] == 0);
            Assert.IsTrue(result.matchResults[0].winner == request.teams[1]);
            Assert.IsTrue(result.matchResults[0].winRemarks.Name == "NoRemarks");
        }

        public void Initialize()
        {
            _matchExecutioner = new FakeMatchCommand();
            _matchFactory = new FootballMatchFactory();
            _RRSelector = new RoundRobinTournamentSelector(_matchExecutioner, _matchFactory);
        }
    }
}
