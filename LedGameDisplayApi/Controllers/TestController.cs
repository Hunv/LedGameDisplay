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
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        // GET: api/LiveMatch
        //[HttpGet]
        //public Match GetLiveMatch()
        //{   
        //    //Load Live Match from DB, if there is no Live Match set. The result can be null if there is really no livematch
        //    if (LiveMatch.CurrentMatch == null)
        //    {
        //        using (var dbContext = new DatabaseContext())
        //        {
        //            LiveMatch.CurrentMatch = dbContext.Matches.Include("Team1").Include("Team2").SingleOrDefault(x => x.IsLive);
        //        }
                
        //        LiveMatch.Initialize();
        //    }

        //    return LiveMatch.CurrentMatch;

        //}

        // POST: api/Test/{area}/{text}
        [HttpPost("{area}/{text}")]
        public async void PostMatch(string area, string text)
        {
            using (var dbContext = new DatabaseContext())
            {
                //dbContext.Matches.Include("Team1").Include("Team2").SingleOrDefault(x => x.IsLive);
                dbContext.DisplayCommands.Add(new DisplayCommand()
                {
                    Area = "team1goals",
                    Command = "showtext",
                    Value = "0"
                });
                dbContext.DisplayCommands.Add(new DisplayCommand()
                {
                    Area = "team2goals",
                    Command = "showtext",
                    Value = "0"
                });
                dbContext.DisplayCommands.Add(new DisplayCommand()
                {
                    Area = "time",
                    Command = "showtext",
                    Value = "10:00",
                    Expires = DateTime.Now.AddSeconds(5)
                });
                await dbContext.SaveChangesAsync();
            }
        }

        // PUT: api/LiveMatch?action={action}
        //[HttpPut]
        //public void PutMatch([FromQuery] string action)
        //{
        //    using (var dbContext = new DatabaseContext())
        //    {
        //        //Load Live Match from DB, if there is no Live Match set. The result can be null if there is really no livematch
        //        if (LiveMatch.CurrentMatch == null)
        //        {
        //            LiveMatch.CurrentMatch = dbContext.Matches.SingleOrDefault(x => x.IsLive);
        //            LiveMatch.Initialize();
        //        }

        //        if (LiveMatch.CurrentMatch == null)
        //        {
        //            _logger.LogWarning("Currently no match is live");
        //            return;
        //        }

        //        switch (action)
        //        {
        //            case "start":
        //                LiveMatch.CurrentMatch.StartActual = DateTime.Now;
        //                LiveMatch.StartHalftime();
        //                break;
        //            case "pause":
        //                LiveMatch.PauseMatch();
        //                break;
        //            case "unpause":
        //                LiveMatch.UnpauseMatch();
        //                break;
        //            case "cancel":
        //                LiveMatch.CancelMatch();
        //                LiveMatch.CurrentMatch.EndActual = DateTime.Now;
        //                break;
        //            case "finish":
        //                LiveMatch.FinishMatch();
        //                LiveMatch.CurrentMatch.EndActual = DateTime.Now;
        //                break;
        //            case "goal1":
        //                LiveMatch.Goal(1);
        //                break;
        //            case "goal2":
        //                LiveMatch.Goal(2);
        //                break;

        //        }
        //        var changeCount = dbContext.SaveChanges();
        //        _logger.LogDebug("Changed livematch action {0} by changing {1} datasets", action, changeCount);
        //    }
        //}

        // DELETE: api/LiveMatch
        //[HttpDelete]
        //public void DeleteLiveMatch()
        //{
        //    using (var dbContext = new DatabaseContext())
        //    {
        //        //Load Live Match from DB, if there is no Live Match set. The result can be null if there is really no livematch
        //        if (LiveMatch.CurrentMatch == null)
        //        {
        //            LiveMatch.CurrentMatch = dbContext.Matches.SingleOrDefault(x => x.IsLive);
        //            LiveMatch.Initialize();
        //        }

        //        if (LiveMatch.CurrentMatch == null)
        //        {
        //            _logger.LogWarning("Currently no match is live");
        //            return;
        //        }

        //        var toBeNonLive = dbContext.Matches.SingleOrDefault(x => x.Id == LiveMatch.CurrentMatch.Id);
        //        if (toBeNonLive == null)
        //        {
        //            _logger.LogWarning("Livematch not found to stop.");
        //            return;
        //        }
        //        toBeNonLive.IsLive = false;
        //        var changeCount = dbContext.SaveChanges();
        //        _logger.LogDebug("Set Livematch {0} to Non-Live by changing {1} datasets", toBeNonLive.Id, changeCount);

        //        LiveMatch.CurrentMatch = null;
        //    }
        //}
    }
}
