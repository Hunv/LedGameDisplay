using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LedGameDisplayApi.DataModel
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
        public static readonly string dbSchema = null; //for SQLite: "dbo"

        /// <summary>
        /// Trigger the Initialization of the Database, if not exists.
        /// </summary>
        public static void Initialize()
        {
            //if (File.Exists(dbFilename))
            //    return;

            using (var dbContext = new DatabaseContext())
            {
                //Ensure database is created
                dbContext.Database.EnsureCreated();
                //if (!dbContext.Teams.Any())
                //{
                //    dbContext.Blogs.AddRange(new Blog[]
                //        {
                //             new Blog{ BlogId=1, Title="Blog 1", SubTitle="Blog 1 subtitle" },
                //             new Blog{ BlogId=2, Title="Blog 2", SubTitle="Blog 2 subtitle" },
                //             new Blog{ BlogId=3, Title="Blog 3", SubTitle="Blog 3 subtitle" }
                //        });
                //    dbContext.SaveChanges();
                //}

                //foreach (var blog in dbContext.Blogs)
                //{
                //    Console.WriteLine($"BlogID={blog.BlogId}\tTitle={blog.Title}\t{blog.SubTitle}\tDateTimeAdd={blog.DateTimeAdd}");
                //}
            }
        }
    }
}
