using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameDisplayFrontend.Data.JsonModel
{
    public class NewMatchData
    {
        [Required]
        public int Team1Id { get; set; }

        [Required]
        public int Team2Id { get; set; }

        [Range(typeof(int), "1", "1000", ErrorMessage = "Bitte eine Anzahl an regulären Halbzeiten angeben")]
        public int HalfTimeAmount { get; set; } = 2;

        [Range(typeof(int), "1", "2147483647", ErrorMessage = "Bitte eine Länge jeder Halbzeit angeben")]
        public int HalfTimeSeconds { get; set; } = 900;
        public List<int> RefereeIds { get; set; } = new List<int>() {0,0,0,0,0};
        

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "01.01.1930 00:00", "01.01.2030 00:00", ErrorMessage = "Bitte eine plausible Uhrzeit der Startzeit angeben")]
        public DateTime StartPlaned { get; set; } = DateTime.Now;

        public int TournamentId { get; set; }
    }
}
