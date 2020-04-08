using PodcastApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WorkoutApp.Model;

namespace WorkoutApp.ViewModel
{
    public class AddExerciseVM : INotifyPropertyChanged
    {
        private bool? _closeDialog;
        public bool? CloseDialog
        {
            get { return _closeDialog; }
            set
            {
                if (_closeDialog == value) return;
                _closeDialog = value;
                OnPropertyChanged("CloseDialog");
            }
        }

        private Exercise _newExercise;
        public Exercise NewExercise
        {
            get { return _newExercise; }
            set
            {
                if (_newExercise == value) return;
                _newExercise = value;
                OnPropertyChanged("NewExercise");
            }
        }
        public List<ExerciseType> ExerciseTypes { get; set; }
        public ICommand SaveExerciseCommand { get; set; }
        public AddExerciseVM()
        {
            ExerciseTypes = Enum.GetValues(typeof(ExerciseType)).Cast<ExerciseType>().ToList();

            NewExercise = new Exercise
            {
                ExerciseType = ExerciseTypes.First()
            };

            InstantiateCommands();
        }
        public void InstantiateCommands()
        {
            // Summary
            //
            // Instantiate all commands here

            // x = nothing, b = bool, e = exercise, w = workout
            SaveExerciseCommand = new BaseCommand(x => CanSaveExercise(), x => SaveExercise());
        }
        public bool CanSaveExercise()
        {
            // Summary
            //
            // CanExecute Method for SaveExerciseCommand

            return true;
        }
        public void SaveExercise()
        {
            // Summary
            //
            // Perform error handling and add exercise

            DatabaseHelper.AddExercise(NewExercise);
            CloseDialog = true;
        }
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
