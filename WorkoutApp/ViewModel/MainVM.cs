using PodcastApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using WorkoutApp.Model;

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
                SaveWorkoutCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedWorkout");
            }
        }
        public Random Rng { get; set; }
        public ICommand RandomWorkoutCommand { get; set; }
        public BaseCommand SaveWorkoutCommand { get; set; }
        public MainVM()
        {
            Exercises = new ObservableCollection<Exercise>();
            Workouts = new ObservableCollection<Workout>();
            Rng = new Random();

            InstantiateCommands();
            ReadExercises();
        }
        public void InstantiateCommands()
        {
            // x = nothing
            RandomWorkoutCommand = new BaseCommand(x => true, x => GenerateRandomWorkout());
            SaveWorkoutCommand = new BaseCommand(w => w != null, x => SaveWorkout());
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

            var workouts = DatabaseHelper.GetWorkouts();

            Workouts.Clear();

            foreach(Workout workout in workouts)
            {
                Workouts.Add(workout);
            }
        }
        public void GenerateRandomWorkout(int numStations = 4, int numExercises = 3)
        {
            // Summary
            // 
            // Create randomized workout using defined sizes with unique exercises

            //TODO: Parameterize numStations and numExrcises via Config 

            //DateTime dateTime1 = DateTime.Now;

            //List<Exercise> randomizedExercises = new List<Exercise>();
            //Workout randomizedWorkout = new Workout { Name = "New Workout" };

            //var exercises = (List<Exercise>)DatabaseHelper.GetExercises();

            //int randIndex;

            //// Don't want any repeats. Loop until randomizedExercises has correct number of unique exercises

            //while (randomizedExercises.Count != numStations * numExercises)
            //{
            //    randIndex = Rng.Next(exercises.Count);

            //    if (!randomizedExercises.Contains(exercises[randIndex]))
            //    {
            //        randomizedExercises.Add(exercises[randIndex]);
            //    }
            //}

            //// Add exercises into workout

            //for (int i = 0; i < numStations; i++)
            //{
            //    randomizedWorkout.Stations.Add(new Station());
            //    randomizedWorkout.Stations[i].StationName = ((i + 1).ToString());
            //    for (int j = 0; j < numExercises; j++)
            //    {
            //        randomizedWorkout.Stations[i].Exercises.Add(new Exercise());
            //        randomizedWorkout.Stations[i].Exercises[j] = randomizedExercises[(3 * i + j)];
            //    }
            //}

            //System.Diagnostics.Debug.WriteLine("Generated Workout in " + (DateTime.Now - dateTime1).TotalMilliseconds.ToString() + " ms");

            //SelectedWorkout = randomizedWorkout;

            DateTime dateTime1 = DateTime.Now;

            List<Exercise> randomizedExercises = new List<Exercise>();
            Workout randomizedWorkout = new Workout { Name = "New Workout" };

            var exercises = (List<Exercise>)DatabaseHelper.GetExercises();

            int randIndex;

            while (randomizedExercises.Count != numStations * numExercises)
            {
                randIndex = Rng.Next(exercises.Count);

                if (!randomizedExercises.Contains(exercises[randIndex]))
                {
                    randomizedExercises.Add(exercises[randIndex]);
                }
            }

            for (int i=0; i < numStations; i++)
            {
                randomizedWorkout.Stations.Add(new Station());
                randomizedWorkout.Stations[i].StationName = "Station " + (1 + i).ToString();

                for (int j=0; j< numExercises; j++)
                {
                    ExerciseStation es = new ExerciseStation();
                    es.Station = randomizedWorkout.Stations[i];
                    es.Exercise = randomizedExercises[numExercises * i + j];

                    randomizedWorkout.Stations[i].ExerciseStations.Add(es);

                    DatabaseHelper.AddExerciseStation(es);
                    DatabaseHelper.AddStation(randomizedWorkout.Stations[i]);
                }
            }

            System.Diagnostics.Debug.WriteLine("Generated Workout in " + (DateTime.Now - dateTime1).TotalMilliseconds.ToString() + " ms");

            SelectedWorkout = randomizedWorkout;
        }
        public void SaveWorkout()
        {
            // Summary
            //
            // Saves selected workout to the DB then refreshes the list

            DatabaseHelper.AddWorkout(SelectedWorkout);
            ReadWorkouts();
        }
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
