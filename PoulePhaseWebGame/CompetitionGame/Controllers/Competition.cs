using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetitionGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Competition : ControllerBase
    {
        // GET: api/<Competition>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Competition>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Competition>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Competition>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Competition>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
