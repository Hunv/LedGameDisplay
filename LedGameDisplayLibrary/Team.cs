using System;
using System.Collections.Generic;
using System.Text;

namespace LedGameDisplayLibrary
{
    public class Team
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public string ScoreArea { get; set; }
        public string PenaltyArea { get; set; }
        public List<TimeSpan> Penalties { get; set; }
    }
}
