using rpi_ws281x;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LedGameDisplayLibrary.Effects
{
    public class TestArea : IEffect
    {
        public override void Execute()
        {
            foreach (var aArea in Display.LayoutConfig.AreaList)
            {
                var rnd = new Random();
                var color = Color.FromArgb(rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255));

                for (int x = aArea.PositionX; x < aArea.PositionX + aArea.Width; x++)
                {
                    for (int y = aArea.PositionY; y < aArea.PositionY + aArea.Height; y++)
                    {
                        Display.SetLed(Display.GetLedNumber(x, y), color);
                    }
                }

                Display.Render();
            }
        }
    }
}
