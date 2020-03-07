using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameDisplayFrontend.Data.JsonModel
{
    public class NewTournamentData
    {
        [Required]
        [MaxLength(256, ErrorMessage = "Der Name kann höchstens aus 256 Zeichen bestehen.")]
        [MinLength(2, ErrorMessage = "Der Name muss mindestens aus zwei Zeichen bestehen.")]
        [RegularExpression(@"^[a-zA-Z\s-]*$", ErrorMessage = "Der Name kann nur aus Buchstaben, Bindestrichen und Leerzeichen bestehen")]
        public string Name { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Die Stadt muss höchstens aus 256 Zeichen bestehen.")]
        [MinLength(2, ErrorMessage = "Die Stadt muss mindestens aus zwei Zeichen bestehen.")]
        public string City { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Der Ort muss höchstens aus 256 Zeichen bestehen.")]
        [MinLength(2, ErrorMessage = "Der Ort muss mindestens aus zwei Zeichen bestehen.")]
        public string Place { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "01.01.2020", "01.01.2030", ErrorMessage = "Bitte ein plausibles Datum angeben")]
        public DateTime Date { get; set; }
    }
}
