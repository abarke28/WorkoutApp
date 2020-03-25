using System.Collections.Generic;

namespace WorkoutApp.Model
{
    public class Workout
    {
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set; }
        public List<Station> Stations { get; set; }

        public Workout()
        {
            Stations = new List<Station>();
        }
    }
}