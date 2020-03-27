using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LedGameDisplayApi.DataModel
{
    public class Match
    {
        /// <summary>
        /// The ID of this match
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Team1 of the current match
        /// </summary>
        [Required]
        public Team Team1 { get; set; }

        /// <summary>
        /// Team2 of the current match
        /// </summary>
        [Required]
        public Team Team2 { get; set; }

        /// <summary>
        /// The current score of Team1
        /// </summary>
        public int ScoreTeam1 { get; set; }

        /// <summary>
        /// The current score of Team2
        /// </summary>
        public int ScoreTeam2 { get; set; }

        /// <summary>
        /// The players that are participating at this match
        /// </summary>
        public ICollection<DbMatch2Player> Players { get; set; }       

        /// <summary>
        /// The current halftime in the ongoing match
        /// </summary>
        public int CurrentHalfTime { get; set; }

        /// <summary>
        /// How many seconds are currently left in this ongoing match
        /// This property is used for JSON parsing and communication. Will not be used for DB actions.
        /// </summary>
        [NotMapped]
        public int CurrentTimeLeftSeconds { get { return (int)CurrentTimeLeft.TotalSeconds; } set { CurrentTimeLeft = new TimeSpan(0, 0, value); } }

        /// <summary>
        /// How much time is currently left in this ongoing match
        /// This property is used for Database actions. Will not be parsed to JSON.
        /// </summary>
        [JsonIgnore]
        public TimeSpan CurrentTimeLeft { get; set; }

        /// <summary>
        /// How many halftimes are going to be played in this match
        /// </summary>
        public int HalfTimeAmount { get; set; }

        /// <summary>
        /// How many Seconds a halftime has.
        /// </summary>
        [NotMapped]        
        public int HalfTimeTimeSeconds { get { return (int)HalfTimeTime.TotalSeconds; } set { HalfTimeTime = new TimeSpan(0, 0, value); } }

        /// <summary>
        /// The time a halftime has.
        /// </summary>
        [JsonIgnore]
        public TimeSpan HalfTimeTime { get; set; }

        /// <summary>
        /// The referees, that are refering the match
        /// </summary>
        public ICollection<DbMatch2PlayerReferee> Referees { get; set; }

        /// <summary>
        /// All details to penalties that occured during the match
        /// </summary>
        public ICollection<Penalty> Penalties { get; set; }

        /// <summary>
        /// The planed start time of the match
        /// </summary>
        public DateTime StartPlaned { get; set; }

        /// <summary>
        /// The actual start time of the match
        /// </summary>
        public DateTime StartActual { get; set; }

        /// <summary>
        /// The end time of the match
        /// </summary>
        public DateTime EndActual { get; set; }

        /// <summary>
        /// Is the current match ongoing?
        /// </summary>
        public bool IsLive { get; set; }

        /// <summary>
        /// The current match status
        /// </summary>
        public string MatchStatus { get; set; }
        
        /// <summary>
        /// The tournament this match belongs to
        /// </summary>
        public Tournament Tournament { get; set; }

        /////////////
        // Matchrules:
        /////////////

        /// <summary>
        /// Automatically stops the time in the last 2 Minutes of a games - except in case of a Goal
        /// </summary>
        public bool StopTimeInLast2Minutes { get; set; } = true;

    }

    public enum MatchStatus
    {
        preparing,
        running,
        halftime,
        paused,
        canceled,
        ended,
        finished
    }
}
