using PodcastApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WorkoutApp.Config;

namespace WorkoutApp.ViewModel
{
    public class ConfigVM : INotifyPropertyChanged
    {
        public Configuration Settings { get; set; }
        public ICommand SaveSettingsCommand { get; set; }

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
            CloseDialog = true;
        }

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}