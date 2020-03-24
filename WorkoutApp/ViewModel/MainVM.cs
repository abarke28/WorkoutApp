using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
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
                OnPropertyChanged("SelectedWorkout");
            }
        }
        public Random Rng { get; set; }
        public MainVM()
        {
            Exercises = new ObservableCollection<Exercise>();
            Workouts = new ObservableCollection<Workout>();

            // RNG for creating random workouts. Important to initialize only once
            Rng = new Random();


            ReadExercises();
            GenerateRandomWorkout();
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
        public Workout GenerateRandomWorkout(int numStations = 4, int numExercises = 3)
        {
            // Summary
            // 
            // Create randomized workout using degined number of 

            List<Exercise> randomizedExercises = new List<Exercise>();
            Workout randomizedWorkout = new Workout();

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

            for (int i = 0; i < numStations; i++)
            {
                for (int j = 0; j <numExercises; j++)
                {
                    randomizedWorkout.Stations[i].Exercises[j] = randomizedExercises[(3 * i + j)];
                }
            }

            return randomizedWorkout;
        }
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
