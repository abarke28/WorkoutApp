using MongoDB.Bson;
using MongoDB.Driver;
using PodcastApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using WorkoutApp.Config;
using WorkoutApp.Model;
using WorkoutApp.View;

namespace WorkoutApp.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        private ObservableCollection<Exercise> _exercises;
        public ObservableCollection<Exercise> Exercises
        {
            get { return _exercises; }
            set
            {
                if (_exercises == value) return;
                _exercises = value;
                OnPropertyChanged("Exercises");
            }
        }

        private ObservableCollection<Workout> _workouts;
        public ObservableCollection<Workout> Workouts
        {
            get { return _workouts; }
            set
            {
                if (_workouts == value) return;
                _workouts = value;
                OnPropertyChanged("Workouts");
            }
        }

        private WorkoutTimer _timer;
        public WorkoutTimer Timer
        {
            get { return _timer; }
            set
            {
                if (_timer == value) return;
                _timer = value;
                OnPropertyChanged("Timer");
            }
        }

        private Exercise _selectedExercise;
        public Exercise SelectedExercise
        {
            get { return _selectedExercise; }
            set
            {
                if (_selectedExercise == value) return;
                _selectedExercise = value;
                OnPropertyChanged("SelectedExercise");
            }
        }

        private Workout _selectedWorkout;
        public Workout SelectedWorkout
        {
            get { return _selectedWorkout; }
            set
            {
                if (_selectedWorkout == value) return;
                _selectedWorkout = value;

                if (value == null) WorkoutSelected = false;
                else if (value != null) WorkoutSelected = true;

                (SaveWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
                (StartWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedWorkout");
            }
        }

        private Workout _customWorkout;
        public Workout CustomWorkout
        {
            get { return _customWorkout; }
            set
            {
                if (_customWorkout == value) return;
                _customWorkout = value;
                OnPropertyChanged("CustomWorkout");
            }
        }

        private bool _workoutActive;
        public bool WorkoutActive
        {
            get { return _workoutActive; }
            set
            {
                if (_workoutActive == value) return;
                _workoutActive = value;
                OnPropertyChanged("WorkoutActive");
            }
        }

        private bool _workoutSelected;
        public bool WorkoutSelected
        {
            get { return _workoutSelected; }
            set
            {
                if (_workoutSelected == value) return;
                _workoutSelected = value;
                OnPropertyChanged("WorkoutSelected");
            }
        }

        private bool _buildingWorkout;
        public bool BuildingWorkout
        {
            get { return _buildingWorkout; }
            set
            {
                if (_buildingWorkout == value) return;
                _buildingWorkout = value;
                OnPropertyChanged("BuildingWorkout");
            }
        }

        private readonly Random _rng;

        private Configuration _config;
        public ICommand RandomWorkoutCommand { get; set; }
        public ICommand CustomWorkoutCommand { get; set; }
        public ICommand AddToWorkoutCommand { get; set; }
        public ICommand RemoveFromWorkoutCommand { get; set; }
        public ICommand SaveWorkoutCommand { get; set; }
        public ICommand DeleteWorkoutCommand { get; set; }
        public ICommand UpdateWorkoutCommand { get; set; }
        public ICommand ExitApplicationCommand { get; set; }
        public ICommand StartWorkoutCommand { get; set; }
        public ICommand StopWorkoutCommand { get; set; }
        public ICommand PlayPauseWorkoutCommand { get; set; }
        public ICommand OpenConfigCommand { get; set; }
        public MainVM()
        {
            Exercises = new ObservableCollection<Exercise>();
            Workouts = new ObservableCollection<Workout>();
            CustomWorkout = new Workout();
            Timer = new WorkoutTimer();

            _rng = new Random();
            _config = Configuration.GetConfig();

            InstantiateCommands();

            ReadExercises();
            ReadWorkouts();
        }
        public void InstantiateCommands()
        {
            // Summary
            //
            // Instantiate all commands for VM

            // x = nothing, b = bool, e = exercise, w = workout

            RandomWorkoutCommand = new BaseCommand(x => true, x => GenerateRandomWorkout());
            CustomWorkoutCommand = new BaseCommand(x => true, x => BuildCustomWorkout());
            AddToWorkoutCommand = new BaseCommand(x => true, e => AddExerciseToWorkout(e));
            RemoveFromWorkoutCommand = new BaseCommand(x => true, e => RemoveExerciseFromWorkout(e));
            SaveWorkoutCommand = new BaseCommand(w => w != null, x => SaveWorkout());
            DeleteWorkoutCommand = new BaseCommand(x => true, w => DeleteWorkout(w));
            UpdateWorkoutCommand = new BaseCommand(x => true, w => UpdateWorkout(w));
            ExitApplicationCommand = new BaseCommand(x => true, x => ExitApplication());
            StartWorkoutCommand = new BaseCommand(w => w != null, w => LoadTimer(w));
            StopWorkoutCommand = new BaseCommand(x => true, x => StopWorkout());
            PlayPauseWorkoutCommand = new BaseCommand(x => true, x => PlayPauseWorkout());
            OpenConfigCommand = new BaseCommand(x => true, x => OpenConfig());
        }
        public void ReadExercises()
        {
            // Summary
            // 
            // Read exercises from Database with DBHelper class. Clear existing exercises first.

            var exercises = DatabaseHelper.GetExercises();

            Exercises.Clear();

            foreach (Exercise exercise in exercises)
            {
                Exercises.Add(exercise);
            }
        }
        public void ReadWorkouts()
        {
            // Summary
            //
            // Read workouts from Database with DbHelper class. Clear existing workouts first.

            var workouts = MongoHelper.GetWorkoutsAsync();

            Workouts.Clear();

            foreach (var workout in workouts)
            {
                Workouts.Add(workout);
            }
        }
        public void GenerateRandomWorkout()
        {
            // Summary
            //
            // Generates new randomized workout

            // Worker variables
            List<Exercise> randomizedExercises = new List<Exercise>();

            var exercises = (List<Exercise>)DatabaseHelper.GetExercises();

            int randIndex;

            // Get parameters from config

            if (_config == null) _config = Configuration.GetConfig();

            int numStations = _config.NumStations;
            int numExercises = _config.NumExercisesPerStation;
            int stationReps = _config.NumRounds;

            int repSeconds = _config.ExerciseLength;
            int restSeconds = _config.ExerciseRestLength;
            int setSeconds = _config.StationRestLength;

            // Get list of distinct randomized exercises of necessary size
            while (randomizedExercises.Count != numStations * numExercises)
            {
                randIndex = _rng.Next(exercises.Count);

                if (!randomizedExercises.Contains(exercises[randIndex]))
                {
                    randomizedExercises.Add(exercises[randIndex]);
                }
            }

            Workout workout = new Workout
            {
                Name = "New Workout",
                Description = "Randomly Generated Workout",
                RepSeconds = repSeconds,
                RestSeconds = restSeconds,
                SetSeconds = setSeconds,
                StationReps = stationReps
            };

            // Fill workout
            for (int i = 0; i < numStations; i++)
            {
                workout.Stations.Add(new Station());
                workout.Stations[i].StationName = ("Station " + (i + 1).ToString());
                workout.Stations[i].Exercises = new List<Exercise>();

                for (int j = 0; j < numExercises; j++)
                {
                    workout.Stations[i].Exercises.Add(new Exercise());
                    workout.Stations[i].Exercises[j] = randomizedExercises[numExercises * i + j];
                }
            }

            SelectedWorkout = workout;
        }
        public void BuildCustomWorkout()
        {
            // Summary
            //
            // Build custom workout. Shows config first to configure workout.
            // Clear old custom workout. Set appropriate flags.

            BuildingWorkout = true;

            OpenConfig();

            CustomWorkout = new Workout
            {
                Name = "Custom Workout",
                Description = "User generated Workout",
                RepSeconds = _config.ExerciseLength,
                RestSeconds = _config.ExerciseRestLength,
                SetSeconds = _config.StationRestLength,
                StationReps = _config.NumRounds,
            };
        }
        public void AddExerciseToWorkout(object parameter)
        {
            // Summary
            //
            // Add supplied exercise to custom workout
            
            throw new NotImplementedException();
        }
        public void RemoveExerciseFromWorkout(object parameter)
        {
            // Summary
            //
            // Remove supplied exercise from custom workout

            throw new NotImplementedException();
        }
        public void SaveWorkout()
        {
            // Summary
            //
            // Saves selected workout to the DB then refreshes the list

            MongoHelper.AddWorkoutAsync(SelectedWorkout);

            ReadWorkouts();
        }
        public void DeleteWorkout(object parameter)
        {
            // Summary
            //
            // Bound to DeleteWorkoutCommand. Call appropriate method in
            // MongoHelper class

            if (!(parameter is Workout)) throw new ArgumentNullException();

            MongoHelper.DeleteWorkout(parameter as Workout);

            // Reset selection and repoll database
            SelectedWorkout = null;
            ReadWorkouts();
        }
        public void UpdateWorkout(object parameter)
        {
            // Summary
            //
            // Update supplied workout

            SelectedWorkout = null;

            MongoHelper.UpdateWorkout(parameter as Workout);

            ReadWorkouts();
        }
        public void LoadTimer(object parameter)
        {
            // Summary
            //
            // Tied to StartWorkoutCommand, calls appropriate method in Timer object

            if (!(parameter is Workout)) throw new ArgumentNullException();

            Timer.LoadWorkout(parameter as Workout);
            WorkoutActive = true;
        }
        public void PlayPauseWorkout()
        {
            // Summary
            //
            // Calls timers play pause method

            Timer.PlayPauseWorkout();
        }
        public void StopWorkout()
        {
            // Summary
            //
            // Stop workout and set appropriate flags

            Timer.StopWorkout();
            WorkoutActive = false;
        }
        public void OpenConfig()
        {
            // Summary
            //
            // Called by OpenConfigCommand. Opens ConfigWindow

            ConfigWindow configWindow = new ConfigWindow();
            configWindow.ShowDialog();

            //Refresh config
            _config = Configuration.GetConfig();
        }
        public void ExitApplication()
        {
            // Summary
            //
            // Exit application

            System.Windows.Application.Current.Shutdown();
        }
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}