namespace CompetitionGame.Evaluators
{
    public interface IPoissonEvaluator
    {
        decimal CummulitiveDistributionFunction(int k);
        decimal ProbabilityMassFunction(int k);
        void SetLambda(decimal lambda = 1.0M);
    }
}