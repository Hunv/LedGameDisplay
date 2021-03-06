﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LedGameDisplayLibrary
{
    public class Area
    {
        public string Name { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Align { get; set; } = "left";
        public string Color { get; set; } = "FFFFFF";
    }


    public enum AreaName
    {
        Team1Name,
        Team2Name,
        Team1Goals,
        Team2Goals,
        GoalDivider,
        Team1Penalties,
        Team2Penalties,
        Text,
        Time

    }
}
