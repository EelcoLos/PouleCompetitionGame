﻿namespace CompetitionGame.Models
{
    public static class CalculatePotentialOutcomeFactory
    {
        public static CalculatePotentialOutcomeRequest CreatePotentialOutcomeRequest(Team HomeTeam, Team AwayTeam, int statisticHomeGoals, int staticHomeMatches, int statisticAwayGoals, int statisticAwayMatches) =>
            new CalculatePotentialOutcomeRequest { homeTeam = HomeTeam, awayTeam = AwayTeam, homeGoals = statisticHomeGoals, homeMatches = staticHomeMatches, awayGoals = statisticAwayGoals, awayMatches = statisticAwayMatches };
    }
}
