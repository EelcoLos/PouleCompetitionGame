using CompetitionGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CompetitionGameTest
{
    [TestClass]
    public class MatchTests
    {
        [TestMethod]
        public void PlayMatchWithOneTeam()
        {
            var team1 = new Team() { TeamName = "Ajax" };
            var teamlist = new List<Team>{team1};
            var match = new Match(teamlist);
            var result = match.Execute();

            Assert.IsTrue(result.Scores.GetValueOrDefault(team1) == 3);
            Assert.IsTrue(result.winner.TeamName == team1.TeamName);
            Assert.IsTrue(result.winRemarks.Name == "WinByDefault");
        }

        [TestMethod]
        public void PlayNormal90MinuteMatch()
        {
            var team1 = new Team() { TeamName = "Ajax", HomeStats = new Stats{GamesPlayed = 17, GoalsMade = 70, GoalsConceded = 13}, AwayStats = new Stats { GamesPlayed = 17, GoalsMade = 49, GoalsConceded = 19 } };
            var team2 = new Team() { TeamName = "Feijenoord", HomeStats = new Stats { GamesPlayed = 17, GoalsMade = 43, GoalsConceded = 16}, AwayStats = new Stats { GamesPlayed = 17, GoalsMade = 25, GoalsConceded = 32 } };
            var teamlist = new List<Team> { team1, team2 };
            var homegoals = 436;
            var homematches = 17;
            var awaygoals = 279;
            var awaymatches = 17;
            var match = new Match(teamlist, homegoals, homematches, awaygoals, awaymatches);
            var result = match.Execute();

            if (result.Scores.GetValueOrDefault(team1) == result.Scores.GetValueOrDefault(team2))
            {
                Assert.IsTrue(result.Scores.GetValueOrDefault(team1) == result.Scores.GetValueOrDefault(team2), "Tie match");
                Assert.IsTrue(result.winner == null, "No winner of the match");
            }
            //Assert.IsTrue(result.winner.TeamName == team1.TeamName);
            Assert.IsTrue(result.winRemarks.Name == "NoRemarks");
        }
    }
}
