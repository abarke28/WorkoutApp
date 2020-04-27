using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WorkoutApp.Model
{
    public static class DropHelper
    {
        public static void InsertItemIntoNotFullStructure<T>(T item, ObservableCollection<T> collection, int targetIndex)
        {
            // Summary
            //
            // Method to drop an item in a collection if it is not full. Items will be pushed down until
            // an empty index is reached. Push is wrapped around collection end.

            // Target index is unnocupied
            if (collection[targetIndex] == null)
            {
                collection[targetIndex] = item;
                return;
            }

            // Target index is occupied
            else
            {
                // find first empty index after targetIndex
                int nextOpenIndex = 0;

                for (int i = 0; i < collection.Count; i++)
                {
                    if (collection[(i + targetIndex) % collection.Count] == null)
                    {
                        nextOpenIndex = (i + targetIndex) % collection.Count;
                        break;
                    }
                }

                // push everything down from target index to next open slot, then insert
                //
                // still want to do up to exercisesPercollection loops, but need to start at
                // nextOpenIndex, hence the odd indices. Use mod operator to prevent indexoutofrange
                for (int i = nextOpenIndex + collection.Count; i > nextOpenIndex; i--)
                {
                    if (i % collection.Count == targetIndex)
                    {
                        collection[i % collection.Count] = item;
                        return;
                    }

                    collection[i % collection.Count] = collection[(i - 1) % collection.Count];
                }
            }
        }

        public static void ReorderItemInNotFullStructure<T>(T item, ObservableCollection<T> collection, int targetIndex, int sourceIndex)
        {
            // Summary
            //
            // Method to drop an item in a collection if it is not full and the item came from the collection. 
            // Items will be pushed down until an empty index is reached. Push is wrapped around collection end.

            // Subcase 1: If target index is empty, simply move item from source to target index
            if (collection[targetIndex] == null)
            {
                collection[targetIndex] = item;
                collection[sourceIndex] = default;
                return;
            }

            // find first empty index after targetIndex
            int nextOpenIndex = 0;

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[(i + targetIndex) % collection.Count] == null)
                {
                    nextOpenIndex = (i + targetIndex) % collection.Count;
                    break;
                }
            }

            // Two Subcases: 
            // Subcase 2 - Target Index > Next Open Index > Source Index
            // Subcase 3 - Target Index > Source Index > Next Open Index

            var modNextOpenIndex = nextOpenIndex;
            var modSourceIndex = sourceIndex;

            if (nextOpenIndex < targetIndex)
                modNextOpenIndex += collection.Count;

            if (sourceIndex < targetIndex)
                modSourceIndex += collection.Count;

            // Subcase 2
            if (modNextOpenIndex < modSourceIndex)
            {
                // push everything down from target index to next open slot, then insert
                //
                // still want to do up to collection.Count loops, but need to start at
                // nextOpenIndex, hence the odd indices. Use mod operator to prevent indexoutofrange

                collection[sourceIndex] = default;

                for (int i = nextOpenIndex + collection.Count; i > nextOpenIndex; i--)
                {
                    if (i % collection.Count == targetIndex)
                    {
                        collection[i % collection.Count] = item;
                        return;
                    }

                    collection[i % collection.Count] = collection[(i - 1) % collection.Count];
                }
            }

            // Subcase 3
            else
            {
                // push everything down from target index to source index
                //
                // still want to do up to collection.Count loops, but need to start at sourceIndex,
                // hence the odd indices. Use mod operator on i to prevent indexoutrange
                for (int i = sourceIndex + collection.Count; i > sourceIndex; i--)
                {
                    if (i % collection.Count == targetIndex)
                    {
                        collection[i % collection.Count] = item;
                        return;
                    }

                    collection[i % collection.Count] = collection[(i - 1) % collection.Count];
                }
            }
        }

        public static void ReorderItemInFullStructure<T>(T item, ObservableCollection<T> collection, int targetIndex, int sourceIndex)
        {
            // Summary
            //
            // Reorder items in collection when an existing item is dropped elsewhere in the collection

            // push everything down from target index to source index
            // still want to do up to exercisesPerStation loops, but need to start at sourceIndex,
            // hence the odd indices. Use mod operator on i to prevent indexoutrange
            for (int i = sourceIndex + collection.Count; i > sourceIndex; i--)
            {
                if (i % collection.Count == targetIndex)
                {
                    collection[i % collection.Count] = item;
                    return;
                }

                collection[i % collection.Count] = collection[(i - 1) % collection.Count];
            }
        }
    }
}