using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutApp.Model
{
    public class ExerciseContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer(DatabaseHelper.CONNECTION_STRING);
        }
    }
}
