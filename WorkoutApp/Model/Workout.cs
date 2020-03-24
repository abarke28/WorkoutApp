using System.Collections.Generic;

namespace WorkoutApp.Model
{
    public class Workout
    {
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set; }
        public List<Station> Stations { get; set; }

        public Workout(int numStations = 4)
        {
            Stations = new List<Station>();

            for (int i = 0; i < numStations; i++)
            {
                Stations.Add(new Station());
            }
        }
    }
}