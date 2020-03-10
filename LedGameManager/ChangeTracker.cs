using LedGameManager.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameManager
{
    public class ChangeTracker
    {
        private readonly Timer tmrChangeTracker = new Timer(100);
        private bool working = false;
        private LedGameDisplayLibrary.DisplayManager Dm = new LedGameDisplayLibrary.DisplayManager();

        public ChangeTracker()
        {
            Dm.Initialize("96x60");
            tmrChangeTracker.Elapsed += TmrChangeTracker_Elapsed;
            tmrChangeTracker.Start();

            Console.WriteLine("Change Tracker initialized");
        }

        private async void TmrChangeTracker_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (working)
                return;

            working = true;

            using (var dbContext = new DatabaseContext())
            {
                var changeList = dbContext.DisplayCommands.ToList();

                if (changeList.Count() > 0)
                {
                    Console.WriteLine("Working on {0} Display Commands", changeList.Count());
                    foreach(var aChange in changeList)
                    {
                        await Execute(aChange);
                    }

                    for (int i = 0; i < changeList.Count(); i++)
                        dbContext.DisplayCommands.Remove(changeList[i]);

                    await dbContext.SaveChangesAsync();
                }
            }

            working = false;
        }

        private async Task Execute(DisplayCommand command)
        {
            if (command.Expires < DateTime.Now)
            {
                Console.WriteLine("Command {0} for Area {1} expired at {2}.", command.Command, command.Area, command.Expires);
                return;
            }

            switch(command.Command)
            {
                case "showtext": //Show the text given by value and color
                    Console.WriteLine("Show text {0} in area {1}", command.Value, command.Area);
                    Dm.ShowText(command.Value, (LedGameDisplayLibrary.AreaName)Enum.Parse(typeof(LedGameDisplayLibrary.AreaName), command.Area, true));
                    break;
                case "clear": //Clears the whole display to black
                    Console.WriteLine("Clear Display");
                    Dm.Clear();
                    break;
            }
        }
    }
}
