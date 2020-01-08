using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameDisplayFrontend.Data
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
    }
}
