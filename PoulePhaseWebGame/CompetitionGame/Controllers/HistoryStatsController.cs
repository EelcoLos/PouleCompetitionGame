using System.Text.Json;
using CompetitionGame.Command;
using CompetitionGame.Data;
using CompetitionGame.Factories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetitionGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryStatsController : ControllerBase
    {
        private ICommandHandler<DataRequest, DataResult> _externalDataService;

        public HistoryStatsController(ICommandHandler<DataRequest, DataResult> externalDataService) => _externalDataService = externalDataService;

        // GET: api/<HistoryStatsController>
        [HttpGet]
        public string Get() => JsonSerializer.Serialize(_externalDataService.Handle(DataRequestFactory.CreateRequest()).HistoryStats);
    }
}
