using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameDisplayApi.DataModel.JsonModel
{
    public class NewPlayerData
    {
        [Required(ErrorMessage = "Es muss ein Vorname angegeben werden")]
        [MaxLength(256, ErrorMessage = "Der Vorname muss höchstens aus 256 Zeichen bestehen.")]
        [MinLength(2, ErrorMessage = "Der Vorname muss mindestens aus zwei Zeichen bestehen.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Es muss ein Nachname angegeben werden")]
        [MaxLength(256, ErrorMessage = "Der Nachname muss höchstens aus 256 Zeichen bestehen.")]
        [MinLength(2, ErrorMessage = "Der Nachname muss mindestens aus zwei Zeichen bestehen.")]
        public string Lastname { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "01.01.1930", "01.01.2030", ErrorMessage = "Bitte ein plausibles Geburtsdatum angeben")]
        public DateTime Birthday { get; set; } = new DateTime(1990, 01, 01);

        [MaxLength(1)]
        [RegularExpression(@"^[mwd]$", ErrorMessage = "Das Geschlecht muss m, w oder d sein.")]
        public string Sex { get; set; } = "m";

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "01.01.1930", "01.01.2030", ErrorMessage = "Bitte ein plausibles Ablaufdatum des ärztlichen Attests angeben")]
        public DateTime HealthCertificationExpireDate { get; set; } = DateTime.Now;

        [MaxLength(128)]
        public string RefereeLevel { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "01.01.1930", "01.01.2030", ErrorMessage = "Bitte ein plausibles Ablaufdatum der Schiedsrichterlizenz angeben")]
        public DateTime RefereeLevelExpireDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;
        public bool IsCaptain { get; set; }
        public bool IsViceCaptain { get; set; }

        public int TeamId { get; set; } = 0;
    }
}

