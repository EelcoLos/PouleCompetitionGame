﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using CompetitionGame.Command;
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

        // GET: api/<Competition>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //List<Team> teams = new List<Team>();
            //HistoryLeagueStats historyLeagueStats;
            //using (StreamReader r = new StreamReader("ExternalData.json"))
            //{
            //    var json = r.ReadToEnd();
            //    var jsonObjects = JObject.Parse(json);
            //    teams = jsonObjects["teams"].ToObject<List<Team>>();
            //    historyLeagueStats = jsonObjects["historyleaguestats"].ToObject<HistoryLeagueStats>();
            //}
            //var request = RoundRobinFactory.CreateRoundRobinRequest(teams.Take(4).ToList(), historyLeagueStats);
            //var result = _RRSelector.Handle(request);
            //return (from res in result.matchResults
            //        let matchresult = JsonConvert.SerializeObject(res)
            //        select matchresult).ToList();
        }

            // POST api/<Competition>
            [HttpPost]
        public IEnumerable<string> Post([FromBody] string value)
        {
            var jsonObjects = JObject.Parse(value);
            var teams = jsonObjects["teams"].ToObject<List<Team>>();
            var historyLeagueStats = jsonObjects["historyleaguestats"].ToObject<HistoryLeagueStats>();
            var request = RoundRobinFactory.CreateRoundRobinRequest(teams.ToList(), historyLeagueStats);
            var result = _RRSelector.Handle(request);

            return (from res in result.matchResults
                    let matchresult = JsonConvert.SerializeObject(res)
                    select matchresult).ToList();

        }
    }
}
