using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameManager.DataModel
{
    public class DisplayCommand
    {
        [Key]
        public int Id { get; set; }
        public string Command { get; set; }
        public string Value { get; set; }
        public string Area { get; set; }
        public string ColorHex { get; set; } = "FFFFFF";
        public DateTime? Expires { get; set; } = null;

    }
}
