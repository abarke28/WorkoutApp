using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WorkoutApp.Model
{
    [BsonIgnoreExtraElements]
    public class Workout : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int RepSeconds { get; set; }
        public int RestSeconds { get; set; }
        public int SetSeconds { get; set; }
        public int StationReps { get; set; }
        public TimeSpan Length { get; set; }
        [BsonIgnore]
        private IList<Station> _stations;
        public IList<Station> Stations
        {
            get { return _stations; }
            set
            {
                if (_stations == value) return;
                _stations = value;
                OnPropertyChanged("Stations");
            }
        }

        public Workout()
        {
            Stations = new List<Station>();
        }

        public static TimeSpan GenerateLength(Workout workout)
        {
            // Add workout length
            TimeSpan workoutLength = TimeSpan.Zero;

            // Add exercise time
            workoutLength += TimeSpan.FromSeconds(workout.StationReps * workout.Stations.Count * workout.Stations[0].Exercises.Count * workout.RepSeconds);

            // Add exercise rest times. Take one from exercises per station since last rep is followed by station break
            workoutLength += TimeSpan.FromSeconds((workout.StationReps * workout.Stations.Count * workout.Stations[0].Exercises.Count - workout.Stations.Count) * workout.RestSeconds);

            // Add station rest times. Take one from number of stations since last station ends workout
            workoutLength += TimeSpan.FromSeconds((workout.Stations.Count - 1) * workout.SetSeconds);

            // Add 10s for start of workout countdown
            workoutLength += TimeSpan.FromSeconds(10);

            // Add 1s per exercise & 1s per rest since timer actually shows 0. Take 1 second off to skip final 0
            workoutLength += TimeSpan.FromSeconds(2 * workout.Stations.Count * workout.Stations[0].Exercises.Count * workout.StationReps - 1);

            return workoutLength;
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}