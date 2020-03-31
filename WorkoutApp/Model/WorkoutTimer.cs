using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WorkoutApp.Model
{
    public class WorkoutTimer : INotifyPropertyChanged
    {
        private Workout _workout;
        public Workout Workout
        {
            get { return _workout; }
            set
            {
                if (_workout == value) return;
                _workout = value;
                OnPropertyChanged("Workout");
            }
        }

        private List<TimerStation> _timeStack;
        public List<TimerStation> TimeStack
        {
            get { return _timeStack; }
            set
            {
                if (_timeStack == value) return;
                _timeStack = value;
                OnPropertyChanged("TimeStack");
            }
        }        
        public WorkoutTimer(Workout workout)
        {
            // Summary
            //
            // Constructor takes a workout and builds a stack of timer stations with each station
            // repersenting an exercise or rest

            TimeStack = new List<TimerStation>();
            int numStations = workout.Stations.Count;
            int exercisesPerStation = workout.Stations[0].Exercises.Count;

            for (int i=0; i < numStations; i++)
            {
                for (int j=0; j < exercisesPerStation; j++)
                {
                    // Add Exercise
                    TimeStack.Add(new TimerStation
                    {
                        ExerciseName = workout.Stations[i].Exercises[j].ExerciseName,
                        Description = workout.Stations[i].Exercises[j].Description,
                        ExerciseNumber = (j+1).ToString(),
                        StationNumber = (i+1).ToString(),
                        Time = new TimeSpan(0,0,workout.RepSeconds)
                    });

                    // Add Rep Rest unless about to add a Station Rest

                    // Need to add 2 to avoid off-by-1 error since indexes start at 0 & have to stop 1 early
                    if ((j + 2) == exercisesPerStation) break;

                    TimeStack.Add(new TimerStation
                    {
                        ExerciseName = "Rest",
                        Description = String.Empty,
                        ExerciseNumber = (j+1).ToString(),
                        StationNumber = (i+1).ToString(),
                        Time = new TimeSpan(0,0,workout.RestSeconds)
                    });
                }

                // Add Station Rest unless workout is over

                if ((i + 2) == numStations) break;

                TimeStack.Add(new TimerStation 
                {
                    ExerciseName = "Rest",
                    Description = String.Empty,
                    ExerciseNumber = "--",
                    StationNumber = "--",
                    Time = new TimeSpan(0,0,workout.SetSeconds)
                });
            }
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
