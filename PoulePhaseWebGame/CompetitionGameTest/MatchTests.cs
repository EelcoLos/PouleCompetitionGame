using CompetitionGame;
using CompetitionGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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
            var potentialOutcomeCalculator = new PoissonPotentialOutcomeCalculator();
            var match = new MatchHandler(matchFactory, potentialOutcomeCalculator);
            MatchRequest matchRequest = matchFactory.CreateRequest(new List<Team> { teams.FirstOrDefault() }, leagueStats);
            var result = match.Handle(matchRequest);

            Assert.IsTrue(result.Scores.GetValueOrDefault(teams[0]) == 3);
            Assert.IsTrue(result.winner.TeamName == teams[0].TeamName);
            Assert.IsTrue(result.winRemarks.Name == "WinByDefault");
        }

        [TestMethod]
        public void PlayNormal90MinuteMatch()
        {
            SetupFootballMatchTests(out List<Team> teams, out FootballMatchFactory matchFactory, out HistoryLeagueStats leagueStats);
            var potentialOutcomeCalculator = new PoissonPotentialOutcomeCalculator();
            //var historyLeagueStats = new HistoryLeagueStats
            //{
            //    Homegoals = 436,
            //    Homematches = 17,
            //    Awaygoals = 279,
            //    Awaymatches = 17
            //};
            var match = new MatchHandler(matchFactory, potentialOutcomeCalculator); // , teamlist, homegoals, homematches, awaygoals, awaymatches
            MatchRequest matchRequest = matchFactory.CreateRequest(teams, leagueStats);
            var result = match.Handle(matchRequest);

            //if (result.Scores.GetValueOrDefault(team1) == result.Scores.GetValueOrDefault(team2))
            //{
            //    Assert.IsTrue(result.Scores.GetValueOrDefault(team1) == result.Scores.GetValueOrDefault(team2), "Tie match");
            //    Assert.IsTrue(result.winner == null, "No winner of the match");
            //}
            //Assert.IsTrue(result.winner.TeamName == team1.TeamName);
            Assert.IsTrue(result.winRemarks.Name == "NoRemarks");
        }

        private static void SetupFootballMatchTests(out List<Team> teams, out FootballMatchFactory matchFactory, out HistoryLeagueStats leagueStats)
        {
            teams = new List<Team>();
            HistoryLeagueStats historyLeagueStats = new HistoryLeagueStats();
            
            using (StreamReader r = new StreamReader("TestData.json"))
            {
                var json = r.ReadToEnd();
                var jsonObjects = JObject.Parse(json);
                var teamJson = jsonObjects["teams"].ToObject<List<Team>>();
                string historyLeagueStatsJson = jsonObjects["historyleaguestats"].ToString();
                var tms = new List<Team>();
                //foreach (var item in jsonObjects["teams"])
                //{
                //    Team team = JsonConvert.DeserializeObject<Team>(item.Children[0].ToString());
                //    tms.Add(team);
                //}
                //teams = JsonConvert.DeserializeObject<List<Team>>(teamJson);
                historyLeagueStats = JsonConvert.DeserializeObject<HistoryLeagueStats>(historyLeagueStatsJson);
            }
            leagueStats = historyLeagueStats;
            matchFactory = new FootballMatchFactory();
        }
    }
}
