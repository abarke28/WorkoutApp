﻿using MongoDB.Bson;
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
        public static void AddWorkoutAsync(Workout workout)
        {
            // Summary
            //
            // Add workout to DB if not present & non-null

            if (workout == null) return;

            var client = new MongoClient(CONNECTION_STRING);
            var database = client.GetDatabase(DATABASE_NAME);
            var collection = database.GetCollection<Workout>(WORKOUTS_COLLECTION);

            collection.InsertOneAsync(workout);
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
        public static void AddExerciseAsync(Exercise exercise)
        {
            // Summary
            //
            // Inserts new exercise

            var client = new MongoClient(CONNECTION_STRING);
            var database = client.GetDatabase(DATABASE_NAME);
            var collection = database.GetCollection<Exercise>(EXERCISES_COLLECTION);

            collection.InsertOneAsync(exercise);
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