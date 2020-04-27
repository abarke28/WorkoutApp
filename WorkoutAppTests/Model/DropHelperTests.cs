using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WorkoutApp.Model;
using Xunit;

namespace WorkoutAppTests.Model
{
    public class DropHelperTests
    {
        [Fact]
        public void InsertItemIntoNotFullStructure_WhenCalled_ProperlyInserts()
        {
            var collection = new ObservableCollection<String> {"ABC", "DEF", null, "GHI"};

            DropHelper.InsertItemIntoNotFullStructure<String>("JKL", collection, 0);

            Assert.Equal("JKL", collection[0]);
            Assert.Equal("ABC", collection[1]);
            Assert.Equal("DEF", collection[2]);
        }

        [Fact]
        public void ReorderItemInNotFullStructure_WhenCalled_ProperlyInserts()
        {
            var collection = new ObservableCollection<String> { "ABC", "DEF", "GHI", null };

            DropHelper.ReorderItemInNotFullStructure<String>("DEF", collection, 0, 1);

            Assert.Equal("DEF", collection[0]);
            Assert.Equal("ABC", collection[1]);
            Assert.Equal("GHI", collection[2]);
        }

        [Fact]
        public void ReorderItemInFullStructure_WhenCalled_ProperlyInserts()
        {
            var collection = new ObservableCollection<String> { "ABC", "DEF", "GHI", "JKL" };

            DropHelper.ReorderItemInFullStructure<String>("ABC", collection, 3, 0);

            Assert.Equal("JKL", collection[0]);
            Assert.Equal("DEF", collection[1]);
            Assert.Equal("GHI", collection[2]);
        }
    }
}
