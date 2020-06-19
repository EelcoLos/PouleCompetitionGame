using CompetitionGame.Data.Models;
using System.Collections.Generic;

namespace CompetitionGame.Evaluators
{
    public class CalculatePotentialOutcomeResult
    {
        public List<(Team hometeam, int homescore, Team awayteam, int awayscore)> PotentialOutcomes;

        public CalculatePotentialOutcomeResult() => PotentialOutcomes = new List<(Team hometeam, int homescore, Team awayteam, int awayscore)>();
    }
}