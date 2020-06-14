using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;

namespace CompetitionGame
{
    public class Match
    {
        private List<Team> _teams;
        private int homeGoals;
        private int homeMatches;
        private float awayGoals;
        private int awayMatches;
        private decimal probabilityLimit = 0.15M;

        public bool OneTeam => _teams?.Count == 1;

        public float averageHomeGoals => homeGoals / homeMatches;
        public float averageAwayGoals => awayGoals / awayMatches;

        MatchResultFactory matchResultFactory;

        public Match(List<Team> teams, int HomeGoals = 10, int HomeMatches = 10, int AwayGoals = 12, int AwayMatches = 12)
        {
            _teams = teams;
            homeGoals = HomeGoals;
            homeMatches = HomeMatches;
            awayGoals = AwayGoals;
            awayMatches = AwayMatches;
        }


        public MatchResult Execute()
        {
            matchResultFactory = new FootballMatchResultFactory();

            if (OneTeam)
            {
                LocalizedString winByDefault = new LocalizedString("WinByDefault", "Won by default, only 1 team participated in the match");
                var OneTeamresult = matchResultFactory.CreateResult((_teams[0],3,null, 0), winByDefault);
                return OneTeamresult;
            }

            Random r = new Random();

            var homeTeamStrength = _teams[0].CalculateAttackAndDefenseStrength(0);
            (Team hometeam, int score, Team otherteam, int awayscore) matchResult = (null,-1,null,-1);  // 'Initialize'
            for (int i = 1; i < _teams.Count; i++)
            {
                var potentialOutcomes = CalculatePotentialOutcomes(r, homeTeamStrength, i);
                // randomize potential outcome
                if (potentialOutcomes.Count == 0) potentialOutcomes.Add((_teams[0], 0, _teams[i], 0)); // in case no found, add "0-0" to potential outcomes
                matchResult = potentialOutcomes[r.Next(0, potentialOutcomes.Count - 1)];
            }

            LocalizedString noRemarks = new LocalizedString("NoRemarks", string.Empty);

            var result = matchResultFactory.CreateResult(matchResult, noRemarks);
            return result;
        }

        private List<(Team hometeam, int score, Team otherteam, int awayscore)> CalculatePotentialOutcomes(Random r, (float AttackStrength, float DefenseStrength) homeTeamStrength, int i)
        {
            // Home team attack strength * away team defence strength * average number of home goals
            var projectedHomeTeamGoals = homeTeamStrength.AttackStrength * _teams[i].AwayStats.DefenseStrength * averageHomeGoals;
            // Away team attack strength *home team defence strength* average number of away goals
            var projectedAwayTeamGoals = _teams[i].AwayStats.AttackStrength * homeTeamStrength.DefenseStrength * averageAwayGoals;

            var evalHome = new PoissonEvaluator(Convert.ToDecimal(projectedHomeTeamGoals));
            var evalAway = new PoissonEvaluator(Convert.ToDecimal(projectedAwayTeamGoals));
            var potentialOutcomes = new List<(Team hometeam, int score, Team otherteam, int awayscore)>();

            for (int j = 0; j < 6; j++)
            {
                var homeGoalsIntpercentage = evalHome.ProbabilityMassFunction(j);

                for (int k = 0; k < 6; k++)
                {
                    var awayGoalsIntpercentage = evalAway.ProbabilityMassFunction(k);
                    var potentialOutcome = (homeGoalsIntpercentage * awayGoalsIntpercentage) * 100;
                    if (potentialOutcome > probabilityLimit)
                    {
                        var res = (_teams[0], j, _teams[i], k);
                        potentialOutcomes.Add(res);
                    }
                }
            }

            return potentialOutcomes;
        }
    }

}