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
            var teamRotation = GenerateRoundRobin(request.teams.Count);
            var tournamentList = new List<List<Team>>();

            // GenerateRoundRobin works with rounds, so have to rotate through the rounds to generate the list.
            for (int i = 0; i < teamRotation.GetUpperBound(1); i++)
            {
                for (int j = 0; j < request.teams.Count-1; j++)
                {
                    var matchuplist = new List<Team>();
                    int teamnumber1 = j;
                    int teamnumber2 = teamRotation[j, i];
                    var team1 = request.teams[teamnumber1];
                    var team2 = request.teams[teamnumber2];
                    matchuplist.Add(team1);
                    matchuplist.Add(team2);
                    tournamentList.Add(matchuplist);
                } 
            }

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

        // Return an array where results(i, j) gives
        // the opponent of team i in round j.
        // Note: num_teams must be odd.
        private int[,] GenerateRoundRobinOdd(int num_teams)
        {
            int n2 = (int)((num_teams - 1) / 2);
            int[,] results = new int[num_teams, num_teams];

            // Initialize the list of teams.
            int[] teams = new int[num_teams];
            for (int i = 0; i < num_teams; i++) teams[i] = i;

            // Start the rounds.
            for (int round = 0; round < num_teams; round++)
            {
                for (int i = 0; i < n2; i++)
                {
                    int team1 = teams[n2 - i];
                    int team2 = teams[n2 + i + 1];
                    results[team1, round] = team2;
                    results[team2, round] = team1;
                }

                // Set the team with the bye.
                results[teams[0], round] = BYE;

                // Rotate the array.
                RotateArray(teams);
            }

            return results;
        }

        // Rotate the entries one position.
        private void RotateArray(int[] teams)
        {
            int tmp = teams[^1];
            Array.Copy(teams, 0, teams, 1, teams.Length - 1);
            teams[0] = tmp;
        }

        private int[,] GenerateRoundRobinEven(int num_teams)
        {
            // Generate the result for one fewer teams.
            int[,] results = GenerateRoundRobinOdd(num_teams - 1);

            // Copy the results into a bigger array,
            // replacing the byes with the extra team.
            int[,] results2 = new int[num_teams, num_teams - 1];
            for (int team = 0; team < num_teams - 1; team++)
            {
                for (int round = 0; round < num_teams - 1; round++)
                {
                    if (results[team, round] == BYE)
                    {
                        // Change the bye to the new team.
                        results2[team, round] = num_teams - 1;
                        results2[num_teams - 1, round] = team;
                    }
                    else
                    {
                        results2[team, round] = results[team, round];
                    }
                }
            }

            return results2;
        }

        private int[,] GenerateRoundRobin(int num_teams)
        {
            if (num_teams % 2 == 0)
                return GenerateRoundRobinEven(num_teams);
            else
                return GenerateRoundRobinOdd(num_teams);
        }
    }
}
