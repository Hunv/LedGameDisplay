using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LedGameDisplayApi.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LedGameDisplayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        // GET: api/Match
        [HttpGet]
        public IEnumerable<Match> GetMatch()
        {
            using (var dbContext = new MyDbContext())
            {
                var matchList = dbContext.Matches;

                return (matchList).ToArray();
            }
        }

        // GET: api/Match/5
        [HttpGet("{id}", Name = "GetMatch")]
        public Match GetMatch(int id)
        {
            return null;
        }

        // POST: api/Match
        [HttpPost]
        public void PostMatch([FromBody] string value)
        {
        }

        // PUT: api/Match/5
        [HttpPut("{id}")]
        public void PutMatch(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteMatch(int id)
        {
        }
    }
}
