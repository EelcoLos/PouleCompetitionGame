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
            Team winningTeam = request.teams[0];
            Team losingTeam = request.teams[3];
            Assert.IsTrue(result.matchResults[0].Scores[winningTeam] == 1);
            Assert.IsTrue(result.matchResults[0].Scores[losingTeam] == 0);
            Assert.IsTrue(result.matchResults[0].winner == winningTeam);
            Assert.IsTrue(result.matchResults[0].winRemarks.Name == "NoRemarks");
        }

        public void Initialize()
        {
            _matchFactory = new FootballMatchFactory();
            //_matchExecutioner = new MatchHandler(_matchFactory, new PoissonPotentialOutcomeCalculator());
            _matchExecutioner = new FakeMatchCommand();
            _RRSelector = new RoundRobinTournamentSelector(_matchExecutioner, _matchFactory);
        }
    }
}
