using System.Collections.Generic;

namespace WorkoutApp.Model
{
    public class Workout
    {
        public int? WorkoutId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RepSeconds { get; set; }
        public int RestSeconds { get; set; }
        public List<Station> Stations { get; set; }
        public Workout()
        {
            Stations = new List<Station>();
        }
    }
}