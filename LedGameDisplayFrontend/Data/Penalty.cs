using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameDisplayFrontend.Data
{
    public class Penalty
    {
        [Key]
        public int Id { get; set; }
        public Player Player { get; set; }
        public Team Team { get; set; }
        public bool IsTeamPenalty { get; set; }
        public int TimeLeft { get; set; }
        public bool IsDismiss { get; set; }
        public Match Match { get; set; }
    }
}
