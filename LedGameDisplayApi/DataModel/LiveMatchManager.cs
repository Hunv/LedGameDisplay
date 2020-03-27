using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using LedGameDisplayLibrary;

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
                //Console.WriteLine("ID:{0} - TimeLeft Elapsed. Time left: {1} Current time: {2}", aMatch.Id, aMatch.CurrentTimeLeft, DateTime.Now);

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
            try
            {
                CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft = CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft.Subtract(new TimeSpan(0, 0, 1));

                using (var dbContext = new DatabaseContext())
                {
                    DisplayManager dM = new DisplayManager();
                    dM.ShowText(string.Format("{0}:{1}", CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft.Minutes, CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft.Seconds.ToString("D2")),
                        AreaName.Time,
                        DateTime.Now.AddSeconds(1));

                    var dbMatch = dbContext.Matches.Single(x => x.Id == matchId);
                    dbMatch.CurrentTimeLeft = CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception) { }
        }

        public static void Initialize()
        {
            _TmrTimeLeft.Elapsed += _TmrTimeLeft_Elapsed;
        }

        public async static Task StartHalftime(int matchId)
        {
            var match = CurrentMatches.Single(x => x.Id == matchId);
            match.CurrentTimeLeft = CurrentMatches.Single(x => x.Id == matchId).HalfTimeTime;
            match.CurrentHalfTime++;
            match.MatchStatus = "running";
            match.IsLive = true;
            match.StartActual = DateTime.Now;

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.Single(x => x.Id == matchId);
                dbMatch.CurrentTimeLeft = CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft;
                dbMatch.CurrentHalfTime = CurrentMatches.Single(x => x.Id == matchId).CurrentHalfTime;
                dbMatch.IsLive = match.IsLive;
                dbMatch.MatchStatus = match.MatchStatus;
                dbMatch.StartActual = match.StartActual;

                await dbContext.SaveChangesAsync();

                DisplayManager dM = new DisplayManager();
                dM.ShowText(string.Format("{0}:{1}", CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft.Minutes.ToString("D2"), CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft.Seconds.ToString("D2")),
                    AreaName.Time,
                    DateTime.Now.AddSeconds(2));
            }

            _TmrTimeLeft.Start();
        }

        public static async Task ShowInitScreen(int matchId)
        {
            var match = CurrentMatches.Single(x => x.Id == matchId);

            //Only count the Halftime, if the previous halftime is over
            if (match.CurrentTimeLeftSeconds == 0)
            {
                match.CurrentHalfTime++;
                match.CurrentTimeLeft = match.HalfTimeTime;
            }
            match.IsLive = true;

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.Single(x => x.Id == matchId);

                DisplayManager dM = new DisplayManager();
                dM.ShowText(string.Format("{0}:{1}", CurrentMatches.Single(x => x.Id == matchId).HalfTimeTime.Minutes.ToString("D2"), CurrentMatches.Single(x => x.Id == matchId).CurrentTimeLeft.Seconds.ToString("D2")),
                    AreaName.Time);
                dM.ShowText(dbMatch.ScoreTeam1.ToString(),
                    AreaName.Team1Goals);
                dM.ShowText(dbMatch.ScoreTeam2.ToString(),
                    AreaName.Team2Goals);
                dM.ShowText(dbMatch.Team1 == null ? "Team1" : dbMatch.Team1.Name ?? "Team1",
                    AreaName.Team1Name);
                dM.ShowText(dbMatch.Team2 == null ? "Team2" : dbMatch.Team2.Name ?? "Team2",
                    AreaName.Team2Name);
                dM.ShowText(":",
                    AreaName.GoalDivider);

                dbMatch.CurrentHalfTime = match.CurrentHalfTime;
                dbMatch.CurrentTimeLeft = match.CurrentTimeLeft;
                dbMatch.IsLive = match.IsLive;
                await dbContext.SaveChangesAsync();
            }
        }

        public static async Task Penalty(int matchId, int playerId, string penaltyStr)
        {
            Console.WriteLine("ID:{0} - Penalty", matchId);
            var match = CurrentMatches.Single(x => x.Id == matchId);
            match.IsLive = true;
            if (match.StopTimeInLast2Minutes && match.CurrentTimeLeftSeconds <= 120)
            {
                _TmrTimeLeft.Stop();
                match.MatchStatus = "paused";
            }

            DataModel.Penalty penalty = null;
            if (playerId != 0 && !string.IsNullOrEmpty(penaltyStr))
            {
                var penaltyTime = 0;
                var regEx = new Regex("\\d?");
                if (regEx.IsMatch(penaltyStr.Split('\'')[0]))
                {
                    penaltyTime = Convert.ToInt32(penaltyStr.Split('\'')[0]);
                }

                if (penaltyStr.EndsWith("team")) //Team Penalty
                {
                    penalty = new DataModel.Penalty()
                    {
                        IsDismiss = penaltyStr == "000",
                        IsTeamPenalty = true,
                        Team = new Team() { Id = playerId },
                        TimePlayed = match.CurrentTimeLeft,
                        HalftimePlayed = match.CurrentHalfTime,
                        TimeLeft = penaltyTime
                    };
                }
                else //Player Penalty
                {
                    penalty = new DataModel.Penalty()
                    {
                        IsDismiss = penaltyStr == "000",
                        IsTeamPenalty = false,
                        Team = new Team() { Id = playerId },
                        TimePlayed = match.CurrentTimeLeft,
                        HalftimePlayed = match.CurrentHalfTime,
                        TimeLeft = penaltyTime
                    };
                }
            }

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.SingleOrDefault(x => x.Id == matchId);

                if (dbMatch != null)
                {
                    dbMatch.Penalties.Add(penalty);
                    dbMatch.IsLive = match.IsLive;
                    dbMatch.MatchStatus = match.MatchStatus;
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public static async Task PauseMatch(int matchId)
        {
            Console.WriteLine("ID:{0} - Pause", matchId);
            var match = CurrentMatches.Single(x => x.Id == matchId);
            match.MatchStatus = "paused";
            match.IsLive = true;

            _TmrTimeLeft.Stop();

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.SingleOrDefault(x => x.Id == matchId);

                if (dbMatch != null)
                {
                    dbMatch.IsLive = match.IsLive;
                    dbMatch.MatchStatus = match.MatchStatus;
                    await dbContext.SaveChangesAsync();
                }
            }
        }
        public static async Task UnpauseMatch(int matchId)
        {
            Console.WriteLine("ID:{0} - Unpause", matchId);
            var match = CurrentMatches.Single(x => x.Id == matchId);
            match.MatchStatus = "running";
            match.IsLive = true;

            _TmrTimeLeft.Start();

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.SingleOrDefault(x => x.Id == matchId);

                if (dbMatch != null)
                {
                    dbMatch.IsLive = match.IsLive;
                    dbMatch.MatchStatus = match.MatchStatus;
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public async static Task CancelMatch(int matchId)
        {
            Console.WriteLine("ID:{0} - Cancel", matchId);
            var match = CurrentMatches.Single(x => x.Id == matchId);
            match.MatchStatus = "canceled";
            match.IsLive = false;
            match.EndActual = DateTime.Now;

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.SingleOrDefault(x => x.Id == matchId);

                if (dbMatch != null)
                {
                    dbMatch.IsLive = false;
                    dbMatch.EndActual = match.EndActual;
                    dbMatch.MatchStatus = match.MatchStatus;
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public async static Task EndMatch(int matchId)
        {
            Console.WriteLine("ID:{0} - End", matchId);
            var match = CurrentMatches.Single(x => x.Id == matchId);
            match.MatchStatus = "ended";
            match.IsLive = true;
            match.EndActual = DateTime.Now;

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.SingleOrDefault(x => x.Id == matchId);

                if (dbMatch != null)
                {
                    dbMatch.IsLive = false;
                    dbMatch.EndActual = match.EndActual;
                    dbMatch.MatchStatus = match.MatchStatus;
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public async static Task FinishMatch(int matchId)
        {
            Console.WriteLine("ID:{0} - Finish", matchId);
            var match = CurrentMatches.Single(x => x.Id == matchId);
            match.MatchStatus = "finished";
            match.IsLive = false;
            //Only set the value if it is not already set by EndMatch
            if (match.EndActual < DateTime.Now.AddDays(-7))
                match.EndActual = DateTime.Now;

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.SingleOrDefault(x => x.Id == matchId);

                if (dbMatch != null)
                {
                    dbMatch.IsLive = false;
                    //Only set the value if it is not already set by EndMatch
                    if (match.EndActual < DateTime.Now.AddDays(-7))
                        dbMatch.EndActual = match.EndActual;
                    dbMatch.MatchStatus = match.MatchStatus;
                    await dbContext.SaveChangesAsync();
                }
            }

            CurrentMatches.Remove(match);
        }

        public async static void Goal(int matchId, int team)
        {
            Console.WriteLine("ID:{0} - Goal for team {1}", matchId, team);

            DisplayManager dM = new DisplayManager();
            var match = CurrentMatches.Single(x => x.Id == matchId);
            if (team == 1)
            {
                match.ScoreTeam1++;
                dM.ShowText(match.ScoreTeam1.ToString(),
                    AreaName.Team1Goals);
            }
            else if (team == 2)
            {
                match.ScoreTeam2++;
                dM.ShowText(match.ScoreTeam2.ToString(),
                    AreaName.Team2Goals);
            }

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.Single(x => x.Id == match.Id);
                dbMatch.ScoreTeam1 = match.ScoreTeam1;
                dbMatch.ScoreTeam2 = match.ScoreTeam2;
                await dbContext.SaveChangesAsync();
            }
        }

        public async static void UnGoal(int matchId, int team)
        {
            Console.WriteLine("ID:{0} - Nogoal for team {1}", matchId, team);

            DisplayManager dM = new DisplayManager();
            var match = CurrentMatches.Single(x => x.Id == matchId);
            if (team == 1)
            {
                match.ScoreTeam1--;
                dM.ShowText(match.ScoreTeam1.ToString(),
                    AreaName.Team1Goals);
            }
            else if (team == 2)
            {
                match.ScoreTeam2--;
                dM.ShowText(match.ScoreTeam2.ToString(),
                    AreaName.Team2Goals);
            }

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.Single(x => x.Id == match.Id);
                dbMatch.ScoreTeam1 = match.ScoreTeam1;
                dbMatch.ScoreTeam2 = match.ScoreTeam2;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
