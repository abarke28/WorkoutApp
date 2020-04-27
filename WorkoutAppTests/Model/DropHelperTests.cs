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
        public void ReorderItemInNotFullStructure_WhenTargetIsClosestToSource_ProperlyInserts()
        {
            var collection = new ObservableCollection<String> { "ABC", "DEF", "GHI", null };

            DropHelper.ReorderItemInNotFullStructure<String>("DEF", collection, 0, 1);

            Assert.Equal("DEF", collection[0]);
            Assert.Equal("ABC", collection[1]);
            Assert.Equal("GHI", collection[2]);
        }

        [Fact]
        public void ReorderItemInNotFullStructure_WhenEmptyIndexIsClosestToSource_ProperlyInserts()
        {
            var collection = new ObservableCollection<String> { "ABC", null, "DEF", "GHI" };

            DropHelper.ReorderItemInNotFullStructure<String>("ABC", collection, 2, 0);

            Assert.Equal("GHI", collection[0]);
            Assert.Null(collection[1]);
            Assert.Equal("ABC", collection[2]);
            Assert.Equal("DEF", collection[3]);
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