using System;
using System.Collections.Generic;
using System.Text;
using WorkoutApp.Config;

namespace WorkoutApp.ViewModel
{
    public class ConfigVM
    {
        public Configuration Settings { get; set; }
        public ConfigVM()
        {
            Settings = Configuration.GetConfig();
        }
        public void SaveSettings()
        {
            // Summary
            //
            // Perform error handling, then update config file

            // TODO: Error handling

            Configuration.UpdateConfig(Settings);
        }
    }
}
