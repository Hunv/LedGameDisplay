using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LedGameDisplayApi.DataModel
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Team Team1 { get; set; }

        [Required]
        public Team Team2 { get; set; }

        public int ScoreTeam1 { get; set; }
        public int ScoreTeam2 { get; set; }
        public ICollection<DbMatch2Player> Players { get; set; }       
        public int CurrentHalfTime { get; set; }

        [NotMapped]
        public int CurrentTimeLeftSeconds { get { return (int)CurrentTimeLeft.TotalSeconds; } set { CurrentTimeLeft = new TimeSpan(0, 0, value); } }

        [JsonIgnore]
        public TimeSpan CurrentTimeLeft { get; set; }
        public int HalfTimeAmount { get; set; }

        [NotMapped]
        public int HalfTimeTimeSeconds { get { return (int)HalfTimeTime.TotalSeconds; } set { HalfTimeTime = new TimeSpan(0, 0, value); } }

        [JsonIgnore]
        public TimeSpan HalfTimeTime { get; set; }
        public ICollection<DbMatch2PlayerReferee> Referees { get; set; }
        public ICollection<Penalty> Penalties { get; set; }
        public DateTime StartPlaned { get; set; }
        public DateTime StartActual { get; set; }
        public DateTime EndActual { get; set; }
        public bool IsLive { get; set; }
        
        public Tournament Tournament { get; set; }

    }
}
