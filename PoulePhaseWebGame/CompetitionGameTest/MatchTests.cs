using CompetitionGame.Command;
using CompetitionGame.Data.Models;
using CompetitionGame.Evaluators;
using CompetitionGame.Factories;
using CompetitionGame.Models.Request;
using CompetitionGame.Models.Result;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CompetitionGameTest
{
    [TestClass]
    public class MatchTests
    {
        [TestMethod]
        public void PlayMatchWithOneTeam()
        {
            SetupFootballMatchTests(out List<Team> teams, out FootballMatchFactory matchFactory, out HistoryLeagueStats leagueStats);
            var potentialOutcomeCalculator = new PoissonPotentialOutcomeCalculator(new PoissonEvaluator(), new PoissonEvaluator());
            var match = new MatchHandler(matchFactory, potentialOutcomeCalculator);
            MatchRequest matchRequest = matchFactory.CreateRequest(new List<Team> { teams.FirstOrDefault() }, leagueStats);
            var result = match.Handle(matchRequest);

            Assert.IsTrue(result.Scores.GetValueOrDefault(teams[0]) == 3);
            Assert.IsTrue(result.winner.TeamName == teams[0].TeamName);
            Assert.IsTrue(result.winRemarks.Name == "WinByDefault");
            Assert.IsTrue(result.GetType() == typeof(MatchResult));
        }

        [TestMethod]
        public void PlayNormalMatch()
        {
            SetupFootballMatchTests(out List<Team> teams, out FootballMatchFactory matchFactory, out HistoryLeagueStats leagueStats);
            var potentialOutcomeCalculator = new PoissonPotentialOutcomeCalculator(new PoissonEvaluator(), new PoissonEvaluator());

            var match = new MatchHandler(matchFactory, potentialOutcomeCalculator);
            MatchRequest matchRequest = matchFactory.CreateRequest(teams, leagueStats);
            var result = match.Handle(matchRequest);

            if (result.Scores.GetValueOrDefault(teams[0]) == result.Scores.GetValueOrDefault(teams[1]))
            {
                Assert.IsTrue(result.Scores.GetValueOrDefault(teams[0]) == result.Scores.GetValueOrDefault(teams[1]), "Tie match");
                Assert.IsTrue(result.winner == null, "No winner of the match");
            }
            Assert.IsTrue(result.winRemarks.Name == "NoRemarks");
            Assert.IsTrue(result.GetType() == typeof(MatchResult));
        }

        public static void SetupFootballMatchTests(out List<Team> teams, out FootballMatchFactory matchFactory, out HistoryLeagueStats leagueStats)
        {
            teams = new List<Team>();
            HistoryLeagueStats historyLeagueStats = new HistoryLeagueStats();
            
            using (StreamReader r = new StreamReader("TestData.json"))
            {
                var json = r.ReadToEnd();
                var jsonObjects = JObject.Parse(json);
                teams = jsonObjects["teams"].ToObject<List<Team>>();
                historyLeagueStats = jsonObjects["historyleaguestats"].ToObject<HistoryLeagueStats>();
            }
            leagueStats = historyLeagueStats;
            matchFactory = new FootballMatchFactory();
        }
    }
}
