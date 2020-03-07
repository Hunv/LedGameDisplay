using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LedGameDisplayApi.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Web;
using LedGameDisplayApi.DataModel.JsonModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public IActionResult GetTournament()
        {
            using (var dbContext = new DatabaseContext())
            {
                var tournamentList = dbContext.Tournaments.Include("Matches");

                var js = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                };
                var json = JsonConvert.SerializeObject(tournamentList, js);
                var result = new OkObjectResult(json);

                _logger.LogDebug("Got {0} tournaments", tournamentList.Count());
                return result;
            }
        }


        // GET: api/Tournament/5
        [HttpGet("{id}", Name = "GetTournament")]
        public IActionResult GetTournament(int id)
        {
            using (var dbContext = new DatabaseContext())
            {
                var tournament = dbContext.Tournaments.Include("Matches").Include("Matches.Team1").Include("Matches.Team2").SingleOrDefault(x => x.Id == id);
                if (tournament == null)
                    _logger.LogDebug("Tournament {0} not found", id);
                else
                    _logger.LogDebug("Got tournament {0}", id);


                var js = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                };
                var json = JsonConvert.SerializeObject(tournament, js);
                var result = new OkObjectResult(json);


                return result;
            }
        }

        // POST: api/Tournament
        //Example Tournament String: {"name":"Schlickteufel Cup 2020","city":"Elmshorn","place":"Traglufthalle","date":"2019-03-28T00:00:00"}
        [HttpPost]
        public void PostTournament([FromBody] NewTournamentData value)
        {
            value.City = HttpUtility.UrlDecode(value.City);
            value.Name = HttpUtility.UrlDecode(value.Name);
            value.Place = HttpUtility.UrlDecode(value.Place);

            var tournament = new Tournament()
            {
                City = value.City,
                Name = value.Name,
                Place = value.Place,
                Date = value.Date
            };

            using (var dbContext = new DatabaseContext())
            {
                dbContext.Tournaments.Add(tournament);
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Added tournament by changing {0} datasets", changeCount);
            }
        }

        // PUT: api/Tournament/5
        [HttpPut("{id}")]
        public void PutTournament(int id, [FromBody] Tournament value)
        {
            value.City = HttpUtility.UrlDecode(value.City);
            value.Name = HttpUtility.UrlDecode(value.Name);
            value.Place = HttpUtility.UrlDecode(value.Place);

            using (var dbContext = new DatabaseContext())
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
            using (var dbContext = new DatabaseContext())
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
