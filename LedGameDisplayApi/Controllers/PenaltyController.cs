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
    public class PenaltyController : ControllerBase
    {
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
            return null;
        }

        // POST: api/Penalty
        [HttpPost]
        public void PostPenalty([FromBody] string value)
        {
        }

        // PUT: api/Penalty/5
        [HttpPut("{id}")]
        public void PutPenalty(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeletePenalty(int id)
        {
        }
    }
}
