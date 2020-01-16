using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LedGameDisplayFrontend.Data
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Es muss ein Vorname angegeben werden")]
        [MaxLength(256, ErrorMessage = "Der Vorname muss höchstens aus 256 Zeichen bestehen.")]
        [MinLength(2, ErrorMessage = "Der Vorname muss mindestens aus zwei Zeichen bestehen.")]
        [RegularExpression(@"^[a-zA-Z\s-]*$", ErrorMessage ="Ein Name kann nur aus Buchstaben, Bindestrichen und Leerzeichen bestehen")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Es muss ein Nachname angegeben werden")]
        [MaxLength(256, ErrorMessage = "Der Nachname muss höchstens aus 256 Zeichen bestehen.")]
        [MinLength(2, ErrorMessage = "Der Nachname muss mindestens aus zwei Zeichen bestehen.")]
        [RegularExpression(@"^[a-zA-Z\s-]*$", ErrorMessage = "Ein Name kann nur aus Buchstaben, Bindestrichen und Leerzeichen bestehen")]
        public string Lastname { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Range(typeof(DateTime), "01.01.1930", "01.01.2030", ErrorMessage = "Bitte ein plausibles Geburtsdatum angeben")]
        public DateTime Birthday { get; set; } = new DateTime(1990, 01, 01);

        [MaxLength(1)]
        public string Sex { get; set; }

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
        
        [Required]
        [Range(typeof(Int32), "1", "2147483647", ErrorMessage = "Bitte einen Verein auswählen")]
        public int TeamId { get; set; }

        [JsonIgnore]
        public Team Team { get; set; }
    }
}
