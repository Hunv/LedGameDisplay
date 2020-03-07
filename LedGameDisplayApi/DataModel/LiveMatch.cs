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
    public static class LiveMatch
    {
        private static Timer _TmrTimeLeft = new Timer(1000);
        private static LedGameDisplayLibrary.DisplayManager _DisplayManager = new LedGameDisplayLibrary.DisplayManager();
        public static Match CurrentMatch { get; set; }

        private async static void _TmrTimeLeft_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (CurrentMatch.CurrentTimeLeft.TotalSeconds == 0)
            {
                _TmrTimeLeft.Stop();
            }
            else
            {
                CurrentMatch.CurrentTimeLeft = CurrentMatch.CurrentTimeLeft.Subtract(new TimeSpan(0, 0, 1));

                _DisplayManager.ShowText(CurrentMatch.CurrentTimeLeft.Minutes.ToString("D2") + ":" + CurrentMatch.CurrentTimeLeft.Seconds.ToString("D2"), LedGameDisplayLibrary.AreaName.Time);

                using (var dbContext = new DatabaseContext())
                {
                    var dbMatch = dbContext.Matches.Single(x => x.Id == CurrentMatch.Id);
                    dbMatch.CurrentTimeLeft = CurrentMatch.CurrentTimeLeft;
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public static void Initialize()
        {
            _TmrTimeLeft.Elapsed += _TmrTimeLeft_Elapsed;

            if (!_DisplayManager.IsInitialized)
                _DisplayManager.Initialize("96x60");
        }

        public async static void StartHalftime()
        {
            CurrentMatch.CurrentTimeLeft = CurrentMatch.HalfTimeTime;
            CurrentMatch.CurrentHalfTime++;
            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.Single(x => x.Id == CurrentMatch.Id);
                dbMatch.CurrentTimeLeft = CurrentMatch.CurrentTimeLeft;
                dbMatch.CurrentHalfTime = CurrentMatch.CurrentHalfTime;
                await dbContext.SaveChangesAsync();
            }

            _TmrTimeLeft.Start();
        }

        public static void PauseMatch()
        {
            _TmrTimeLeft.Stop();
        }
        public static void UnpauseMatch()
        {
            _TmrTimeLeft.Start();
        }

        public static void CancelMatch()
        {
            _TmrTimeLeft.Stop();
        }

        public async static void FinishMatch()
        {
            _TmrTimeLeft.Stop();
            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.SingleOrDefault(x => x.IsLive);

                if (dbMatch != null)
                {
                    dbMatch.IsLive = false;
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public async static void Goal(int team)
        {
            if (team == 1)
                CurrentMatch.ScoreTeam1++;
            else if (team == 2)
                CurrentMatch.ScoreTeam2++;

            using (var dbContext = new DatabaseContext())
            {
                var dbMatch = dbContext.Matches.Single(x => x.Id == CurrentMatch.Id);
                dbMatch.ScoreTeam1 = CurrentMatch.ScoreTeam1;
                dbMatch.ScoreTeam2 = CurrentMatch.ScoreTeam2;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
