using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LedGameDisplayApi.DataModel
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(256)]
        public string Lastname { get; set; }

        public DateTime Birthday { get; set; }

        [MaxLength(1)]
        public string Sex { get; set; }

        public DateTime HealthCertificationExpireDate { get; set; }

        [MaxLength(128)]
        public string RefereeLevel { get; set; }
        public DateTime RefereeLevelExpireDate { get; set; }

        public bool IsActive { get; set; }
        public bool IsCaptain { get; set; }
        public bool IsViceCaptain { get; set; }

        public Team Team { get; set; }

        public DateTime Created { get; set; }

        [JsonIgnore]
        public ICollection<DbMatch2Player> MatchParticipations { get; set; }
        
        [JsonIgnore]
        public ICollection<DbMatch2PlayerReferee> MatchReferee { get; set; }
    }
}
