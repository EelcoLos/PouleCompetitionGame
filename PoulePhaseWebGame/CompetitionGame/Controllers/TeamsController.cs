using System.Collections.Generic;
using CompetitionGame.Command;
using CompetitionGame.Data;
using CompetitionGame.Factories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using CompetitionGame.Data.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetitionGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private ICommandHandler<DataRequest, DataResult> _externalDataService;
        private List<Team> cachedTeams;
        public TeamsController(ICommandHandler<DataRequest, DataResult> externalDataService)
        {
            _externalDataService = externalDataService;
            Initialize();
        }

        private void Initialize()
        {
            var externalResult = _externalDataService.Handle(DataRequestFactory.CreateRequest());
            cachedTeams = externalResult.Teams;
        }


        // GET: api/<Teams>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //return JsonConvert.SerializeObject(cachedTeams);
            var list = new List<string>();
            foreach (var team in cachedTeams)
            {
                var res = JsonConvert.SerializeObject(team);
                //.Replace("\"", "")/*.Replace("","")*/
                list.Add(res);
            }

            return list;
        }

        // GET api/<Teams>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return JsonConvert.SerializeObject(cachedTeams[id]).Replace("\"", "");
        }
    }
}
