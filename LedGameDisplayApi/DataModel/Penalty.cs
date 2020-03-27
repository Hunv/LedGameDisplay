using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameDisplayApi.DataModel
{
    public class Penalty
    {
        /// <summary>
        /// Id of the Penalty
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The Player, that caused the penalty. May be null in case it is a team penalty
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The Team, that caused the penalty. may be null in case it is a player penalty
        /// </summary>
        public Team Team { get; set; }

        /// <summary>
        /// Is it a team penalty?
        /// </summary>
        public bool IsTeamPenalty { get; set; }

        /// <summary>
        /// The time left in seconds until the penalty is over
        /// </summary>
        public int TimeLeft { get; set; }

        /// <summary>
        /// Is the penalty a total dismiss?
        /// </summary>
        public bool IsDismiss { get; set; }

        /// <summary>
        /// The match this penalty was dones
        /// </summary>
        public Match Match { get; set; }

        /// <summary>
        /// The time played when this penalty was done
        /// </summary>
        public TimeSpan TimePlayed { get; set; }

        /// <summary>
        /// The halftime played, when tis penalty was done
        /// </summary>
        public int HalftimePlayed { get; set; }
    }
}
