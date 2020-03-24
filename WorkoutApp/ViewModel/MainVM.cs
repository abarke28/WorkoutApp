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
        private ObservableCollection<Exercise>  exercises;
        public ObservableCollection<Exercise> Exercises
        {
            get { return exercises; }
            set
            {
                if (exercises == value) return;
                exercises = value;
                OnPropertyChanged("Exercises");
            }
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
