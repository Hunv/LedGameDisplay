﻿using rpi_ws281x;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LedGameDisplayLibrary.Effects
{
    public class TestColorWipe : IEffect
    {
        public override void Execute()
        {
            
            for (int i = 0; i < 255; i++)
            {
                var color = i % 3;
                if (color == 0)
                    Display.SetAll(Color.FromArgb(i,0,0));
                else if (color == 1)
                    Display.SetAll(Color.FromArgb(0, i, 0));
                else if (color == 2)
                    Display.SetAll(Color.FromArgb(0, 0, i));

                Display.Render();
            }

            Display.SetAll(Color.FromArgb(0, 0, 0));
            Display.Render();
            
        }
    }
}
