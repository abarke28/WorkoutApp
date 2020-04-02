using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using utilities;

namespace WorkoutApp.Config
{
    public class Config
    {
        public static string ConfigFilePath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
            + @"\Poor Yorrick Workouts\config.xml";
        public string AppName { get; set; }
        public string ConfigPath { get; set; }
        public string ConfigFileName { get; set; }
        public int NumStations { get; set; }
        public int NumExercisesPerStation { get; set; }
        public int NumRounds { get; set; }
        public int ExerciseLength { get; set; }
        public int ExerciseRestLength { get; set; }
        public int StationRestLength { get; set; }
        private Config()
        {
            // Summary
            //
            // Will only be called if there is no existing config file

            AppName = @"Poor Yorrick Workouts";
            ConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\" + AppName;
            ConfigFileName = @"config.xml";

            NumStations = 4;
            NumExercisesPerStation = 3;
            NumRounds = 4;

            ExerciseLength = 35;
            ExerciseRestLength = 10;
            StationRestLength = 45;
        }
        public static Config GetConfig()
        {
            // Summary
            //
            // Check if config exists. If so, return it. Else, construct a
            // new one, serialize it, then return the object.

            if (!(File.Exists(ConfigFilePath)))
            {
                Config config = new Config();
                Serializer.SerializeToXmlFile<Config>(ConfigFilePath, config);
            }

            return Serializer.DeserializeFromXmlFile<Config>(ConfigFilePath);
        }
    }
}