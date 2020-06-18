using CompetitionGame.Data.Models;
using CompetitionGame.Evaluators;
using CompetitionGame.Factories;
using CompetitionGame.Models.Request;
using CompetitionGame.Models.Result;
using Microsoft.Extensions.Localization;
using System;

namespace CompetitionGame.Command
{
    public class MatchHandler : ICommandHandler<MatchRequest, MatchResult>
    {
        private MatchFactory _matchResultFactory;
        private readonly ICommandHandler<CalculatePotentialOutcomeRequest, CalculatePotentialOutcomeResult> _outcomeHandler;

        

        public MatchHandler(MatchFactory MatchResultFactory, ICommandHandler<CalculatePotentialOutcomeRequest, CalculatePotentialOutcomeResult> OutcomeHandler)
        {
            _matchResultFactory = MatchResultFactory;
            _outcomeHandler = OutcomeHandler;
        }

        public MatchResult Handle(MatchRequest request)
        {
            if (request.OneTeam)
            {
                LocalizedString winByDefault = new LocalizedString("WinByDefault", "Won by default, only 1 team participated in the match");
                var OneTeamresult = _matchResultFactory.CreateResult((request.teams[0],3,null, 0), winByDefault);
                return OneTeamresult;
            }
            Random r = new Random();

            (Team hometeam, int score, Team otherteam, int awayscore) matchResult = (null,-1,null,-1);  // 'Initialize'
            for (int i = 1; i < request.teams.Count; i++)
            {
                var outcomerequest = CalculatePotentialOutcomeFactory.CreatePotentialOutcomeRequest(request.teams[0], request.teams[i], request.leagueStats.Homegoals, request.leagueStats.Homematches, request.leagueStats.Awaygoals, request.leagueStats.Awaymatches);
                var outcomeResult = _outcomeHandler.Handle(outcomerequest); //CalculatePotentialOutcomes(r, homeTeamStrength, i);
                // randomize potential outcome
                if (outcomeResult.PotentialOutcomes.Count == 0) outcomeResult.PotentialOutcomes.Add((request.teams[0], 0, request.teams[i], 0)); // in case no found, add "0-0" to potential outcomes
                matchResult = outcomeResult.PotentialOutcomes[r.Next(0, outcomeResult.PotentialOutcomes.Count - 1)];
            }

            LocalizedString noRemarks = new LocalizedString("NoRemarks", string.Empty);

            var result = _matchResultFactory.CreateResult(matchResult, noRemarks);
            return result;
        }
    }
}