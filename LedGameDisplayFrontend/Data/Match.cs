using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameDisplayFrontend.Data
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
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public int HalfTime { get; set; }
        public TimeSpan TimeLeft { get; set; }
        public ICollection<Player> Referees { get; set; } = new List<Player>() { new Player(), new Player(), new Player(), new Player(), new Player() };
        public ICollection<Penalty> Penalties { get; set; } = new List<Penalty>();


        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "01.01.1930", "01.01.2030", ErrorMessage = "Bitte eine plausible Uhrzeit der Startzeit angeben")]
        public DateTime StartPlaned { get; set; } = DateTime.Now;
        public DateTime StartActual { get; set; } = DateTime.Now;

        public Tournament Tournament { get; set; }
    }
}
