using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        //[JsonIgnore] //Used for Database
        public ICollection<Match> Matches { get; set; }

        //[NotMapped] //Used for JSON
        //public IEnumerable<int> MatchIds { get { return (Matches != null ? Matches.Select(x => x.Id) : new List<int>()); } }
    }
}
