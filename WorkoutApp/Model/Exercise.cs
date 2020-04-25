using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkoutApp.Model
{
    [BsonIgnoreExtraElements]
    public class Exercise : INotifyPropertyChanged
    {
        public int? ExerciseId { get; set; }

        [NotMapped]
        private string _exerciseName;
        public string ExerciseName
        {
            get { return _exerciseName; }
            set
            {
                if (_exerciseName == value) return;
                _exerciseName = value;
                RaisePropertyChanged("ExerciseName");
            }
        }

        [NotMapped]
        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value) return;
                _description = value;
                RaisePropertyChanged("Description");
            }
        }

        public ExerciseType ExerciseType { get; set; }

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}