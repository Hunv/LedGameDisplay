using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LedGameDisplayFrontend.Data.JsonModel
{
    public class UpdateMatchOngoing
    {
        public int Id { get; set; }

        public int ScoreTeam1 { get; set; }
        public int ScoreTeam2 { get; set; }
        public int CurrentHalfTime { get; set; }

        [JsonIgnore]
        public TimeSpan CurrentTimeLeft { get; set; }

        public int CurrentTimeLeftSeconds { get { return (int)CurrentTimeLeft.TotalSeconds; } set { CurrentTimeLeft = new TimeSpan(0, 0, value); } }

        public DateTime StartActual { get; set; }

    }
}
