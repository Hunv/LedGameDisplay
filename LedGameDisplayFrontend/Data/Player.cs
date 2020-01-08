using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameDisplayFrontend.Data
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
                
        [MaxLength(256)]
        public string Clubname { get; set; }

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
    }
}
