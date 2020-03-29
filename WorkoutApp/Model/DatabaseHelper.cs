using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkoutApp.Model
{
    public static class DatabaseHelper
    {
        public const string CONNECTION_STRING = @"Data Source=DELLLAPTOP\ABSQL01;Initial Catalog=WorkoutAppDB;Integrated Security=True";
        public static Exercise GetExercise(int Id)
        {
            // Summary
            //
            // Return individual Exercise based on Id

            return new ExerciseContext().Exercises.Find(Id);
        }
        public static IEnumerable<Exercise> GetExercises()
        {
            // Summary
            //
            // Returns all Exercises

            return new ExerciseContext().Exercises.OrderBy(e => e.ExerciseName).ToList();
        }
        public static IEnumerable<Exercise> GetExercises(Func<Exercise,bool> predicate)
        {
            // Summary
            //
            // Return Exercises as filtered by a Predicate

            return new ExerciseContext().Exercises.Where(predicate).OrderBy(e => e.ExerciseName).ToList();
        }
        public static IEnumerable<ExerciseStation> GetExerciseStations()
        {
            // Summary
            //
            // Returns all ExerciseStations

            return new ExerciseContext().ExerciseStations.ToList();
        }
        public static IEnumerable<ExerciseStation> GetExerciseStations(Func<ExerciseStation, bool> predicate)
        {
            // Summary
            //
            // Return ExerciseStations as filtered by a Predicate

            return new ExerciseContext().ExerciseStations.Where(predicate).OrderBy(e => e.StationId).ToList();
        }
        public static void AddExerciseStation(ExerciseStation exerciseStation)
        {
            // Summary
            //
            // Add ExerciseStation mapping to DB if not already present

            ExerciseContext exerciseContext = new ExerciseContext();

            if (exerciseContext.ExerciseStations.Contains(exerciseStation)) return;

            exerciseContext.Add(exerciseStation);

            exerciseContext.SaveChanges();
        }
        public static Station GetStation(int Id)
        {
            // Summary
            //
            // Return individual Station based on Id

            return new ExerciseContext().Stations.Find(Id);
        }
        public static IEnumerable<Station> GetStations()
        {
            // Summary
            //
            // Returns all Stations

            return new ExerciseContext().Stations.OrderBy(s => s.StationName).ToList();
        }
        public static IEnumerable<Station> GetStations(Func<Station, bool> predicate)
        {
            // Summary
            //
            // Return Stations as filtered by a Predicate

            return new ExerciseContext().Stations.Where(predicate).OrderBy(s => s.StationName).ToList();
        }
        public static void AddStation(Station station)
        {
            // Summary
            //
            // Add Station to DB if not already present

            ExerciseContext exerciseContext = new ExerciseContext();

            if (exerciseContext.Stations.Contains(station)) return;

            exerciseContext.Stations.Add(station);

            exerciseContext.SaveChanges();
        }
        public static Workout GetWorkout(int Id)
        {
            // Summary
            //
            // Returns specified Workout

            return new ExerciseContext().Workouts.Find(Id);
        }
        public static IEnumerable<Workout> GetWorkouts()
        {
            // Summary
            //
            // Returns all Workouts

            return new ExerciseContext().Workouts.OrderBy(w => w.Name).ToList();
        }
        public static IEnumerable<Workout> GetWorkouts(Func<Workout, bool> predicate)
        {
            // Summary
            //
            // Returns all Workouts as filtered by a supplied predicae

            return new ExerciseContext().Workouts.Where(predicate).OrderBy(w => w.Name).ToList();
        }
        public static void AddWorkout(Workout workout)
        {
            // Summary
            //
            // Adds Workout to DB if not already present

            ExerciseContext exerciseContext = new ExerciseContext();

            if (exerciseContext.Workouts.Contains(workout)) return;

            exerciseContext.Workouts.Add(workout);

            exerciseContext.SaveChanges();
        }
    }
}
