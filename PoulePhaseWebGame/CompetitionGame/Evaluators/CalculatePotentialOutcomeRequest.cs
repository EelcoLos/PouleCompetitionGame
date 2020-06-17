namespace CompetitionGame.Command
{
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
