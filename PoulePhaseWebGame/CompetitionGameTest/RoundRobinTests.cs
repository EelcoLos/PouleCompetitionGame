using CompetitionGame;
using CompetitionGame.Command;
using CompetitionGame.Factories;
using CompetitionGame.Models.Request;
using CompetitionGame.Models.Result;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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


            var request = RoundRobinFactory.CreateRoundRobinRequest(teams, leagueStats);
            var result = _RRSelector.Handle(request);

            Assert.IsTrue(result.GetType() == typeof(RoundRobinResult));
        }

        public void Initialize()
        {
            _matchExecutioner = new FakeMatchCommand();
            _matchFactory = new FootballMatchFactory();
            _RRSelector = new RoundRobinTournamentSelector(_matchExecutioner, _matchFactory);
        }
    }
}
