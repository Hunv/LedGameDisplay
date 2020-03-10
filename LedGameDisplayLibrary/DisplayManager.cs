using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Timers;

namespace LedGameDisplayLibrary
{
    public class DisplayManager
    {
        private Queue<Tuple<string, string, DateTime?>> _ChangeQueue = new Queue<Tuple<string, string, DateTime?>>();
        private Timer _TmrWorker = new Timer(500);

        /// <summary>
        /// Is the DisplayManager initalized?
        /// </summary>
        public bool IsInitialized { get { return Display.LayoutConfig == null ? false : true; } }

        /// <summary>
        /// Show a text on the Display
        /// </summary>
        /// <param name="text"></param>
        /// <param name="area"></param>
        public void ShowText(string text, AreaName area, DateTime? experationTime = null)
        {
            if (Display.LayoutConfig == null)
            {
                Console.WriteLine("Display not initialized. Call Initialize function first.");
            }

            _ChangeQueue.Enqueue(new Tuple<string, string, DateTime?>(area.ToString(), text, experationTime));
        }

        public void Clear()
        {
            Display.SetAll(Color.Black);
        }

        /// <summary>
        /// Initializes the Display Manager
        /// </summary>
        /// <param name="layoutName"></param>
        public void Initialize(string layoutName)
        {
            if (Display.LayoutConfig == null)
            {
                Display.Initialize(layoutName);
                _TmrWorker.Elapsed += _TmrWorker_Elapsed;
                _TmrWorker.Start();
            }
        }

        private void _TmrWorker_Elapsed(object sender, ElapsedEventArgs e)
        {
            var change = _ChangeQueue.Dequeue();

            if (change == null)
                return;

            //In case the change is expired, don't handle it and cann the Worker again
            if (change.Item3 != null && change.Item3 < DateTime.Now)
            {
                Console.WriteLine("Text \"{0}\" expired.", change.Item2);
                _TmrWorker_Elapsed(sender, e);
                return;
            }
            
            Display.ShowString(change.Item2, change.Item1.ToLower());
            Display.Render();
        }
    }
}
