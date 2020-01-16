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
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(ILogger<PlayerController> logger)
        {
            _logger = logger;
        }

        // GET: api/Player
        [HttpGet]
        public IEnumerable<Player> GetPlayer()
        {
            using (var dbContext = new MyDbContext())
            {
                var playerList = dbContext.Players;

                return (playerList).ToArray();
            }
        }

        // GET: api/Player/5
        [HttpGet("{id}", Name = "GetPlayer")]
        public Player GetPlayer(int id)
        {
            using (var dbContext = new MyDbContext())
            {
                var player = dbContext.Players.SingleOrDefault(x => x.Id == id);
                if (player == null)
                    _logger.LogDebug("Player {0} not found", id);
                else
                    _logger.LogDebug("Got player {0}", id);
                return player;
            }
        }

        // POST: api/Player
        [HttpPost]
        public void PostPlayer([FromBody] Player value)
        {
            value.Firstname = HttpUtility.UrlDecode(value.Firstname);
            value.Lastname = HttpUtility.UrlDecode(value.Lastname);

            using (var dbContext = new MyDbContext())
            {
                dbContext.Players.Add(value);
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Added player by changing {0} datasets", changeCount);
            }
        }

        // PUT: api/Player/5
        [HttpPut("{id}")]
        public void PutPlayer(int id, [FromBody] Player value)
        {
            value.Firstname = HttpUtility.UrlDecode(value.Firstname);
            value.Lastname = HttpUtility.UrlDecode(value.Lastname);

            using (var dbContext = new MyDbContext())
            {
                var toUpdate = dbContext.Players.SingleOrDefault(x => x.Id == id);
                if (toUpdate == null)
                {
                    _logger.LogWarning("Player {0} not found to update.", id);
                    return;
                }

                toUpdate = value;

                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Changed player {0} by changing {1} datasets", id, changeCount);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeletePlayer(int id)
        {
            using (var dbContext = new MyDbContext())
            {
                var toRemove = dbContext.Players.SingleOrDefault(x => x.Id == id);
                if (toRemove == null)
                {
                    _logger.LogWarning("Player {0} not found to remove.", id);
                    return;
                }

                dbContext.Players.Remove(toRemove);
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Deleted player {0} by changing {1} datasets", id, changeCount);
            }
        }
    }
}
