using CompetitionGame.Factories;
using CompetitionGame.Models.Request;
using CompetitionGame.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetitionGame.Command
{
    public class RoundRobinTournamentSelector : ICommandHandler<RoundRobinRequest, RoundRobinResult>
    {
        ICommandHandler<MatchRequest, MatchResult> _matchCommand;
        MatchFactory _matchFactory;

        private const int BYE = -1;

        public RoundRobinTournamentSelector(ICommandHandler<MatchRequest, MatchResult> matchCommand, MatchFactory matchFactory)
        {
            _matchCommand = matchCommand;
            _matchFactory = matchFactory;
        }

        public RoundRobinResult Handle(RoundRobinRequest request)
        {
            List<List<Team>> tournamentList = ConstructTournamentList(request);
            List<MatchResult> matchResults = new List<MatchResult>();
            PlayTournament(request, tournamentList, matchResults);
            var result = RoundRobinFactory.CreateResult(matchResults);

            return result;
        }

        /// <summary>
        /// Generate a Round Robin style tournament List, using numbers, then attach that to an actual list of matchups
        /// </summary>
        /// <param name="request">Teams for the tournament</param>
        /// <returns>The tournament matchup list</returns>
        private List<List<Team>> ConstructTournamentList(RoundRobinRequest request)
        {
            var tournamentList = GenerateRoundRobin(request.teams);          
            return tournamentList;
        }

        /// <summary>
        /// Play the actual tournament.
        /// </summary>
        /// <remarks>I know this could be a seperate action/class. As it stands, it's only creating more classes then already necessary</remarks>
        /// <param name="request"></param>
        /// <param name="tournamentList"></param>
        /// <param name="matchResults"></param>
        private void PlayTournament(RoundRobinRequest request, List<List<Team>> tournamentList, List<MatchResult> matchResults)
        {
            foreach (var matchup in tournamentList)
            {
                var matchrequest = _matchFactory.CreateRequest(matchup, request.stats);
                matchResults.Add(_matchCommand.Handle(matchrequest));
            }
        }

        private List<List<Team>> GenerateRoundRobin(List<Team> ListTeam)
        {


            int numTeams = ListTeam.Count;
            if (numTeams % 2 != 0)
            {
                ListTeam.Add(new Team(){TeamName = "BYE"});
            }


            int numDays = (numTeams - 1);
            int halfSize = numTeams / 2;

            List<Team> functionTeams = new List<Team>();

            functionTeams.AddRange(ListTeam); // Copy all the elements.
            functionTeams.RemoveAt(0); // To exclude the first team.

            int teamsSize = functionTeams.Count;
            var tournamentList = new List<List<Team>>();
            for (int day = 0; day < numDays; day++)
            {
                Console.WriteLine("Day {0}", (day + 1));

                int teamIdx = day % teamsSize;

                Console.WriteLine("{0} vs {1}", functionTeams[teamIdx], ListTeam[0]);
                if (day % 2 == 0)
                    tournamentList.Add(new List<Team> { ListTeam[0], functionTeams[teamIdx] });
                else
                    tournamentList.Add(new List<Team> { functionTeams[teamIdx], ListTeam[0] });

                for (int idx = 1; idx < halfSize; idx++)
                {
                    var matchup = new List<Team>();
                    int firstTeam = (day + idx) % teamsSize;
                    int secondTeam = (day + teamsSize - idx) % teamsSize;
                    Console.WriteLine("{0} vs {1}", functionTeams[firstTeam].TeamName, functionTeams[secondTeam].TeamName);
                    matchup.Add(functionTeams[firstTeam]);
                    matchup.Add(functionTeams[secondTeam]);
                    tournamentList.Add(matchup);
                }
            }
            return tournamentList;

        }
    }
}
