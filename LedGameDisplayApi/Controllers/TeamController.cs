using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LedGameDisplayApi.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LedGameDisplayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ILogger<TeamController> _logger;

        public TeamController(ILogger<TeamController> logger)
        {
            _logger = logger;
        }

        // GET: api/Team
        [HttpGet]
        public IEnumerable<Team> GetTeam()
        {
            using (var dbContext = new MyDbContext())
            {
                var teamList = dbContext.Teams;

                return (teamList).ToArray();
            }
        }

        // GET: api/Team/5
        [HttpGet("{id}", Name = "GetTeam")]
        public Team GetTeam(int id)
        {
            using (var dbContext = new MyDbContext())
            {
                var team = dbContext.Teams.SingleOrDefault(x => x.Id == id);
                if (team == null)
                    _logger.LogDebug("Team {0} not found", id);
                else
                    _logger.LogDebug("Got Team {0}", id);
                return team;
            }
        }

        // POST: api/Team
        [HttpPost]
        public void PostTeam([FromBody] Team value)
        {
            value.Name = HttpUtility.UrlDecode(value.Name);
            value.Clubname = HttpUtility.UrlDecode(value.Clubname);
            value.Shortname = HttpUtility.UrlDecode(value.Shortname);

            using (var dbContext = new MyDbContext())
            {
                dbContext.Teams.Add(value);
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Added team by changing {0} datasets", changeCount);
            }
        }

        // PUT: api/Team/5
        [HttpPut("{id}")]
        public void PutTeam(int id, [FromBody] Team value)
        {
            value.Name = HttpUtility.UrlDecode(value.Name);
            value.Clubname = HttpUtility.UrlDecode(value.Clubname);
            value.Shortname = HttpUtility.UrlDecode(value.Shortname);

            using (var dbContext = new MyDbContext())
            {
                var toUpdate = dbContext.Teams.SingleOrDefault(x => x.Id == id);
                if (toUpdate == null)
                {
                    _logger.LogWarning("Team {0} not found to update.", id);
                    return;
                }

                toUpdate = value;

                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Changed team {0} by changing {1} datasets", id, changeCount);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteTeam(int id)
        {
            using (var dbContext = new MyDbContext())
            {
                var toRemove = dbContext.Teams.SingleOrDefault(x => x.Id == id);
                if (toRemove == null)
                {
                    _logger.LogWarning("Team {0} not found to remove.", id);
                    return;
                }

                dbContext.Teams.Remove(toRemove);
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Deleted team {0} by changing {1} datasets", id, changeCount);
            }
        }
    }
}
