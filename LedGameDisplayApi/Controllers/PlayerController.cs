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
    public class PlayerController : ControllerBase
    {
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
            return null;
        }

        // POST: api/Player
        [HttpPost]
        public void PostPlayer([FromBody] string value)
        {
        }

        // PUT: api/Player/5
        [HttpPut("{id}")]
        public void PutPlayer(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeletePlayer(int id)
        {
        }
    }
}
