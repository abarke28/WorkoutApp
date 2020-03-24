using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutApp.Model
{
    public class Exercise
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; }
        public string Description { get; set; }
        public ExerciseType ExerciseType { get; set; }
    }

    public enum ExerciseType
    {
        Cardio, 
        LowerBody, 
        UpperBody, 
        Core
    }
}
