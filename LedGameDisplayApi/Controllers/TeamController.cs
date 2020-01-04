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
    public class TeamController : ControllerBase
    {
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
            return null;
        }

        // POST: api/Team
        [HttpPost]
        public void PostTeam([FromBody] string value)
        {
        }

        // PUT: api/Team/5
        [HttpPut("{id}")]
        public void PutTeam(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteTeam(int id)
        {
        }
    }
}
