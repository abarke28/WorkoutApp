using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Media;
using System.Text;
using System.Windows.Threading;
using WorkoutApp.AppResources;

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

        private TimeSpan _timeToGo;
        public TimeSpan TimeToGo
        {
            get { return _timeToGo; }
            set
            {
                if (_timeToGo == value) return;
                _timeToGo = value;
                OnPropertyChanged("TimeToGo");
            }
        }

        private TimeSpan _timeElapsed;
        public TimeSpan TimeElapsed
        {
            get { return _timeElapsed; }
            set
            {
                if (_timeElapsed == value) return;
                _timeElapsed = value;
                OnPropertyChanged("TimeElapsed");
            }
        }

        private string _roundNumText;
        public string RoundNumText
        {
            get { return _roundNumText; }
            set
            {
                if (_roundNumText == value) return;
                _roundNumText = value;
                OnPropertyChanged("RoundNumText");
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

        private string _playPauseButtonSource;
        public string PlayPauseButtonSource
        {
            get { return _playPauseButtonSource; }
            set
            {
                if (_playPauseButtonSource == value) return;
                _playPauseButtonSource = value;
                OnPropertyChanged("PlayPauseButtonSource");
            }
        }

        private int _stackIndex;
        private readonly SoundPlayer _soundPlayer;
        private readonly DispatcherTimer _dispatcherTimer;
        private readonly List<TimerStation> _timeStack;

        public WorkoutTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += OnTick;
            _soundPlayer = new SoundPlayer();
            _timeStack = new List<TimerStation>();
            _stackIndex = 0;

            TimeElapsed = TimeSpan.Zero;
            TimeToGo = TimeSpan.Zero;

            PlayPauseButtonSource = AppResources.AppResources.PLAYIMAGE;
        }

        public void BuildTimer(Workout workout)
        {
            // Summary
            //
            // Takes a workout and builds a stack of timer stations with each station repersenting
            // an exercise or rest.

            if (workout.Stations.Count == 0) throw new ArgumentNullException(nameof(workout));

            _timeStack.Clear();

            int numStations = workout.Stations.Count;
            int stationReps = workout.StationReps;
            int exercisesPerStation = workout.Stations[0].Exercises.Count;

            // Add 10s countdown for start of workout
            _timeStack.Add(new TimerStation
            {
                ExerciseName = "Get Ready",
                Description = "Up Next: " + workout.Stations[0].Exercises[0].ExerciseName,
                ExerciseNumber = "--",
                RoundNumber = "--",
                StationNumber = "--",
                Time = TimeSpan.FromSeconds(10)
            });

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
                            ExerciseNumber = (k + 1).ToString() + @" / " + exercisesPerStation.ToString(),
                            RoundNumber = (j + 1).ToString() + @" / " + stationReps.ToString(),
                            StationNumber = (i + 1).ToString() + @" / " + numStations.ToString(),
                            Time = TimeSpan.FromSeconds(workout.RepSeconds)
                        });

                        //System.Diagnostics.Debug.WriteLine(_timeStack[^1].ExerciseName);

                        // Add Rep Rest unless about to add a Station Rest

                        // Need to add 1 to avoid off-by-1 error since indices start at 0 & have to stop 1 early
                        if (((k+1) == exercisesPerStation) && ((j+1) == stationReps)) break;

                        _timeStack.Add(new TimerStation
                        {
                            ExerciseName = "Rest",
                            Description = "Up Next: " + workout.Stations[i].Exercises[(k+1)%exercisesPerStation].ExerciseName,
                            ExerciseNumber = (k + 1).ToString() + @" / " + exercisesPerStation.ToString(),
                            RoundNumber = (j + 1).ToString() + @" / " + stationReps.ToString(),
                            StationNumber = (i + 1).ToString() + @" / " + numStations.ToString(),
                            Time = TimeSpan.FromSeconds(workout.RestSeconds)
                        });

                        //System.Diagnostics.Debug.WriteLine(_timeStack[^1].ExerciseName + " - " + _timeStack[^1].Description);
                    }
                }

                // Add Station Rest unless workout is over

                // Need to add 1 to avoid off-by-1 error since indices start at 0 & have to stop 1 early
                if ((i + 1) == numStations) break;

                _timeStack.Add(new TimerStation 
                {
                    ExerciseName = "Rest",
                    Description = "Up Next: " + workout.Stations[i+1].Exercises[0].ExerciseName,
                    ExerciseNumber = "--",
                    RoundNumber = "--",
                    StationNumber = "--",
                    Time = TimeSpan.FromSeconds(workout.SetSeconds)
                });

                //System.Diagnostics.Debug.WriteLine(_timeStack[^1].ExerciseName + " - " + _timeStack[^1].Description);
            }

            // Set TimeToGo to total workout length. Clear first
            TimeElapsed = TimeSpan.Zero;
            TimeToGo = Workout.GenerateLength(workout);
        }
        public void LoadWorkout(Workout workout)
        {
            // Summary
            //
            // Set ticks to 1 second, sets initial timer properties, does not actually start timer
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);

            // Build workout
            BuildTimer(workout);

            // Set initial properties of timer
            TimerText = (int)_timeStack[_stackIndex].Time.TotalSeconds;
            RoundNumText = _timeStack[_stackIndex].RoundNumber;
            StationNumText = _timeStack[_stackIndex].StationNumber;
            ExerciseNumText = _timeStack[_stackIndex].ExerciseNumber;
            ExerciseNameText = _timeStack[_stackIndex].ExerciseName;
            ExerciseDescriptionText = _timeStack[_stackIndex].Description;

            _dispatcherTimer.IsEnabled = false;
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
                    PlayPauseButtonSource = AppResources.AppResources.PLAYIMAGE;
                    break;
                case false:
                    _dispatcherTimer.Start();
                    PlayPauseButtonSource = AppResources.AppResources.PAUSEIMAGE;
                    break;
            }
        }
        public void StopWorkout()
        {
            // Summary
            //
            // Stops timer

            if (_dispatcherTimer.IsEnabled) _dispatcherTimer.Stop();

            TimerText = 0;
            TimeToGo = TimeSpan.Zero;
            ExerciseNameText = "Good Job!";
            ExerciseDescriptionText = String.Empty;
            PlayPauseButtonSource = AppResources.AppResources.PLAYIMAGE;

            // Reset appropriate properties

            _stackIndex = 0;
        }
        private void OnTick(object sender, EventArgs e)
        {
            // Summary
            //
            // On tick events, decrement timer by 1 second. Check if timer has hit 0. If so, then check if 
            // workout is over. If not, increment stack index and timer properties

            TimerText -= 1;

            // Increment TimeElapsed, decrement TimeToGo
            TimeElapsed += TimeSpan.FromSeconds(1);
            TimeToGo -= TimeSpan.FromSeconds(1);

            // Play sounds counting down
            switch (TimerText)
            {
                case 3:
                    _soundPlayer.SoundLocation = AppResources.AppResources.THREE;
                    _soundPlayer.Play();
                    break;

                case 2:
                    _soundPlayer.SoundLocation = AppResources.AppResources.TWO;
                    _soundPlayer.Play();
                    break;

                case 1:
                    _soundPlayer.SoundLocation = AppResources.AppResources.ONE;
                    _soundPlayer.Play();
                    break;

                case 0:
                    // As long as workout is not over.. if next station is Rest, play rest.wav, else, play go.wav
                    if (!((_stackIndex + 1) == _timeStack.Count))
                    {
                        switch (_timeStack[_stackIndex + 1].ExerciseName)
                        {
                            case "Rest":
                                _soundPlayer.SoundLocation = AppResources.AppResources.REST;
                                _soundPlayer.Play();
                                break;

                            default:
                                _soundPlayer.SoundLocation = AppResources.AppResources.GO;
                                _soundPlayer.Play();
                                break;
                        }
                    }
                    break;

                case 9:
                    // Play get ready sound file if very start of workout. Check this last as it only occurs once
                    if (_stackIndex != 0) break;

                    _soundPlayer.SoundLocation = AppResources.AppResources.GETREADY;
                    _soundPlayer.Play();
                    break;
            }

            // Current movement/rest is not yet over. Let timer hit 0.
            if (TimerText != -1) return;

            // Have finished final stackIndex item
            if ((_stackIndex + 1) == _timeStack.Count)
            {
                _soundPlayer.SoundLocation = AppResources.AppResources.DONE;
                _soundPlayer.Play();
                StopWorkout();
                RaiseWorkoutFinished();
                return;
            }

            // Increment stack index and change timer properties
            _stackIndex++;

            TimerText = (int)_timeStack[_stackIndex].Time.TotalSeconds;
            RoundNumText = _timeStack[_stackIndex].RoundNumber;
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

        private void RaiseWorkoutFinished()
        {
            // Summary
            //
            // Raise event for workout completed

            WorkoutFinished?.Invoke(this, new EventArgs());
        }
        public event EventHandler WorkoutFinished;
    }
}