using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LedGameDisplayApi.DataModel;
using LedGameDisplayApi.DataModel.JsonModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            using (var dbContext = new DatabaseContext())
            {
                var matchList = dbContext.Matches;

                return (matchList).ToArray();
            }
        }

        // GET: api/Match/5
        [HttpGet("{id}", Name = "GetMatch")]
        public IActionResult GetMatch(int id)
        {
            using (var dbContext = new DatabaseContext())
            {
                var match = dbContext.Matches
                    .Include("Team1")
                    .Include("Team2")
                    .Include("Players")
                    .Include("Referees")
                    .Include("Penalties")
                    .SingleOrDefault(x => x.Id == id);

                if (match == null)
                    _logger.LogDebug("Match {0} not found", id);
                else
                    _logger.LogDebug("Got match {0}", id);

                var js = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                };
                var json = JsonConvert.SerializeObject(match, js);
                var result = new OkObjectResult(json);
                return result;
            }
        }

        // POST: api/Match
        [HttpPost]
        public void PostMatch([FromBody] NewMatchData value)
        {
            using (var dbContext = new DatabaseContext())
            {
                var match = new Match()
                {
                    StartPlaned = value.StartPlaned,
                    Team1 = dbContext.Teams.Single(x => x.Id == value.Team1Id),
                    Team2 = dbContext.Teams.Single(x => x.Id == value.Team2Id),
                    Referees = new List<DbMatch2PlayerReferee>(),
                    HalfTimeAmount = value.HalfTimeAmount,
                    HalfTimeTime = new TimeSpan(0, 0, value.HalfTimeSeconds),
                    Tournament = dbContext.Tournaments.Single(x => x.Id == value.TournamentId)
                };

                foreach (var aRef in value.RefereeIds)
                {
                    if (aRef == 0) continue;

                    var a = new DbMatch2PlayerReferee()
                    {
                        Referee = dbContext.Players.Single(x => x.Id == aRef),
                        Match = match
                    };
                    match.Referees.Add(a);
                }

                dbContext.Matches.Add(match);
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Added match by changing {0} datasets", changeCount);
            }
        }

        // PUT: api/Match/5
        [HttpPut("{id}")]
        public void PutMatch(int id, [FromBody] UpdateMatchOngoing value)
        {
            using (var dbContext = new DatabaseContext())
            {
                var toUpdate = dbContext.Matches.SingleOrDefault(x => x.Id == id);
                if (toUpdate == null)
                {
                    _logger.LogWarning("Match {0} not found to update.", id);
                    return;
                }

                toUpdate.CurrentHalfTime = value.CurrentHalfTime;
                toUpdate.CurrentTimeLeft = value.CurrentTimeLeft;
                toUpdate.ScoreTeam1 = value.ScoreTeam1;
                toUpdate.ScoreTeam2 = value.ScoreTeam2;
                toUpdate.StartActual = value.StartActual;

                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Changed match {0} by changing {1} datasets", id, changeCount);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteMatch(int id)
        {
            using (var dbContext = new DatabaseContext())
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
