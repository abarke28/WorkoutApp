using System;
using WorkoutApp.Model;
using WorkoutApp.ViewModel;
using Xunit;

namespace WorkoutAppTests
{
    public class WorkoutTimerTests
    {
        [Fact]
        public void BuildTimer_NullWorkoutSupplied_ThrowsArgumentNullException()
        {
            // Arrange
            var workoutTimer = new WorkoutTimer();
            var workout = new Workout();

            // Act & Assert - 
            Assert.Throws<ArgumentNullException>(()=>workoutTimer.BuildTimer(workout));
        }

        [Fact]
        public void BuildTimer_WhenCalled_BuildsCorrectTimer()
        {
            // Arrange
            //var workoutTimer = new WorkoutTimer();
            //var vm = new MainVM();
        }
    }
}