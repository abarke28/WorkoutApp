using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutApp.Model
{
    public class Record
    {
        public DateTime Date { get; set; }
        public Workout Workout { get; set; }

        public Record(Workout workout)
        {
            Date = DateTime.Today;
            Workout = workout;
        }
    }
}
