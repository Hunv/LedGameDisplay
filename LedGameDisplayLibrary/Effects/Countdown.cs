﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Linq;
using rpi_ws281x;

namespace LedGameDisplayLibrary.Effects
{
    public class Countdown : IEffect
    {
        /// <summary>
        /// The Minutes to countdown
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// The Seconds to count down
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Show only seconds (i.e. 78 instead of 1:18)
        /// </summary>
        public bool SecondsOnly { get; set; }

        /// <summary>
        /// The Area where the Countdown should be shown
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// The Charset to use
        /// </summary>
        public CharacterSet CharSet { get; set; }

        private Timer tmrCountdown = new Timer(1000);

        public override void Execute()
        {
            tmrCountdown.Elapsed += TmrCountdown_Elapsed;
            tmrCountdown.Start();
        }

        private void TmrCountdown_Elapsed(object sender, ElapsedEventArgs e)
        {
            string time = Minutes.ToString() + ":" + Seconds.ToString("00");

            Display.ShowString(time, "time");
            Display.Render();

            Seconds -= 1;
            if (Seconds < 0)
            {
                Minutes -= 1;
                Seconds = 59;
            }
            if (Minutes < 0)
            {
                tmrCountdown.Stop();
            }
        }
    }
}
