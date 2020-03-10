using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LedGameManager.DataModel
{
    public class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<DisplayCommand> DisplayCommands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(string.Format("Filename={0}", DbSettings.dbFilename), options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DisplayCommand>().ToTable("DisplayCommands", DbSettings.dbSchema);
            modelBuilder.Entity<DisplayCommand>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            base.OnModelCreating(modelBuilder);
        } 
    }
}