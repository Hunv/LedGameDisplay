using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LedGameDisplayApi.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LedGameDisplayApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentController : ControllerBase
    {
        private readonly ILogger<TournamentController> _logger;

        public TournamentController(ILogger<TournamentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Tournament> GetTournament()
        {
            using (var dbContext = new MyDbContext())
            {
                var tournamentList = dbContext.Tournaments;
                _logger.LogDebug("Got {0} tournaments", tournamentList.Count());
                return (tournamentList).ToArray();
            }
        }


        // GET: api/Tournament/5
        [HttpGet("{id}", Name = "GetTournament")]
        public Tournament GetTournament(int id)
        {
            using (var dbContext = new MyDbContext())
            {
                var tournament = dbContext.Tournaments.SingleOrDefault(x => x.Id == id);
                if (tournament == null)
                    _logger.LogDebug("Tournament {0} not found", id);
                else
                    _logger.LogDebug("Got tournament {0}", id);
                return tournament;
            }
        }

        // POST: api/Tournament
        //Example Tournament String: {"name":"Schlickteufel Cup 2020","city":"Elmshorn","place":"Traglufthalle"}
        [HttpPost]
        public void PostTournament([FromBody] Tournament value)
        {
            using (var dbContext = new MyDbContext())
            {
                dbContext.Tournaments.Add(value);
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Added tournament by changing {0} datasets", changeCount);
            }
        }

        // PUT: api/Tournament/5
        [HttpPut("{id}")]
        public void PutTournament(int id, [FromBody] Tournament value)
        {
            using (var dbContext = new MyDbContext())
            {
                var toUpdate = dbContext.Tournaments.SingleOrDefault(x => x.Id == id);
                if (toUpdate == null)
                {
                    _logger.LogWarning("Tournament {0} not found to update.", id);
                    return;
                }

                toUpdate = value;
                
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Changed tournament {0} by changing {1} datasets", id, changeCount);
            }
        }

        // DELETE: api/Tournament/5
        [HttpDelete("{id}")]
        public void DeleteTournament(int id)
        {
            using (var dbContext = new MyDbContext())
            {
                var toRemove = dbContext.Tournaments.SingleOrDefault(x => x.Id == id);
                if (toRemove == null)
                {
                    _logger.LogWarning("Tournament {0} not found to remove.", id);
                    return;
                }

                dbContext.Tournaments.Remove(toRemove);
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Deleted tournament {0} by changing {1} datasets", id, changeCount);
            }
        }
    }
}
