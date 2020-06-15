using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace CompetitionGame
{
    public class MatchResult
    {
        public Dictionary<Team, int> Scores;
        public Team winner;
        public LocalizedString winRemarks;
    }

}