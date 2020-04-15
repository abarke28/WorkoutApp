using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutApp.Model
{
    public static class MongoHelper
    {
        public const string CONNECTION_STRING = @"mongodb+srv://admin:admin@workoutcluster-8zs1u.azure.mongodb.net/test?retryWrites=true&w=majority";
        public const string DATABASE_NAME = @"workoutsDB";
        public const string WORKOUTS_COLLECTION = @"workouts";
        public const string EXERCISES_COLLECTION = @"exercises";

        public static async void AddWorkoutAsync(Workout workout)
        {
            // Summary
            //
            // Add workout to DB if not present & non-null

            if (workout == null) return;

            var client = new MongoClient(CONNECTION_STRING);
            var database = client.GetDatabase(DATABASE_NAME);
            var collection = database.GetCollection<Workout>(WORKOUTS_COLLECTION);

            await collection.InsertOneAsync(workout);
        }
        public static IEnumerable<Workout> GetWorkoutsAsync()
        {
            // Summary
            //
            // Get all workouts

            var client = new MongoClient(CONNECTION_STRING);
            var database = client.GetDatabase(DATABASE_NAME);
            var collection = database.GetCollection<Workout>(WORKOUTS_COLLECTION);

            return collection.FindSync(w => true).ToEnumerable();
        }
        public static void DeleteWorkout(Workout workout)
        {
            // Summary
            //
            // Delete supplied workout

            var client = new MongoClient(CONNECTION_STRING);
            var database = client.GetDatabase(DATABASE_NAME);
            var collection = database.GetCollection<Workout>(WORKOUTS_COLLECTION);

            var deletionFilter = Builders<Workout>.Filter.Eq("Stations", workout.Stations);

            collection.DeleteOneAsync(deletionFilter);
        }
        public static void UpdateWorkout(Workout workout)
        {
            // Summary
            //
            // Update supplied workout

            var client = new MongoClient(CONNECTION_STRING);
            var database = client.GetDatabase(DATABASE_NAME);
            var collection = database.GetCollection<Workout>(WORKOUTS_COLLECTION);

            var updateFilter = Builders<Workout>.Filter.Eq("Stations", workout.Stations);
            var updateName = Builders<Workout>.Update.Set("Name", workout.Name);
            var updateDescription = Builders<Workout>.Update.Set("Description", workout.Description);

            collection.UpdateOneAsync(updateFilter, updateName);
            collection.UpdateOneAsync(updateFilter, updateDescription);
        }

        public static async void AddExerciseAsync(Exercise exercise)
        {
            // Summary
            //
            // Inserts new exercise

            var client = new MongoClient(CONNECTION_STRING);
            var database = client.GetDatabase(DATABASE_NAME);
            var collection = database.GetCollection<Exercise>(EXERCISES_COLLECTION);

            await collection.InsertOneAsync(exercise);
        }
        public static IEnumerable<Exercise> GetExercisesAsync()
        {
            // Summary
            //
            // Get all exercises

            var client = new MongoClient(CONNECTION_STRING);
            var database = client.GetDatabase(DATABASE_NAME);
            var collection = database.GetCollection<Exercise>(EXERCISES_COLLECTION);

            return collection.FindSync(e => true).ToEnumerable();
        }
    }
}