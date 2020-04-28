using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutApp.AppResources
{
    public static class AppResources
    {
        public static string PLAYIMAGE { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Poor Yorrick Workouts\Play.png";
        public static string PAUSEIMAGE { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Poor Yorrick Workouts\Pause.png";
        public static string REST { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Poor Yorrick Workouts\rest.wav";
        public static string ONE { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Poor Yorrick Workouts\one.wav";
        public static string TWO { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Poor Yorrick Workouts\two.wav";
        public static string THREE { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Poor Yorrick Workouts\three.wav";
        public static string GO { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Poor Yorrick Workouts\go.wav";
        public static string DONE { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Poor Yorrick Workouts\done.wav";
        public static string GETREADY { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Poor Yorrick Workouts\getready.wav";
    }
}