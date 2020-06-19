using CompetitionGame.Data.Models;

namespace CompetitionGame.Command
{
    public class CalculatePotentialOutcomeRequest
    {
        public int homeGoals;
        public int homeMatches;
        public int awayGoals;
        public int awayMatches;
        public int numberofTeams = 18;

        public float AverageHomeGoals => (float)homeGoals / (float)homeMatches / (float)numberofTeams;
        public float AverageAwayGoals => (float)awayGoals / (float)awayMatches / (float)numberofTeams;

        public Team homeTeam;
        public Team awayTeam;
    }
}
