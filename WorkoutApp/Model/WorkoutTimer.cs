using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Threading;

namespace WorkoutApp.Model
{
    public class WorkoutTimer : INotifyPropertyChanged
    {
        private int? _timerText;
        public int? TimerText
        {
            get { return _timerText; }
            set
            {
                if (_timerText == value) return;
                _timerText = value;
                OnPropertyChanged("TimerText");
            }
        }

        private string _stationNumText;
        public string StationNumText
        {
            get { return _stationNumText; }
            set
            {
                if (_stationNumText == value) return;
                _stationNumText = value;
                OnPropertyChanged("StationNumText");
            }
        }

        private string _exerciseNumText;
        public string ExerciseNumText
        {
            get { return _exerciseNumText; }
            set
            {
                if (_exerciseNumText == value) return;
                _exerciseNumText = value;
                OnPropertyChanged("ExerciseNumText");
            }
        }

        private string _exerciseNameText;
        public string ExerciseNameText
        {
            get { return _exerciseNameText; }
            set
            {
                if (_exerciseNameText == value) return;
                _exerciseNameText = value;
                OnPropertyChanged("ExerciseNameText");
            }
        }

        private string _exerciseDescriptionText;
        public string ExerciseDescriptionText
        {
            get { return _exerciseDescriptionText; }
            set
            {
                if (_exerciseDescriptionText == value) return;
                _exerciseDescriptionText = value;
                OnPropertyChanged("ExerciseDescriptionText");
            }
        }

        private int _stackIndex;

        private readonly DispatcherTimer _dispatcherTimer;

        private readonly List<TimerStation> _timeStack;
        public WorkoutTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _timeStack = new List<TimerStation>();
            _stackIndex = 0;
        }
        public void BuildTimer(Workout workout)
        {
            // Summary
            //
            // Takes a workout and builds a stack of timer stations with each station
            // repersenting an exercise or rest

            int numStations = workout.Stations.Count;
            int stationReps = workout.StationReps;
            int exercisesPerStation = workout.Stations[0].Exercises.Count;

            // Looping through Stations
            for (int i=0; i < numStations; i++)
            {
                // Looping through Reps per Station
                for (int j=0; j < stationReps; j++)
                {
                    // Looing through Exercises
                    for (int k = 0; k < exercisesPerStation; k++)
                    {
                        // Add Exercise
                        _timeStack.Add(new TimerStation
                        {
                            ExerciseName = workout.Stations[i].Exercises[k].ExerciseName,
                            Description = workout.Stations[i].Exercises[k].Description,
                            ExerciseNumber = (k + 1).ToString(),
                            StationNumber = (i + 1).ToString(),
                            Time = new TimeSpan(0, 0, workout.RepSeconds)
                        });

                        System.Diagnostics.Debug.WriteLine("Exercise "+(i*exercisesPerStation + (k+1)).ToString());

                        // Add Rep Rest unless about to add a Station Rest

                        // Need to add 1 to avoid off-by-1 error since indices start at 0 & have to stop 1 early
                        if (((k + 1) == exercisesPerStation) && ((j + 1) == stationReps)) break;

                        _timeStack.Add(new TimerStation
                        {
                            ExerciseName = "Rest",
                            Description = String.Empty,
                            ExerciseNumber = (k + 1).ToString(),
                            StationNumber = (i + 1).ToString(),
                            Time = new TimeSpan(0, 0, workout.RestSeconds)
                        });


                        System.Diagnostics.Debug.WriteLine("10 Second Rest");
                    }
                }

                // Add Station Rest unless workout is over

                // Need to add 1 to avoid off-by-1 error since indices start at 0 & have to stop 1 early
                if ((i + 1) == numStations) break;

                _timeStack.Add(new TimerStation 
                {
                    ExerciseName = "Rest",
                    Description = String.Empty,
                    ExerciseNumber = "--",
                    StationNumber = "--",
                    Time = new TimeSpan(0,0,workout.SetSeconds)
                });

                System.Diagnostics.Debug.WriteLine("45 Second Rest");
            }
        }
        public void StartWorkout(Workout workout)
        {
            // Summary
            //
            // Set ticks to 1 second, sets initial timer properties, subscribes to event, starts timer

            _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            _dispatcherTimer.Tick += OnTick;

            // Build workout
            BuildTimer(workout);

            // Set initial properties of timer
            TimerText = (int)_timeStack[_stackIndex].Time.TotalSeconds;
            StationNumText = _timeStack[_stackIndex].StationNumber;
            ExerciseNumText = _timeStack[_stackIndex].ExerciseNumber;
            ExerciseNameText = _timeStack[_stackIndex].ExerciseName;
            ExerciseDescriptionText = _timeStack[_stackIndex].Description;

            _dispatcherTimer.Start();
        }
        public void PlayPauseWorkout()
        {
            // Summary
            //
            // Toggle timer Pause or Play Timer

            switch (_dispatcherTimer.IsEnabled)
            {
                case true:
                    _dispatcherTimer.Stop();
                    break;
                case false:
                    _dispatcherTimer.Start();
                    break;
            }
        }
        public void StopWorkout()
        {
            // Summary
            //
            // Stops timer

            if (_dispatcherTimer.IsEnabled) _dispatcherTimer.Stop();

            TimerText = null;
            StationNumText = String.Empty;
            ExerciseNumText = String.Empty;
            ExerciseNameText = String.Empty;
            ExerciseDescriptionText = String.Empty;
        }
        private void OnTick(object sender, EventArgs e)
        {
            // Summary
            //
            // On tick events, decrement timer by 1 second. Check if timer has hit 0. If so, then check if 
            // workout is over. If not, increment stack index and timer properties

            TimerText -= 1;

            // Current movement/rest is not yet over
            if (TimerText != 0) return;

            // Have finished final stackIndex item
            if ((_stackIndex + 1) == _timeStack.Count)
            {
                StopWorkout();
                return;
            }

            // Increment stack index and change timer properties
            _stackIndex++;

            TimerText = (int)_timeStack[_stackIndex].Time.TotalSeconds;
            StationNumText = _timeStack[_stackIndex].StationNumber;
            ExerciseNumText = _timeStack[_stackIndex].ExerciseNumber;
            ExerciseNameText = _timeStack[_stackIndex].ExerciseName;
            ExerciseDescriptionText = _timeStack[_stackIndex].Description;
        }
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}