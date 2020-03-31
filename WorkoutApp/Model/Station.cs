using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutApp.Model
{
    public class Station
    {
        public string StationName { get; set; }
        public List<Exercise> Exercises { get; set; }
        public Station()
        {
            Exercises = new List<Exercise>();
        }
    }
}