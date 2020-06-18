﻿using CompetitionGame.Command;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CompetitionGame.Data
{
    /// <summary>
    /// This Simulates an external service
    /// </summary>
    public class ExternalDataService : ICommandHandler<DataRequest, DataResult>
    {
        ICommandHandler<ExternalRequest, ExternalData> dataClient;
        public DataResult Handle(DataRequest request)
        {
            var data = dataClient.Handle(request.requestdata);
            var jsonObjects = JObject.Parse(data);
            var teams = jsonObjects["teams"].ToObject<List<Team>>();
            var historyLeagueStats = jsonObjects["historyleaguestats"].ToObject<HistoryLeagueStats>();
            return new DataResult() { Teams = teams, HistoryStats = historyLeagueStats };
        }
    }

    public class DataRequest
    {
        public dynamic requestdata;
    }

    public class DataResult
    {
        public List<Team> Teams;
        public HistoryLeagueStats HistoryStats;
    }
}
