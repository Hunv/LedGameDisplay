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
    public class LiveMatchController : ControllerBase
    {
        private readonly ILogger<LiveMatchController> _logger;

        public LiveMatchController(ILogger<LiveMatchController> logger)
        {
            _logger = logger;
        }

        // GET: api/LiveMatch
        [HttpGet]
        public List<Match> GetLiveMatches()
        {   
            //Load Live Match from DB, if there is no Live Match set. The result can be null if there is really no livematch
            if (LiveMatchManager.CurrentMatches == null || LiveMatchManager.CurrentMatches.Count == 0)
            {
                using (var dbContext = new DatabaseContext())
                {
                    var liveMatches = dbContext.Matches.Include("Team1").Include("Team2").Where(x => x.IsLive);
                    LiveMatchManager.CurrentMatches = new List<Match>(liveMatches);
                }

                LiveMatchManager.Initialize();
            }

            return LiveMatchManager.CurrentMatches;

        }

        // PUT: api/LiveMatch/id?action={action}&param1={param1}&param2={param2}
        [HttpPut("{id}")]
        public async Task PutMatch(int id, [FromQuery] string action, [FromQuery] string param1 = null, [FromQuery] string param2 = null)
        {
            using (var dbContext = new DatabaseContext())
            {
                //Load Live Match from DB, if there is no Live Match set. The result can be null if there is really no livematch
                if (LiveMatchManager.CurrentMatches == null || LiveMatchManager.CurrentMatches.Count == 0)
                {
                    var liveMatches = dbContext.Matches.Include("Team1").Include("Team2").Where(x => x.IsLive);
                    LiveMatchManager.CurrentMatches = new List<Match>(liveMatches);
                    LiveMatchManager.Initialize();
                }

                //if (LiveMatchManager.CurrentMatches == null || LiveMatchManager.CurrentMatches.Count == 0)
                //{
                //    _logger.LogWarning("Currently no match is live");
                //    return;
                //}

                // Get the given match and add it to the list of Current matches
                if (LiveMatchManager.CurrentMatches.Count(x => x.Id == id) == 0)
                {
                    LiveMatchManager.CurrentMatches.Add(dbContext.Matches.Include("Team1").Include("Team2").Single(x => x.Id == id));
                }

                switch (action)
                {
                    case "prepare": //Prepare a match/halftime to start. The match will be marked as live but the time will not run. 
                        await LiveMatchManager.ShowInitScreen(id);
                        break;
                    case "start": //start a match/halftime. The match will be marked as live and the time will start to run.
                        await LiveMatchManager.StartHalftime(id);
                        break;
                    case "pause": //pause a match. The match will be marked as live but the time will stop running
                        await LiveMatchManager.PauseMatch(id);
                        break;
                    case "unpause": //unpause a match. The match will be marked as live and the time will start running
                        await LiveMatchManager.UnpauseMatch(id);
                        break;
                    case "cancel": //cancel a match. The match will be unmarked as live
                        await LiveMatchManager.CancelMatch(id);
                        break;
                    case "end": //a match time is over. The match will still be marked as live until "finish" is triggerd 
                        await LiveMatchManager.EndMatch(id);
                        break;
                    case "finish": //a match time is over. The match will be unmarked as live 
                        await LiveMatchManager.FinishMatch(id);
                        break;
                    case "goal1": //Team 1 scores a goal
                        LiveMatchManager.Goal(id, 1);
                        break;
                    case "goal2": //Team 2 scores a goal
                        LiveMatchManager.Goal(id, 2);
                        break;
                    case "nogoal1": //Undo a Team 1 score
                        LiveMatchManager.UnGoal(id, 1);
                        break;
                    case "nogoal2": //Undo a Team 2 score
                        LiveMatchManager.UnGoal(id, 2);
                        break;
                    case "penalty": //A player did a penalty
                        await LiveMatchManager.Penalty(id, int.Parse(param1), param2) ;
                        break;

                }
                //var changeCount = dbContext.SaveChanges();
                //_logger.LogDebug("Changed livematch action {0} by changing {1} datasets", action, changeCount);
            }
        }

        // DELETE: api/LiveMatch/id
        [HttpDelete("{id}")]
        public void DeleteLiveMatch(int id)
        {
            using (var dbContext = new DatabaseContext())
            {
                //Load Live Match from DB, if there is no Live Match set. The result can be null if there is really no livematch
                if (LiveMatchManager.CurrentMatches == null || LiveMatchManager.CurrentMatches.Count == 0)
                {
                    var liveMatches = dbContext.Matches.Include("Team1").Include("Team2").Where(x => x.IsLive);
                    LiveMatchManager.CurrentMatches = new List<Match>(liveMatches);
                    LiveMatchManager.Initialize();
                }

                if (LiveMatchManager.CurrentMatches == null || LiveMatchManager.CurrentMatches.Count == 0)
                {
                    _logger.LogWarning("Currently no match is live");
                    return;
                }

                var toBeNonLive = dbContext.Matches.SingleOrDefault(x => x.Id == id);
                if (toBeNonLive == null)
                {
                    _logger.LogWarning("Match with id {0} not found to stop.", id);
                    return;
                }
                toBeNonLive.IsLive = false;
                var changeCount = dbContext.SaveChanges();
                _logger.LogDebug("Set Match {0} to Non-Live by changing {1} datasets", toBeNonLive.Id, changeCount);
            }
        }
    }
}
