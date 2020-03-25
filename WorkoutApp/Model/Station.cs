using System.Collections.Generic;

namespace WorkoutApp.Model
{
    public class Station
    {
        public int StationId { get; set; }
        public List<Exercise> Exercises { get; set; }
        public Station()
        {
            Exercises = new List<Exercise>();
        }
    }
}
