using GongSolutions.Wpf.DragDrop;
using MongoDB.Bson;
using MongoDB.Driver;
using PodcastApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WorkoutApp.Config;
using WorkoutApp.Model;
using WorkoutApp.View;

namespace WorkoutApp.ViewModel
{
    public class MainVM : INotifyPropertyChanged, IDropTarget
    {
        private ObservableCollection<Exercise> _exercises;
        public ObservableCollection<Exercise> Exercises
        {
            get { return _exercises; }
            set
            {
                if (_exercises == value) return;
                _exercises = value;
                RaisePropertyChanged("Exercises");
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
                RaisePropertyChanged("Workouts");
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
                RaisePropertyChanged("Timer");
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
                RaisePropertyChanged("SelectedExercise");
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

                RandomWorkoutGenerated = false;

                if (value == null) WorkoutSelected = false;
                else if (value != null) WorkoutSelected = true;

                (SaveRandomWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
                (StartWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
                (UpdateWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
                (UseAsCustomBaseCommand as BaseCommand).RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedWorkout");
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
                RaisePropertyChanged("CustomWorkout");
            }
        }

        private ExerciseType _exerciseFilter;
        public ExerciseType ExerciseFilter
        {
            get { return _exerciseFilter; }
            set
            {
                if (_exerciseFilter == value) return;
                _exerciseFilter = value;
                RaisePropertyChanged("ExerciseFilter");

                if (_exerciseFilter == ExerciseType.All)
                {
                    SelectedExercise = null;
                    ReadExercises();
                }

                else
                {
                    SelectedExercise = null;
                    ReadExercises(_exerciseFilter);

                }
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

                WorkoutControlsAvailable = (!_workoutActive && _workoutSelected) ? true : false;

                RaisePropertyChanged("WorkoutActive");
                (SaveRandomWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
                (RandomWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
                (CustomWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
                (StartWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
                (AddExerciseCommand as BaseCommand).RaiseCanExecuteChanged();
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
                
                WorkoutControlsAvailable = (!_workoutActive && _workoutSelected) ? true : false;

                RaisePropertyChanged("WorkoutSelected");
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
                RaisePropertyChanged("BuildingWorkout");
                (SaveRandomWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
                (RandomWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
                (CustomWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
            }
        }

        private bool _randomWorkoutGenerated;
        public bool RandomWorkoutGenerated
        {
            get { return _randomWorkoutGenerated; }
            set
            {
                if (_randomWorkoutGenerated == value) return;
                _randomWorkoutGenerated = value;
                RaisePropertyChanged("RandomWorkoutGenerated");

                (SaveRandomWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
                (UpdateWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
            }
        }

        private bool _workoutControlsAvailable;
        public bool WorkoutControlsAvailable
        {
            get { return _workoutControlsAvailable; }
            set
            {
                if (_workoutControlsAvailable == value) return;
                _workoutControlsAvailable = value;
                RaisePropertyChanged("WorkoutControlsAvailable");
            }
        }

        private readonly Random _rng;
        private Configuration _config;
        private int _customWorkoutExerciseCount;

        public ICommand RandomWorkoutCommand { get; set; }
        public ICommand CustomWorkoutCommand { get; set; }
        public ICommand UseAsCustomBaseCommand { get; set; }
        public ICommand AbortCustomWorkoutCommand { get; set; }
        public ICommand AddToWorkoutCommand { get; set; }
        public ICommand RemoveFromWorkoutCommand { get; set; }
        public ICommand SaveRandomWorkoutCommand { get; set; }
        public ICommand SaveCustomWorkoutCommand { get; set; }
        public ICommand DeleteWorkoutCommand { get; set; }
        public ICommand UpdateWorkoutCommand { get; set; }
        public ICommand ExitApplicationCommand { get; set; }
        public ICommand StartWorkoutCommand { get; set; }
        public ICommand StopWorkoutCommand { get; set; }
        public ICommand PlayPauseWorkoutCommand { get; set; }
        public ICommand OpenConfigCommand { get; set; }
        public ICommand AddExerciseCommand { get; set; }

        public MainVM()
        {
            Exercises = new ObservableCollection<Exercise>();
            Workouts = new ObservableCollection<Workout>();
            CustomWorkout = new Workout();
            Timer = new WorkoutTimer();

            // Subscribe to the WorkoutFinished Event
            Timer.WorkoutFinished += OnWorkoutFinished;

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

            RandomWorkoutCommand = new BaseCommand(x => !(BuildingWorkout || WorkoutActive), x => GenerateRandomWorkout());
            CustomWorkoutCommand = new BaseCommand(x => !(BuildingWorkout || WorkoutActive), x => BuildCustomWorkout());
            UseAsCustomBaseCommand = new BaseCommand(x => !BuildingWorkout && (SelectedWorkout != null), x => UseAsCustomWorkoutBase(SelectedWorkout));
            AbortCustomWorkoutCommand = new BaseCommand(x => true, x => AbortCustomWorkout());
            AddToWorkoutCommand = new BaseCommand(x => true, e => AddExerciseToWorkout(e));
            RemoveFromWorkoutCommand = new BaseCommand(x => true, e => RemoveExerciseFromWorkout(e));
            SaveRandomWorkoutCommand = new BaseCommand(w => ((!(BuildingWorkout || WorkoutActive)) && w != null && RandomWorkoutGenerated), x => SaveRandomWorkout());
            SaveCustomWorkoutCommand = new BaseCommand(x => (_customWorkoutExerciseCount == 0 ? false : 
                (_customWorkoutExerciseCount == (CustomWorkout.Stations.Count * CustomWorkout.Stations[0].Exercises.Count))), w => SaveCustomWorkout(w));
            DeleteWorkoutCommand = new BaseCommand(x => true, w => DeleteWorkout(w));
            UpdateWorkoutCommand = new BaseCommand(x => ((SelectedWorkout != null) && !RandomWorkoutGenerated && !BuildingWorkout), x => UpdateWorkout(SelectedWorkout));
            ExitApplicationCommand = new BaseCommand(x => true, x => System.Windows.Application.Current.Shutdown());
            StartWorkoutCommand = new BaseCommand(x => ((!WorkoutActive) && (SelectedWorkout != null) & (!BuildingWorkout)), x => LoadTimer(SelectedWorkout));
            StopWorkoutCommand = new BaseCommand(x => true, x => StopWorkout());
            PlayPauseWorkoutCommand = new BaseCommand(x => true, x => Timer.PlayPauseWorkout());
            OpenConfigCommand = new BaseCommand(x => true, x => OpenConfig());
            AddExerciseCommand = new BaseCommand(x => !WorkoutActive, x => AddExercise());
        }
        public void ReadExercises()
        {
            // Summary
            // 
            // Read exercises from Database with DBHelper class. Clear existing exercises first.

            //var exercises = DatabaseHelper.GetExercises();
            var exercises = MongoHelper.GetExercises();

            Exercises.Clear();

            foreach (Exercise exercise in exercises)
            {
                Exercises.Add(exercise);
            }
        }
        public void ReadExercises(ExerciseType filter)
        {
            // Summary
            //
            // Read exercises and filter the list by a ExerciseType

            //var exercises = DatabaseHelper.GetExercises();
            var exercises = MongoHelper.GetExercises();

            Exercises.Clear();

            foreach (Exercise exercise in exercises)
            {
                if (exercise.ExerciseType == filter)
                {
                    Exercises.Add(exercise);

                }
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

            // Let user specify congig first
            OpenConfig();

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

            // Add workout length
            workout.Length = Workout.GenerateLength(workout);

            SelectedWorkout = workout;
            RandomWorkoutGenerated = true;
        }
        public void BuildCustomWorkout()
        {
            // Summary
            //
            // Build custom workout. Shows config first to configure workout.
            // Clear old custom workout. Set appropriate flags.

            ExerciseFilter = ExerciseType.All;

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

            _customWorkoutExerciseCount = 0;

            // Initialize empty CustomWorkout
            for (int i=0; i<_config.NumStations; i++)
            {
                CustomWorkout.Stations.Add(new Station());
                CustomWorkout.Stations[i].StationName = @"Station " + (i + 1).ToString();

                for (int j = 0; j < _config.NumExercisesPerStation; j++)
                {
                    CustomWorkout.Stations[i].Exercises.Add(null);
                }
            }
        }
        public void UseAsCustomWorkoutBase(object parameter) 
        {
            // Summary
            //
            // Take supplied workout and load into custom workout. Method is called from context menu from
            // Workouts list. Not re-calling Config since settings should be linked to imported workout. 

            var import = parameter as Workout;
            var numStations = import.Stations.Count;
            var numExercises = import.Stations[0].Exercises.Count;

            // Command casting & parameterization drops ObservableCollection<T> type. Need to rebox

            CustomWorkout.Name = "Copy of " + import.Name;
            CustomWorkout.Description = import.Description;
            CustomWorkout.Length = import.Length;
            CustomWorkout.RepSeconds = import.RepSeconds;
            CustomWorkout.RestSeconds = import.RestSeconds;
            CustomWorkout.SetSeconds = import.SetSeconds;
            CustomWorkout.StationReps = import.StationReps;

            CustomWorkout.Stations = new ObservableCollection<Station>();

            for (int i=0; i<numStations; i++)
            {
                CustomWorkout.Stations.Add(new Station
                {
                    StationName = "Station " + (i+1).ToString(),
                    Exercises = new ObservableCollection<Exercise>()
                });
                
                for(int j=0; j<numExercises; j++)
                {
                    CustomWorkout.Stations[i].Exercises.Add(import.Stations[i].Exercises[j]);
                }
            }

            _customWorkoutExerciseCount = CustomWorkout.Stations.Count * CustomWorkout.Stations[0].Exercises.Count;

            ExerciseFilter = ExerciseType.All;

            BuildingWorkout = true;

            (SaveCustomWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
        }
        public void AbortCustomWorkout()
        {
            // Summary
            //
            // Abort custom workout build. Reset properties and set flags

            BuildingWorkout = false;

            CustomWorkout = new Workout();
            _customWorkoutExerciseCount = 0;
        }
        public void AddExerciseToWorkout(object parameter)
        {
            // Summary
            //
            // Add supplied exercise to custom workout

            Exercise exercise = parameter as Exercise;

            int exercisesPerStation = CustomWorkout.Stations[0].Exercises.Count;
            int numStations = CustomWorkout.StationReps;
            int maxExerciseCount = numStations * exercisesPerStation;

            // Cant add above capacity of workout
            if (_customWorkoutExerciseCount == maxExerciseCount) return;

            // Loop through to find first open spot. Need to do so as user can remove any exercise.
            for (int i = 0; i < numStations; i++)
            {
                for (int j = 0; j < exercisesPerStation; j++)
                {
                    // Find first spot with no exercise
                    if (CustomWorkout.Stations[i].Exercises[j] == null)
                    {
                        CustomWorkout.Stations[i].Exercises[j] = exercise;
                        _customWorkoutExerciseCount++;
                        i = numStations - 1;
                        break;
                    }
                }
            }

            // Re-evaluate if Workout is full and can be saved
            (SaveCustomWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
        }
        public void RemoveExerciseFromWorkout(object parameter)
        {
            // Summary
            //
            // Remove supplied exercise from custom workout. Exercise still needs to be filled. Leave as 
            // empty exercise instead of a null reference

            if (_customWorkoutExerciseCount == 0) return;
            if (parameter == null) return;

            int numStations = CustomWorkout.Stations.Count;
            int numExercisesPerStation = CustomWorkout.Stations[0].Exercises.Count;

            Exercise exercise = parameter as Exercise;

            // Find exercise and remove
            for (int i = 0; i < numStations; i++)
            {
                for (int j = 0; j < numExercisesPerStation; j++)
                {
                    // Remove exercise if found, decrement, then return.
                    if (CustomWorkout.Stations[i].Exercises[j] == exercise)
                    {
                        CustomWorkout.Stations[i].Exercises[j] = null;
                        _customWorkoutExerciseCount--;

                        // Re-evaluate if Workout is full and can be saved
                        (SaveCustomWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();

                        return;
                    }
                }
            }

            // Have failed to find exercise
            throw new ArgumentException("Exercise not found");
        }
        public void SaveRandomWorkout()
        {
            // Summary
            //
            // Saves selected workout to the DB then refreshes the list

            MongoHelper.AddWorkoutAsync(SelectedWorkout);

            ReadWorkouts();
        }
        public void SaveCustomWorkout(object parameter)
        {
            // Summary
            //
            // Save Custom made workout

            if (parameter == null) throw new ArgumentNullException();

            (parameter as Workout).Length = Workout.GenerateLength(parameter as Workout);

            MongoHelper.AddWorkoutAsync(parameter as Workout);

            ReadWorkouts();

            BuildingWorkout = false;
            CustomWorkout = null;
            _customWorkoutExerciseCount = 0;
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
        public void AddExercise()
        {
            // Summary
            //
            // Execute method for AddExerciseCommand. Adds user submitted exercise.

            AddExerciseWindow addExerciseWindow = new AddExerciseWindow();
            addExerciseWindow.ShowDialog();

            //Refresh exercises
            ReadExercises();
        }

        public void OnWorkoutFinished(object sender, EventArgs e)
        {
            // Summary
            //
            // Event raised by Timer class when workout is called. Write record of workout to Db

            MongoHelper.AddRecordAsync(new Record(SelectedWorkout));
        }

        public void DragOver(IDropInfo dropInfo)
        {
            // Summary
            //
            // Event Handler for drag over a droptarget. Styles drop targets appropriately
            // Cases are a drop of a new exercise, or a re-order of the current station

            //var station = dropInfo.TargetCollection as ObservableCollection<Exercise>;

            var station = (ObservableCollection<Exercise>)dropInfo.TargetCollection;
            bool isReorder = station.Contains(dropInfo.Data as Exercise);

            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = isReorder ? DragDropEffects.Move : DragDropEffects.Copy;
        }
        public void Drop(IDropInfo dropInfo)
        {
            // Summary
            //
            // Drop Event handler. Need to ensure station can accept item. Four cases are:
            // - Station is not full & exercise is new > accept
            // - Station is full & exercise is new > reject
            // - Station is not full & exercise is a re-order > accept
            // - Station is full & exercise is a re-order > accept

            // If the dropInfo.InsertPosition is AfterTargetItem, or contains it (InsertPosition is bitwise enum)
            // then need to decrement the InsertIndex by one
            var targetIndex = (dropInfo.InsertPosition & RelativeInsertPosition.AfterTargetItem) == RelativeInsertPosition.AfterTargetItem
                ? dropInfo.InsertIndex - 1 : dropInfo.InsertIndex;

            var station = dropInfo.TargetCollection as ObservableCollection<Exercise>;
            var exercisesPerStation = station.Count;
            var exercise = dropInfo.Data as Exercise;

            System.Diagnostics.Debug.WriteLine(String.Format("Insert Index: {0}", targetIndex));
            System.Diagnostics.Debug.WriteLine(String.Format("Insert Position: {0}", dropInfo.InsertPosition));

            // Not allowed to expand Station size
            if (targetIndex == exercisesPerStation) return;

            // Variables for 4 insertion cases. Assign sourceIndex now while already
            // looping. Will only be assigned in case of reorder

            bool isReorder = station.Contains(exercise) ? true : false;
            int currentCount = 0;
            int sourceIndex = 0;

            // Get variables for drop cases
            for (int i = 0; i < exercisesPerStation; i++)
            {
                if (station[i] != null) currentCount++;
                if (station[i] == exercise) sourceIndex = i;
                if (station[i]?.ExerciseName == exercise.ExerciseName) isReorder = true;
            }

            // FOUR CASES
            // Station is full & not a reorder - reject
            if (currentCount == exercisesPerStation && !isReorder)
            {
                System.Diagnostics.Debug.WriteLine("Not Inserting - Full & Not Reorder");
                System.Diagnostics.Debug.WriteLine(String.Format("Target Index: {0}", targetIndex));
                return;
            }

            // Station is not full & not a reorder - accept
            if (currentCount < exercisesPerStation && !isReorder)
            {
                System.Diagnostics.Debug.WriteLine("Inserting - Not Full & Not Reorder");

                DropHelper.InsertItemIntoNotFullStructure<Exercise>(exercise, station, targetIndex);
                _customWorkoutExerciseCount++;
                (SaveCustomWorkoutCommand as BaseCommand).RaiseCanExecuteChanged();
                return;
            }

            // Station is full & a reorder - accept
            if (currentCount == exercisesPerStation && isReorder)
            {
                System.Diagnostics.Debug.WriteLine("Inserting - Full & Reorder");

                DropHelper.ReorderItemInFullStructure<Exercise>(exercise, station, targetIndex, sourceIndex);
                return;
            }

            // Station is not full & a reorder - accept
            if (currentCount < exercisesPerStation && isReorder)
            {
                System.Diagnostics.Debug.WriteLine("Inserting - Not Full & Reorder ");
                System.Diagnostics.Debug.WriteLine(String.Format("Target Index: {0}", targetIndex));

                DropHelper.ReorderItemInNotFullStructure<Exercise>(exercise, station, targetIndex, sourceIndex);
                return;
            }
        }

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}