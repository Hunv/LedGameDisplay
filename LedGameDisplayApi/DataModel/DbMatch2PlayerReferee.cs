using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameDisplayApi.DataModel
{
    public class DbMatch2PlayerReferee
    {
        public int RefereeId { get; set; }
        public Player Referee { get; set; }

        public int MatchId { get; set; }
        public Match Match { get; set; }
    }
}
