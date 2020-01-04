﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        public ICollection<Player> Team1Players { get; set; }
        public ICollection<Player> Team2Players { get; set; }
        public int HalfTime { get; set; }
        public TimeSpan TimeLeft { get; set; }
        public ICollection<Player> Referees { get; set; }
        public ICollection<Penalty> Penalties { get; set; }
        public DateTime StartPlaned { get; set; }
        public DateTime StartActual { get; set; }
        
        public Tournament Tournament { get; set; }
    }
}
