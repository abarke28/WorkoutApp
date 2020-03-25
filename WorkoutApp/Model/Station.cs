using System.Collections.Generic;

namespace WorkoutApp.Model
{
    public class Station
    {
        public int StationId { get; set; }
        public List<Exercise> Exercises { get; set; }
        public Station(int numExercises = 3)
        {
            Exercises = new List<Exercise>();

            for (int i = 0; i < numExercises; i++)
            {
                Exercises.Add(new Exercise());
            }
        }
    }
}
