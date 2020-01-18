using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(ILogger<PlayerController> logger)
        {
            _logger = logger;
        }

        // GET: api/Player
        [HttpGet]
        public IActionResult GetPlayer()
        {
            using (var DatabaseContext = new DatabaseContext())
            {
                var playerList = DatabaseContext.Players.Include("Team").ToArray();

                var js = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                };
                var json = JsonConvert.SerializeObject(playerList, js);
                var result = new OkObjectResult(json);
                

                return result;
            }
        }

        // GET: api/Player/5
        [HttpGet("{id}", Name = "GetPlayer")]
        public Player GetPlayer(int id)
        {
            using (var DatabaseContext = new DatabaseContext())
            {
                var player = DatabaseContext.Players.SingleOrDefault(x => x.Id == id);
                if (player == null)
                    _logger.LogDebug("Player {0} not found", id);
                else
                    _logger.LogDebug("Got player {0}", id);
                return player;
            }
        }

        // POST: api/Player
        [HttpPost]
        public void PostPlayer([FromBody] NewPlayerData value)
        {
            value.Firstname = HttpUtility.UrlDecode(value.Firstname);
            value.Lastname = HttpUtility.UrlDecode(value.Lastname);

            using (var DatabaseContext = new DatabaseContext())
            {
                var newPlayer = new Player()
                {
                    Birthday = value.Birthday,
                    Firstname = value.Firstname,
                    Lastname = value.Lastname,
                    IsActive = value.IsActive,
                    IsCaptain = value.IsCaptain,
                    IsViceCaptain = value.IsViceCaptain,
                    HealthCertificationExpireDate = value.HealthCertificationExpireDate,
                    RefereeLevel = value.RefereeLevel,
                    RefereeLevelExpireDate = value.RefereeLevelExpireDate,
                    Sex = value.Sex,
                    Team = DatabaseContext.Teams.Single(x => x.Id == value.TeamId),
                    Created = DateTime.Now
                };

                DatabaseContext.Players.Add(newPlayer);
                var changeCount = DatabaseContext.SaveChanges();
                _logger.LogDebug("Added player by changing {0} datasets", changeCount);
            }
        }

        // PUT: api/Player/5
        [HttpPut("{id}")]
        public void PutPlayer(int id, [FromBody] Player value)
        {
            value.Firstname = HttpUtility.UrlDecode(value.Firstname);
            value.Lastname = HttpUtility.UrlDecode(value.Lastname);

            using (var DatabaseContext = new DatabaseContext())
            {
                var toUpdate = DatabaseContext.Players.SingleOrDefault(x => x.Id == id);
                if (toUpdate == null)
                {
                    _logger.LogWarning("Player {0} not found to update.", id);
                    return;
                }

                toUpdate = value;

                var changeCount = DatabaseContext.SaveChanges();
                _logger.LogDebug("Changed player {0} by changing {1} datasets", id, changeCount);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeletePlayer(int id)
        {
            using (var DatabaseContext = new DatabaseContext())
            {
                var toRemove = DatabaseContext.Players.SingleOrDefault(x => x.Id == id);
                if (toRemove == null)
                {
                    _logger.LogWarning("Player {0} not found to remove.", id);
                    return;
                }

                DatabaseContext.Players.Remove(toRemove);
                var changeCount = DatabaseContext.SaveChanges();
                _logger.LogDebug("Deleted player {0} by changing {1} datasets", id, changeCount);
            }
        }
    }
}
