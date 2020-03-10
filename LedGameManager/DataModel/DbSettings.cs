using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameManager.DataModel
{
    public static class DbSettings
    {
        /// <summary>
        /// Filename of the Database file
        /// </summary>
        public static readonly string dbFilename = "../GameDatabase.db";

        /// <summary>
        /// Filename of the Database file
        /// </summary>
        public static readonly string dbSchema = "dbo";

        /// <summary>
        /// Trigger the Initialization of the Database, if not exists.
        /// </summary>
        public static void Initialize()
        {
            //If file not exists, just do nothing

            //if (File.Exists(dbFilename))
            //    return;

            //using (var dbContext = new DatabaseContext())
            //{
            //    //Ensure database is created
            //    dbContext.Database.EnsureCreated();
            //}
        }
    }
}
