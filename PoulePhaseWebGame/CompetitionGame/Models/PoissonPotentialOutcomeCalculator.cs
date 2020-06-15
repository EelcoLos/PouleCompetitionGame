using System;
using System.Collections.Generic;

namespace CompetitionGame.Models
{
    public class PoissonPotentialOutcomeCalculator : ICommandHandler<CalculatePotentialOutcomeRequest, CalculatePotentialOutcomeResult>
    {
        private decimal probabilityLimit;

        public PoissonPotentialOutcomeCalculator(decimal ProbabilityLimit = 0.15M)
        {
            probabilityLimit = ProbabilityLimit;
        }

        public CalculatePotentialOutcomeResult Handle(CalculatePotentialOutcomeRequest request)
        {
            var potentialOutcomeResult = new CalculatePotentialOutcomeResult();

            // Home team attack strength * away team defence strength * average number of home goals
            var projectedHomeTeamGoals = request.homeTeam.HomeStats.AttackStrength * request.homeTeam.AwayStats.DefenseStrength * request.AverageHomeGoals;
            // Away team attack strength *home team defence strength* average number of away goals
            var projectedAwayTeamGoals = request.awayTeam.AwayStats.AttackStrength * request.homeTeam.HomeStats.DefenseStrength * request.AverageAwayGoals;

            var evalHome = new PoissonEvaluator(Convert.ToDecimal(projectedHomeTeamGoals));
            var evalAway = new PoissonEvaluator(Convert.ToDecimal(projectedAwayTeamGoals));
//            var potentialOutcomes = new List<(Team hometeam, int homescore, Team awayteam, int awayscore)>();

            for (int j = 0; j < 6; j++)
            {
                var homeGoalsIntpercentage = evalHome.ProbabilityMassFunction(j);

                for (int k = 0; k < 6; k++)
                {
                    var awayGoalsIntpercentage = evalAway.ProbabilityMassFunction(k);
                    var potentialOutcome = (homeGoalsIntpercentage * awayGoalsIntpercentage) * 100;
                    if (potentialOutcome > probabilityLimit)
                    {
                        var res = (request.homeTeam, j, request.awayTeam, k);
                        potentialOutcomeResult.PotentialOutcomes.Add(res);
                    }
                }
            }
            return potentialOutcomeResult;
        }
    }

    public class CalculatePotentialOutcomeRequest
    {
        public int homeGoals;
        public int homeMatches;
        public int awayGoals;
        public int awayMatches;
        public int numberofTeams = 18;

        public decimal AverageHomeGoals => homeGoals / homeMatches / numberofTeams;
        public decimal AverageAwayGoals => awayGoals / awayMatches / numberofTeams;

        public Team homeTeam;
        public Team awayTeam;
    }
}
