﻿using CompetitionGame.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace CompetitionGame.Models.Result
{
    public class RoundRobinResult
    {
        public Dictionary<Team, CompetitionNumbers> competitionScore;
        public List<MatchResult> matchResults;
        public new string ToString
        {
            get
            {
                List<string> resultStrings = (from result in matchResults
                                              select result.ToString).ToList();
                return string.Join<string>(",", resultStrings);
            }
        }

        public dynamic GetStatitistics()
        {
            return null;
        }
    }
}
