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
        public static void AddExercise(Exercise exercise)
        {
            // Summary
            //
            // Adds supplied exercise to database

            if (exercise == null) throw new ArgumentNullException();

            ExerciseContext exerciseContext = new ExerciseContext();

            exerciseContext.Exercises.Add(exercise);
        }
    }
}