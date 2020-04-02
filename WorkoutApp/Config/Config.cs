using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutApp.Config
{
    public class Config
    {
        public int NumStations { get; set; } = 4;
        public int NumExercisesPerStation { get; set; } = 3;
        public int NumRounds { get; set; } = 4;
        public int ExerciseLength { get; set; } = 30;
        public int ExerciseRestLength { get; set; } = 10;
        public int StationRestLength { get; set; } = 30;
    }
}