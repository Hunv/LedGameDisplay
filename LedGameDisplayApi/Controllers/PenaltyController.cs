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
    public class PenaltyController : ControllerBase
    {
        private readonly ILogger<PenaltyController> _logger;

        public PenaltyController(ILogger<PenaltyController> logger)
        {
            _logger = logger;
        }

        // GET: api/Penalty
        [HttpGet]
        public IEnumerable<Penalty> GetPenalty()
        {
            using (var dbContext = new MyDbContext())
            {
                var penaltyList = dbContext.Penalties;

                return (penaltyList).ToArray();
            }
        }

        // GET: api/Penalty/5
        [HttpGet("{id}", Name = "GetPenalty")]
        public Penalty GetPenalty(int id)
        {
            using (var dbContext = new MyDbContext())
            {
                var penalty = dbContext.Penalties.SingleOrDefault(x => x.Id == id);
                if (penalty == null)
                    _logger.LogDebug("Penalty {0} not found", id);
                else
                    _logger.LogDebug("Got penalty {0}", id);
                return penalty;
            }
        }

        // POST: api/Penalty
        [HttpPost]
        public void PostPenalty([FromBody] Penalty value)
        {
            using (var dbContext = new MyDbContext())
            {
                dbContext.Penalties.Add(value);
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Added penalty by changing {0} datasets", changeCount);
            }
        }

        // PUT: api/Penalty/5
        [HttpPut("{id}")]
        public void PutPenalty(int id, [FromBody] Penalty value)
        {
            using (var dbContext = new MyDbContext())
            {
                var toUpdate = dbContext.Penalties.SingleOrDefault(x => x.Id == id);
                if (toUpdate == null)
                {
                    _logger.LogWarning("Penalty {0} not found to update.", id);
                    return;
                }

                toUpdate = value;

                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Changed penalty {0} by changing {1} datasets", id, changeCount);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeletePenalty(int id)
        {
            using (var dbContext = new MyDbContext())
            {
                var toRemove = dbContext.Penalties.SingleOrDefault(x => x.Id == id);
                if (toRemove == null)
                {
                    _logger.LogWarning("Penalty {0} not found to remove.", id);
                    return;
                }

                dbContext.Penalties.Remove(toRemove);
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Deleted penalty {0} by changing {1} datasets", id, changeCount);
            }
        }
    }
}
