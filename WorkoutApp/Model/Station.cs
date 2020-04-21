﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutApp.Model
{
    public class Station
    {
        public string StationName { get; set; }
        public IList<Exercise> Exercises { get; set; }

        public Station()
        {
            Exercises = new ObservableCollection<Exercise>();
        }
    }
}