using System.Collections.Generic;

namespace CompetitionGame
{
    public class CalculatePotentialOutcomeResult
    {
        public List<(Team hometeam, int homescore, Team awayteam, int awayscore)> PotentialOutcomes;

        public CalculatePotentialOutcomeResult() => PotentialOutcomes = new List<(Team hometeam, int homescore, Team awayteam, int awayscore)>();
    }
}