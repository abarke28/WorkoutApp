using GongSolutions.Wpf.DragDrop;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutApp.Model;
using WorkoutApp.ViewModel;
using Xunit;

namespace WorkoutAppTests.ViewModel
{
    public class MainVMTests
    {
        [Fact]
        public void Drop_IndexGreaterThanCount_RejectsDrop()
        {
            var vm = new MainVM();
            var dropInfo = new Mock<IDropInfo>();
            dropInfo.Setup(d => d.Data).Returns(new Exercise { Description = "abc", ExerciseName = "def", ExerciseType = ExerciseType.Core });
        }
    }
}
