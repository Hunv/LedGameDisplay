using rpi_ws281x;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LedGameDisplayLibrary.Effects
{
    public class IdleBar : IEffect
    {
        public override void Execute()
        {
            while (1 == 1)
            {
                for (var currentPosition = 0; currentPosition <= Display.X + 2; currentPosition++)
                {
                    for (int line = 0; line < Display.Y; line++)
                    {       
                        if (currentPosition - 3 >= 0 && currentPosition - 3 < Display.X - 1)
                            Display.SetLed(Display.GetLedNumber(currentPosition - 3, line), Color.FromArgb(0, 0, 0));
                            
                        if (currentPosition - 2 >= 0 && currentPosition - 2 < Display.X - 1)
                            Display.SetLed(Display.GetLedNumber(currentPosition - 2, line), Color.FromArgb(50, 50, 50));
                            
                        if (currentPosition - 1 >= 0 && currentPosition - 1 < Display.X - 1)
                            Display.SetLed(Display.GetLedNumber(currentPosition - 1, line), Color.FromArgb(100, 100, 100));
                            
                        if (currentPosition >= 0 && currentPosition < Display.X - 1)
                            Display.SetLed(Display.GetLedNumber(currentPosition, line), Color.FromArgb(255, 255, 255));
                            
                        if (currentPosition + 1 >= 0 && currentPosition + 1 < Display.X - 1)
                            Display.SetLed(Display.GetLedNumber(currentPosition +1, line), Color.FromArgb(100, 100, 100));
                            
                        if (currentPosition + 2 >= 0 && currentPosition + 2 < Display.X - 1)
                            Display.SetLed(Display.GetLedNumber(currentPosition +2, line), Color.FromArgb(50, 50, 50));
                            
                    }

                    Display.Render();

                    if (currentPosition == Display.X + 3)
                        currentPosition = 0;
                }
            }
        }
    }
}
