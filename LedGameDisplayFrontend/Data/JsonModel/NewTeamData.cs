using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameDisplayFrontend.Data.JsonModel
{
    public class NewTeamData
    {
        [Required]
        [MaxLength(128, ErrorMessage = "Der Nachname muss höchstens aus 128 Zeichen bestehen.")]
        [MinLength(2, ErrorMessage = "Der Nachname muss mindestens aus zwei Zeichen bestehen.")]
        public string Name { get; set; } = "";

        [Required]
        [MaxLength(8, ErrorMessage = "Die Abkürzung darf aus höchstens 8 Zeichen bestehen")]
        [MinLength(2, ErrorMessage = "Die Abkürzung muss mindestens aus zwei Zeichen bestehen.")]
        public string Shortname { get; set; } = "";

        [Required]
        [MaxLength(256, ErrorMessage = "Der Vereinsname muss höchstens aus 256 Zeichen bestehen.")]
        [MinLength(2, ErrorMessage = "Der Vereinsname muss mindestens aus zwei Zeichen bestehen.")]

        public string Clubname { get; set; } = "";
    }
}
