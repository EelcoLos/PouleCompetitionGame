using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompetitionGame.Command;
using CompetitionGame.Data.Models;
using CompetitionGame.Factories;
using CompetitionGame.Models.Request;
using CompetitionGame.Models.Result;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetitionGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        ICommandHandler<RoundRobinRequest, RoundRobinResult> _RRSelector;

        public CompetitionController(ICommandHandler<RoundRobinRequest, RoundRobinResult> rRSelector)
        {
            _RRSelector = rRSelector;
        }

        // POST api/<Competition>
        [HttpPost]
        public async Task<IEnumerable<string>> PostAsync()
        {
            var bodyRequest = Request.Body;
            string streamresult;
            using (StreamReader reader = new StreamReader(bodyRequest, Encoding.UTF8))
            {
                streamresult = await reader.ReadToEndAsync();
            }
            var jsonObjects = JObject.Parse(streamresult);
            var teams = jsonObjects["teams"].ToObject<List<Team>>();
            var historyLeagueStats = jsonObjects["historyleaguestats"].ToObject<HistoryLeagueStats>();
            var request = RoundRobinFactory.CreateRoundRobinRequest(teams.ToList(), historyLeagueStats);
            var result = _RRSelector.Handle(request);


            var list = new List<string>();
            foreach (var res in result.matchResults)
            {
                var matchresult = JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings { ContractResolver = JsonContractResolvers.IgnoreIsSpecifiedMembersResolver, MaxDepth = 10 });
                list.Add(matchresult);
            }
            foreach (var compscore in result.competitionScore)
            {
                var competitionscore = JsonConvert.SerializeObject(compscore, Formatting.Indented, new JsonSerializerSettings { ContractResolver = JsonContractResolvers.IgnoreIsSpecifiedMembersResolver, MaxDepth = 10 });
                list.Add(competitionscore);
            }


            return list;
        }
    }
}
