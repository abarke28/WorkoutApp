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
                RaisePropertyChanged("CloseDialog");
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
                RaisePropertyChanged("NewExercise");
            }
        }

        public List<ExerciseType> ExerciseTypes { get; set; }
        public ICommand SaveExerciseCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AddExerciseVM()
        {
            // Summary
            //
            // ExerciseTypes is data source for Radio Buttons. Subscribe to PropertyChanged event of NewExercise so that 
            // CanExecute logic can be called whenever NewExercise properties change

            ExerciseTypes = Enum.GetValues(typeof(ExerciseType)).Cast<ExerciseType>().Where(e => e != ExerciseType.All).ToList();

            NewExercise = new Exercise
            {
                ExerciseType = ExerciseTypes.First()
            };

            InstantiateCommands();
            NewExercise.PropertyChanged += OnNewExercisePropertyChanged;
        }
        
        public void InstantiateCommands()
        {
            // Summary
            //
            // Instantiate all commands here

            // x = nothing, b = bool, e = exercise, w = workout
            SaveExerciseCommand = new BaseCommand(x => CanSaveExercise(), x => SaveExercise());
            CancelCommand = new BaseCommand(x => true, x => CloseDialog = true);
        }
        public bool CanSaveExercise()
        {
            // Summary
            //
            // CanExecute Method for SaveExerciseCommand

            return !(String.IsNullOrWhiteSpace(NewExercise.ExerciseName) || String.IsNullOrEmpty(NewExercise.Description));
            //return true;
        }
        public void SaveExercise()
        {
            // Summary
            //
            // Perform error handling and add exercise

            //DatabaseHelper.AddExercise(NewExercise);
            MongoHelper.AddExercise(NewExercise);

            CloseDialog = true;
        }

        private void OnNewExercisePropertyChanged(object sender, EventArgs e)
        {
            // Summary
            //
            // When property of NewExercise changes, need to re-evaluate can-execute logic for SaveExerciseCommand

            (SaveExerciseCommand as BaseCommand).RaiseCanExecuteChanged();
        }

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
