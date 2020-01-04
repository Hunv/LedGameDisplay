using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameDisplayApi.DataModel
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string City { get; set; }

        [Required]
        [MaxLength(256)]
        public string Place { get; set; }

        public DateTime Date { get; set; }

        public ICollection<Match> Matches { get; set; }
    }
}
