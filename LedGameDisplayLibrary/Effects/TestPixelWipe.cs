using rpi_ws281x;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LedGameDisplayLibrary.Effects
{
    public class TestPixelWipe:IEffect
    {
        public Color Color { get; set; } = Color.White;

        public override void Execute()
        {
            for (int i = 0; i <= Display.LedCount -1; i++)
            {
                Display.SetLed(i, Color);
                Display.Render();
            }
        }
    }
}
