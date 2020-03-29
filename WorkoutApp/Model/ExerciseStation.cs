using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutApp.Model
{
    public class ExerciseStation
    {
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public int StationId { get; set; }
        public Station Station { get; set; }
    }
}
