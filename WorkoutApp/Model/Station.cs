using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Model
{
    public class Station
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public IList<ExerciseStation> ExerciseStations { get; set; }
        public Station()
        {
            // Exercises = new List<Exercise>();
        }
    }
}
