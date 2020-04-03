using PodcastApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WorkoutApp.Config;

namespace WorkoutApp.ViewModel
{
    public class ConfigVM
    {
        public Configuration Settings { get; set; }
        public ICommand SaveSettingsCommand { get; set; }
        public ConfigVM()
        {
            Settings = Configuration.GetConfig();

            InstatiateCommands();
        }
        public void InstatiateCommands()
        {
            // Summary
            //
            // Instatiate commands for ConfigVM

            // x=nothing, w=workout
            SaveSettingsCommand = new BaseCommand(x => true, x => SaveSettings());
        }
        public void SaveSettings()
        {
            // Summary
            //
            // Perform error handling, then update config file

            // TODO: Error handling

            Configuration.UpdateConfig(Settings);
            System.Threading.Thread.Sleep(500);
        }
    }
}
