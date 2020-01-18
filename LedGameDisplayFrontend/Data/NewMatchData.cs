using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameDisplayFrontend.Data
{
    public class NewMatchData
    {
        [Required]
        public int Team1Id { get; set; }

        [Required]
        public int Team2Id { get; set; }

        public int MatchtimeSeconds { get; set; } = 900;
        public List<int> RefereeIds { get; set; } = new List<int>() {0,0,0,0,0};
        

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "01.01.1930 00:00", "01.01.2030 00:00", ErrorMessage = "Bitte eine plausible Uhrzeit der Startzeit angeben")]
        public DateTime StartPlaned { get; set; } = DateTime.Now;

        public int TournamentId { get; set; }
    }
}
