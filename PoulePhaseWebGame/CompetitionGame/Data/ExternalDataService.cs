using CompetitionGame.Command;
using CompetitionGame.Data.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CompetitionGame.Data
{
    /// <summary>
    /// This Simulates an external service
    /// </summary>
    public class ExternalDataService : ICommandHandler<DataRequest, DataResult>
    {
        private ICommandHandler<ExternalRequest, ExternalData> dataClient;

        public ExternalDataService(ICommandHandler<ExternalRequest, ExternalData> DataClient)
        {
            dataClient = DataClient;
        }

        public DataResult Handle(DataRequest request)
        {
            var data = dataClient.Handle(request.requestdata);
            var jsonObjects = JObject.Parse(data.Json);
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
