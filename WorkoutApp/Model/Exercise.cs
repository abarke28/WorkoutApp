using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutApp.Model
{
    [BsonIgnoreExtraElements]
    public class Exercise
    {
        public int? ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public string Description { get; set; }
        public ExerciseType ExerciseType { get; set; }
    }
}