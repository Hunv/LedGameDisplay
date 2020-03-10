using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace LedGameDisplayApi.DataModel
{
    /// <summary>
    /// This match is holding the data for an ongoing match including the counter
    /// </summary>
    public static class LiveMatchManager
    {
        private static Timer _TmrTimeLeft = new Timer(1000);
        public static List<Match> CurrentMatches { get; set; }

        private async static void _TmrTimeLeft_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var aMatch in CurrentMatches)
            {
                Console.WriteLine("ID:{0} - TimeLeft Elapsed. Time left: {1} Current time: {2}", aMatch.Id, aMatch.CurrentTimeLeft, DateTime.Now);

                if (aMatch.CurrentTimeLeft.TotalSeconds > 0 && 
                    aMatch.MatchStatus == "running")
                {
                    await TimeElapsed(aMatch.Id);
                }
                else if (aMatch.CurrentTimeLeft.TotalSeconds <= 0 &&
                    aMatch.MatchStatus == "running")
                {
                    await EndMatch(aMatch.Id);
                }
            }
        }

        public async static Task TimeElapsed(int matchId)
        {
            CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft = CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft.Subtract(new TimeSpan(0, 0, 1));

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.Single(x => x.Id == matchId);
                dbMatch.CurrentTimeLeft = CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft;

                dbContext.DisplayCommands.Add(new DisplayCommand()
                {
                    Area = "time",
                    ColorHex = "00FF00",
                    Command = "showtext",
                    Value = string.Format("{0}:{1}", CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft.Minutes, CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft.Seconds)
                }) ;
                await dbContext.SaveChangesAsync();
            }
        }

        public static void Initialize()
        {
            _TmrTimeLeft.Elapsed += _TmrTimeLeft_Elapsed;
        }

        public async static void StartHalftime(int matchId)
        {
            CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft = CurrentMatches.Single(x => x.Id == matchId).HalfTimeTime;
            CurrentMatches.Single(x => x.Id == matchId).CurrentHalfTime++;
            CurrentMatches.Single(x => x.Id == matchId).MatchStatus = "running";

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.Single(x => x.Id == matchId);
                dbMatch.CurrentTimeLeft = CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft;
                dbMatch.CurrentHalfTime = CurrentMatches.Single(x => x.Id == matchId).CurrentHalfTime;

                dbContext.DisplayCommands.Add(new DisplayCommand()
                {
                    Area = "time",
                    Command = "showtext",
                    Value = string.Format("{0}:{1}", CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft.Minutes.ToString("D2"), CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft.Seconds.ToString("D2")),
                    Expires = DateTime.Now.AddSeconds(5)
                }) ;

                await dbContext.SaveChangesAsync();
            }

            _TmrTimeLeft.Start();
        }

        public static void PauseMatch(int matchId)
        {
            Console.WriteLine("ID:{0} - Pause", matchId);
            CurrentMatches.Single(x => x.Id == matchId).MatchStatus = "paused";
        }
        public static void UnpauseMatch(int matchId)
        {
            Console.WriteLine("ID:{0} - Unpause", matchId);
            CurrentMatches.Single(x => x.Id == matchId).MatchStatus = "running";
        }

        public async static Task CancelMatch(int matchId)
        {
            Console.WriteLine("ID:{0} - Cancel", matchId);
            CurrentMatches.Single(x => x.Id == matchId).MatchStatus = "canceled";
            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.SingleOrDefault(x => x.Id == matchId);

                if (dbMatch != null)
                {
                    dbMatch.IsLive = false;
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public async static Task EndMatch(int matchId)
        {
            Console.WriteLine("ID:{0} - End", matchId);
            CurrentMatches.Single(x => x.Id == matchId).MatchStatus = "ended";

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.SingleOrDefault(x => x.Id == matchId);

                if (dbMatch != null)
                {
                    dbMatch.IsLive = false;
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public async static Task FinishMatch(int matchId)
        {
            Console.WriteLine("ID:{0} - Finish", matchId);
            CurrentMatches.Single(x => x.Id == matchId).MatchStatus = "finished";

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.SingleOrDefault(x => x.Id == matchId);

                if (dbMatch != null)
                {
                    dbMatch.IsLive = false;
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public async static void Goal(int matchId, int team)
        {
            Console.WriteLine("ID:{0} - Goal for team {1}", matchId, team);
            if (team == 1)
                CurrentMatches.Single(x => x.Id == matchId).ScoreTeam1++;
            else if (team == 2)
                CurrentMatches.Single(x => x.Id == matchId).ScoreTeam2++;

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.Single(x => x.Id == CurrentMatches.Single(x => x.Id == matchId).Id);
                dbMatch.ScoreTeam1 = CurrentMatches.Single(x => x.Id == matchId).ScoreTeam1;
                dbMatch.ScoreTeam2 = CurrentMatches.Single(x => x.Id == matchId).ScoreTeam2;
                await dbContext.SaveChangesAsync();
            }
        }

        public async static void UnGoal(int matchId, int team)
        {
            Console.WriteLine("ID:{0} - Nogoal for team {1}", matchId, team);

            if (team == 1)
                CurrentMatches.Single(x => x.Id == matchId).ScoreTeam1--;
            else if (team == 2)
                CurrentMatches.Single(x => x.Id == matchId).ScoreTeam2--;

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.Single(x => x.Id == CurrentMatches.Single(x => x.Id == matchId).Id);
                dbMatch.ScoreTeam1 = CurrentMatches.Single(x => x.Id == matchId).ScoreTeam1;
                dbMatch.ScoreTeam2 = CurrentMatches.Single(x => x.Id == matchId).ScoreTeam2;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
