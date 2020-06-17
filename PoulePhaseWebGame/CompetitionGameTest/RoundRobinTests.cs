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

            #region Round Robin Asserts
            // Round Robin should be the following matchups
            // Round 	   Match one 	      Match two
            // Round 1     Team A v.Team D    Team B v.Team C
            // Round 2     Team C v.Team A    Team D v.Team B
            // Round 3     Team A v.Team B    Team C v.Team D

            // Noticed with the switching etc, that round 3 is round 1 and visa versa, but that's ok, all the matchups are still the same
            Team TeamA = request.teams[0];
            Team TeamB = request.teams[1];
            Team TeamC = request.teams[2];
            Team TeamD = request.teams[3];

            // Round 1: Team A v.Team B
            try
            {
                Assert.IsTrue(result.matchResults[0].Scores[TeamA] == 1);
                Assert.IsTrue(result.matchResults[0].Scores[TeamB] == 0);
            }
            catch (KeyNotFoundException ex)
            {
                Assert.Fail("Round 1 didn't have the correct teams: Team A v.Team B");
            }

            try
            {
                // Round 1: Team C v.Team D
                Assert.IsTrue(result.matchResults[1].Scores[TeamC] == 1);
                Assert.IsTrue(result.matchResults[1].Scores[TeamD] == 0);
            }
            catch (KeyNotFoundException ex)
            {
                Assert.Fail("Round 1 didn't have the correct teams: Team C v.Team D");
            }

            // Round 2: Team C v.Team A
            try
            {
                Assert.IsTrue(result.matchResults[2].Scores[TeamC] == 1);
                Assert.IsTrue(result.matchResults[2].Scores[TeamA] == 0);
            }
            catch (KeyNotFoundException ex)
            {
                Assert.Fail("Round 2 didn't have the correct teams: Team C v.Team A");
            }

            // Round 2: Team D v.Team B
            try
            {
                Assert.IsTrue(result.matchResults[3].Scores[TeamD] == 1);
                Assert.IsTrue(result.matchResults[3].Scores[TeamB] == 0);
            }
            catch (KeyNotFoundException ex)
            {
                Assert.Fail("Round 2 didn't have the correct teams: Team D v.Team B");
            }

            // Round 3: Team A v.Team D
            try
            {
                Assert.IsTrue(result.matchResults[4].Scores[TeamA] == 1);
                Assert.IsTrue(result.matchResults[4].Scores[TeamD] == 0);
            }
            catch (KeyNotFoundException ex)
            {
                Assert.Fail("Round 3 didn't have the correct teams: Team A v.Team D");
            }

            // Round 3: Team B v.Team C
            try
            {
                Assert.IsTrue(result.matchResults[5].Scores[TeamB] == 1);
                Assert.IsTrue(result.matchResults[5].Scores[TeamC] == 0);
            }
            catch (KeyNotFoundException ex)
            {
                Assert.Fail("Round 3 didn't have the correct teams: Team B v.Team C");
            }
            #endregion


            Assert.IsTrue(result.matchResults[0].winRemarks.Name == "NoRemarks");
        }

        [TestMethod]
        public void TestStatistics()
        {
            //RoundRobinResult result = new RoundRobinResult
            //{
            //    matchResults
            //}
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
