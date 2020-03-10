using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LedGameDisplayApi.DataModel
{
    public class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<DbMatch2Player> DbMatch2Player { get; set; }
        public DbSet<DbMatch2PlayerReferee> DbMatch2PlayerReferee { get; set; }
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
            // Map table names
            modelBuilder.Entity<DbMatch2Player>().ToTable("DbMatch2Player", DbSettings.dbSchema);
            modelBuilder.Entity<DbMatch2Player>(entity =>
            {
                entity.HasKey(mp => new { mp.PlayerId, mp.MatchId });
                entity.HasOne(mp => mp.Player)
                    .WithMany(m => m.MatchParticipations)
                    .HasForeignKey(mp => mp.MatchId);
                entity.HasOne(mp => mp.Match)
                    .WithMany(p => p.Players)
                    .HasForeignKey(mp => mp.PlayerId);
            });

            modelBuilder.Entity<DbMatch2PlayerReferee>().ToTable("DbMatch2PlayerReferee", DbSettings.dbSchema);
            modelBuilder.Entity<DbMatch2PlayerReferee>(entity =>
            {
                entity.HasKey(mp => new { mp.RefereeId, mp.MatchId });
                entity.HasOne(mp => mp.Referee)
                    .WithMany(m => m.MatchReferee)
                    .HasForeignKey(mp => mp.MatchId);
                entity.HasOne(mp => mp.Match)
                    .WithMany(p => p.Referees)
                    .HasForeignKey(mp => mp.RefereeId);
            });

            modelBuilder.Entity<Player>().ToTable("Players", DbSettings.dbSchema);
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.Id);
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
                entity.HasMany(e => e.Penalties).WithOne(e => e.Match);
                entity.HasOne(e => e.Team1).WithMany(e => e.MatchesTeam1);
                entity.HasOne(e => e.Team2).WithMany(e => e.MatchesTeam2);
            });

            modelBuilder.Entity<Tournament>().ToTable("Tournaments", DbSettings.dbSchema);
            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Matches).WithOne(e => e.Tournament);
            });

            modelBuilder.Entity<DisplayCommand>().ToTable("DisplayCommands", DbSettings.dbSchema);
            modelBuilder.Entity<DisplayCommand>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            base.OnModelCreating(modelBuilder);
        } 
    }
}