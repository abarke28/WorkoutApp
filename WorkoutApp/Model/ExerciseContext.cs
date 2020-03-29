using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutApp.Model
{
    public class ExerciseContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<ExerciseStation> ExerciseStations { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer(DatabaseHelper.CONNECTION_STRING);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExerciseStation>().HasKey(es => new { es.ExerciseId, es.StationId });
        }
    }
}