using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;

namespace CompetitionGame.Models.Result
{
    public class MatchResult
    {
        public Dictionary<Team, int> Scores;
        public Team winner;
        public LocalizedString winRemarks;

        public new string ToString => $"{Scores.First().Key.TeamName} - {Scores.Last().Key.TeamName}: {Scores.First().Value} - {Scores.Last().Value}";
    }

}