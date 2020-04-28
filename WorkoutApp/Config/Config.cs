using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using utilities;

namespace WorkoutApp.Config
{
    public class Configuration
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
        
        private Configuration()
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

        public static Configuration GetConfig()
        {
            // Summary
            //
            // Check if config exists. If so, return it. Else, construct a
            // new one, serialize it, then return the object. Create directory if necessary

            if (!(File.Exists(ConfigFilePath)))
            {
                Configuration config = new Configuration();

                if (!Directory.Exists(config.ConfigPath))
                {
                    Directory.CreateDirectory(config.ConfigPath);
                }
                
                Serializer.SerializeToXmlFile<Configuration>(ConfigFilePath, config);

            }

            return Serializer.DeserializeFromXmlFile<Configuration>(ConfigFilePath);
        }
        public static void UpdateConfig(Configuration config)
        {
            // Summary
            //
            // Write supplied config file, method is used to updates settings

            if (!(File.Exists(ConfigFilePath)))
            {
                Configuration newConfig = new Configuration();
                Serializer.SerializeToXmlFile<Configuration>(ConfigFilePath, newConfig);
            }

            Serializer.SerializeToXmlFile<Configuration>(ConfigFilePath, config);
        }
    }
}