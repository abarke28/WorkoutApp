using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutApp.Model
{
    [BsonIgnoreExtraElements]
    public class Station : INotifyPropertyChanged
    {
        public string StationName { get; set; }

        private IList<Exercise> _exercises;
        public IList<Exercise> Exercises
        {
            get { return _exercises; }
            set
            {
                if (_exercises == value) return;
                _exercises = value;
                RaisePropertyChanged("Exercises");
            }
        }

        public Station()
        {
            Exercises = new ObservableCollection<Exercise>();
        }

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}