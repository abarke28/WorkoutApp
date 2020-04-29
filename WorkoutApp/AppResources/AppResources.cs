using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutApp.AppResources
{
    public static class AppResources
    {
        public static string PLAYIMAGE { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Poor Yorrick Software\Poor Yorrick Workouts\resources\Play.png";
        public static string PAUSEIMAGE { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Poor Yorrick Software\Poor Yorrick Workouts\resources\Pause.png";
        public static string REST { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Poor Yorrick Software\Poor Yorrick Workouts\resources\rest.wav";
        public static string ONE { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Poor Yorrick Software\Poor Yorrick Workouts\resources\one.wav";
        public static string TWO { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Poor Yorrick Software\Poor Yorrick Workouts\resources\two.wav";
        public static string THREE { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Poor Yorrick Software\Poor Yorrick Workouts\resources\three.wav";
        public static string GO { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Poor Yorrick Software\Poor Yorrick Workouts\resources\go.wav";
        public static string DONE { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Poor Yorrick Software\Poor Yorrick Workouts\resources\done.wav";
        public static string GETREADY { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Poor Yorrick Software\Poor Yorrick Workouts\resources\getready.wav";
        public static string SEGFONT { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Poor Yorrick Software\Poor Yorrick Workouts\resources\Ni7seg.ttf";
    }
}