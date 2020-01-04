using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using rpi_ws281x;

namespace LedGameDisplayLibrary.Effects
{
    public class TestChar : IEffect
    {
        public override void Execute()
        {
            foreach (var aArea in Display.LayoutConfig.AreaList)
            {
                foreach (var aCharSet in Display.CharacterSets)
                {
                    foreach(var aChar in aCharSet.Characters)
                    { 
                        Display.ShowChar(aChar, aArea.Name);
                        Display.Render();
                        System.Threading.Thread.Sleep(200);
                    }
                }
            }
        }

        
    }
}
