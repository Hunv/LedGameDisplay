using System;
using System.Collections.Generic;
using System.Text;

namespace LedGameDisplayLibrary
{
    public class DisplayManager
    {
        /// <summary>
        /// Show a text on the Display
        /// </summary>
        /// <param name="text"></param>
        /// <param name="area"></param>
        public void ShowText(string text, AreaName area)
        {
            if (Display.LayoutConfig == null)
            {
                Console.WriteLine("Display not initialized. Call Initialize function first.");
            }

            Display.ShowString(text, area.ToString().ToLower());
            Display.Render();
        }

        /// <summary>
        /// Initializes the Display Manager
        /// </summary>
        /// <param name="layoutName"></param>
        public void Initialize(string layoutName)
        {
            Display.Initialize(layoutName);
        }

        /// <summary>
        /// Is the DisplayManager initalized?
        /// </summary>
        public bool IsInitialized { get { return Display.LayoutConfig == null ? false : true; } }
    }
}
