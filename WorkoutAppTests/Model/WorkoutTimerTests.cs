using System;
using WorkoutApp.Model;
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

            // Act & Assert
            Assert.Throws<ArgumentNullException>(()=>workoutTimer.BuildTimer(workout));
        }
    }
}