using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LedGameDisplayApi.DataModel
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        [MaxLength(8)]
        public string Shortname { get; set; }

        [Required]
        [MaxLength(256)]
        public string Clubname { get; set; }

        public ICollection<Player> Players { get; set; }

        [JsonIgnore]
        public ICollection<Match> MatchesTeam1 { get; set; }

        [JsonIgnore]
        public ICollection<Match> MatchesTeam2 { get; set; }
    }
}
