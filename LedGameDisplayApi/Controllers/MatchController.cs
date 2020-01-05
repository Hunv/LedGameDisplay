using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LedGameDisplayApi.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LedGameDisplayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly ILogger<MatchController> _logger;

        public MatchController(ILogger<MatchController> logger)
        {
            _logger = logger;
        }

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
            using (var dbContext = new MyDbContext())
            {
                var match = dbContext.Matches.SingleOrDefault(x => x.Id == id);
                if (match == null)
                    _logger.LogDebug("Matches {0} not found", id);
                else
                    _logger.LogDebug("Got match {0}", id);
                return match;
            }
        }

        // POST: api/Match
        [HttpPost]
        public void PostMatch([FromBody] Match value)
        {
            using (var dbContext = new MyDbContext())
            {
                dbContext.Matches.Add(value);
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Added match by changing {0} datasets", changeCount);
            }
        }

        // PUT: api/Match/5
        [HttpPut("{id}")]
        public void PutMatch(int id, [FromBody] Match value)
        {
            using (var dbContext = new MyDbContext())
            {
                var toUpdate = dbContext.Matches.SingleOrDefault(x => x.Id == id);
                if (toUpdate == null)
                {
                    _logger.LogWarning("Match {0} not found to update.", id);
                    return;
                }

                toUpdate = value;

                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Changed match {0} by changing {1} datasets", id, changeCount);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteMatch(int id)
        {
            using (var dbContext = new MyDbContext())
            {
                var toRemove = dbContext.Matches.SingleOrDefault(x => x.Id == id);
                if (toRemove == null)
                {
                    _logger.LogWarning("Match {0} not found to remove.", id);
                    return;
                }

                dbContext.Matches.Remove(toRemove);
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Deleted match {0} by changing {1} datasets", id, changeCount);
            }
        }
    }
}
