using CompetitionGame.Evaluators;
using System;

namespace CompetitionGame.Command
{
    public class PoissonPotentialOutcomeCalculator : ICommandHandler<CalculatePotentialOutcomeRequest, CalculatePotentialOutcomeResult>
    {
        private decimal probabilityLimit;
        IPoissonEvaluator evalHome, evalAway;

        public PoissonPotentialOutcomeCalculator(IPoissonEvaluator EvalHome, IPoissonEvaluator EvalAway, decimal ProbabilityLimit = 0.04M)
        {
            probabilityLimit = ProbabilityLimit;
            evalHome = EvalHome;
            evalAway = EvalAway;
        }

        public CalculatePotentialOutcomeResult Handle(CalculatePotentialOutcomeRequest request)
        {
            var potentialOutcomeResult = new CalculatePotentialOutcomeResult();

            // Home team attack strength * away team defence strength * average number of home goals
            var projectedHomeTeamGoals = request.homeTeam.HomeStats.AttackStrength * request.homeTeam.AwayStats.DefenseStrength * request.AverageHomeGoals;
            // Away team attack strength *home team defence strength* average number of away goals
            var projectedAwayTeamGoals = request.awayTeam.AwayStats.AttackStrength * request.homeTeam.HomeStats.DefenseStrength * request.AverageAwayGoals;

            evalHome.SetLambda(Convert.ToDecimal(projectedHomeTeamGoals));
            evalAway.SetLambda(Convert.ToDecimal(projectedAwayTeamGoals));

            for (int j = 0; j < 6; j++)
            {
                var homeGoalsIntpercentage = evalHome.ProbabilityMassFunction(j);

                for (int k = 0; k < 6; k++)
                {
                    var awayGoalsIntpercentage = evalAway.ProbabilityMassFunction(k);
                    var potentialOutcome = (homeGoalsIntpercentage * awayGoalsIntpercentage);
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
}
