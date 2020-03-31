using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WorkoutApp.Model
{
    public class TimerStation : INotifyPropertyChanged
    {

        private TimeSpan _time;
        public TimeSpan Time
        {
            get { return _time; }
            set
            {
                if (_time == value) return;
                _time = value;
                OnPropertyChanged("Time");
            }
        }

        private string _exerciseName;
        public string ExerciseName
        {
            get { return _exerciseName; }
            set
            {
                if (_exerciseName == value) return;
                _exerciseName = value;
                OnPropertyChanged("ExerciseName");
            }
        }

        private string _stationNumber;
        public string StationNumber
        {
            get { return _stationNumber; }
            set
            {
                if (_stationNumber == value) return;
                _stationNumber = value;
                OnPropertyChanged("StationNumber");
            }
        }

        private string _exerciseNumber;
        public string ExerciseNumber
        {
            get { return _exerciseNumber; }
            set
            {
                if (_exerciseNumber == value) return;
                _exerciseNumber = value;
                OnPropertyChanged("ExerciseNumber");
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged("Description");
            }
        }
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}