using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LedGameDisplayApi.DataModel
{
    public class MyDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Penalty> Penalties { get; set; }

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
            // Map table names
            modelBuilder.Entity<Player>().ToTable("Players", DbSettings.dbSchema);
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.Id);
                //entity.HasOne(e => e.Team).WithMany(e => e.Players);
            });

            modelBuilder.Entity<Team>().ToTable("Teams", DbSettings.dbSchema);
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Players).WithOne(e => e.Team);
            });

            modelBuilder.Entity<Penalty>().ToTable("Penalties", DbSettings.dbSchema);
            modelBuilder.Entity<Penalty>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Match>().ToTable("Matches", DbSettings.dbSchema);
            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Team1Players);
                entity.HasMany(e => e.Team2Players);
                entity.HasMany(e => e.Referees);
                entity.HasMany(e => e.Penalties).WithOne(e => e.Match);
            });

            modelBuilder.Entity<Tournament>().ToTable("Tournaments", DbSettings.dbSchema);
            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Matches).WithOne(e => e.Tournament);
            });

            base.OnModelCreating(modelBuilder);
        } 
    }
}