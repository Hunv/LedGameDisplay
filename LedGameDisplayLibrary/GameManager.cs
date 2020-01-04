using System;
using System.Collections.Generic;
using System.Text;

namespace LedGameDisplayLibrary
{
    public static class GameManager
    {
        public static List<Team> Teams { get; set; } = new List<Team>();

        /// <summary>
        /// The the game currently running or paused (i.e. because of Timeouts)
        /// </summary>
        public static bool GameRunning = false;

        /// <summary>
        /// Is the game over?
        /// </summary>
        public static bool GameFinished = false;

        /// <summary>
        /// Is there currently a half time (or any other kind of break between parts of the game)?
        /// </summary>
        public static bool HalfTime = false;
    }
}
