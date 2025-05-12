//Uncomment the definition below in order to include tests that fail at this hierarchy level by design!
// #define IncludeFailedTestsByDesign

using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IGLib.Tests.Base.SampleClasses;
using static IGLib.Tests.Base.SampleCollsctions.SampleCollections;
using System.Runtime.ExceptionServices;

namespace IGLib.Core.Tests
{


    /// <summary>Tests of the basic type converter (<see cref="BasicTypeConverter"/>,
    /// implementation of the <see cref="ITypeConverter"/> interface).</summary>
    public class CollectionTypeConverterTests : BasicTypeConverterTests
    {

        public CollectionTypeConverterTests(ITestOutputHelper output) : base(output)
        { }


        /// <summary>The type converter that is under test.</summary>
        protected override ITypeConverter TypeConverter { get; } = new CollectionTypeConverter();






        #region CollectionConversions.SameElementTypes


        /// <remarks>Currently, conversions to IEnumerable{T} are not possible. Instead, conversions to List{T} can be used.</remarks>
#if IncludeFailedTestsByDesign
        [Fact]
#endif
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntIListToArrayObjectToIEnumerable()
        {
            CustomList<int> list = new CustomList<int>(1, 2, 3);
            CustomEnumerable<int> enumerable = new CustomEnumerable<int>(1, 2, 3);

            IList<int> originalObject = new CustomList<int>(1, 2, 3);
            int[] expectedAssignedObject = originalObject.ToArray();
            CustomEnumerable<int> expectedRestoredValue = new(originalObject);
            TypeConverter_ConversionToObjectAndBackTest<
                IList<int>, int[], CustomEnumerable<int> > (
                TypeConverter, originalObject, expectedAssignedObject, expectedRestoredValue);
        }



        /// <remarks>Currently, conversions to IEnumerable{T} are not possible. Instead, conversions to List{T} or T[] can be used.</remarks>
#if IncludeFailedTestsByDesign
        [Fact]
#endif
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntIListToIntArrayObjectToIntIList()
        {
            int[] values = { 1, 2, 3, 4, 5 };
            CustomList<int> original = new CustomList<int>(values);
            int[] expectedConvertedObject = original.ToArray();
            IList<int> expectedRestoredValue = new List<int>(values);

            var result = TypeConverter_ConversionToObjectAndBackTest<
                IList<int>, int[], IList<int> > (
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            int[] converted = result.Converted;
            IList<int> restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimension of original:  {original?.Count}");
            Console.WriteLine($"  Dimension of converted: {converted?.Length}");
            Console.WriteLine($"  Dimension of restored:  {restored?.Count}");
            converted.Length.Should().Be(expectedConvertedObject.Length);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < expectedConvertedObject.Length; i++)
            {
                converted[i].Should().Be(expectedConvertedObject[i]);
            }
            restored.Count.Should().Be(expectedRestoredValue.Count);
            Console.WriteLine("Checking the restored object's elements...");
            for (int i = 0; i < expectedRestoredValue.Count; i++)
            {
                restored[i].Should().Be(expectedRestoredValue[i]);
            }
        }




        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntIListToIntArrayObjectToIntList()
        {
            int[] values = { 1, 2, 3, 4, 5 };
            IList<int> original = new CustomList<int>(values);
            int[] expectedConvertedObject = original.ToArray();
            List<int> expectedRestoredValue = new List<int>(values);

            var result = TypeConverter_ConversionToObjectAndBackTest<
                IList<int>, int[], List<int>>(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            int[] converted = result.Converted;
            List<int> restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimension of original:  {original?.Count}");
            Console.WriteLine($"  Dimension of converted: {converted?.Length}");
            Console.WriteLine($"  Dimension of restored:  {restored?.Count}");
            converted.Length.Should().Be(expectedConvertedObject.Length);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < expectedConvertedObject.Length; i++)
            {
                converted[i].Should().Be(expectedConvertedObject[i]);
            }
            restored.Count.Should().Be(expectedRestoredValue.Count);
            Console.WriteLine("Checking the restored object's elements...");
            for (int i = 0; i < expectedRestoredValue.Count; i++)
            {
                restored[i].Should().Be(expectedRestoredValue[i]);
            }
        }



        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArrayToIntListObjectToIntArray()
        {
            int[] original = { 1, 2, 3, 4, 5, 6, 7, 8 };
            List<int> expectedConvertedObject = original.ToList();
            int[] expectedRestoredValue = original;

            var result = TypeConverter_ConversionToObjectAndBackTest<
                int[], List<int>, int[]>(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            List<int> converted = result.Converted;
            int[] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimension of original:  {original?.Length}");
            Console.WriteLine($"  Dimension of converted: {converted?.Count}");
            Console.WriteLine($"  Dimension of restored:  {restored?.Length}");
            converted.Count.Should().Be(expectedConvertedObject.Count);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < expectedConvertedObject.Count; i++)
            {
                converted[i].Should().Be(expectedConvertedObject[i]);
            }
            Console.WriteLine("Checking the restored object's elements...");
            restored.Length.Should().Be(expectedRestoredValue.Length);
            for (int i = 0; i < expectedRestoredValue.Length; i++)
            {
                restored[i].Should().Be(expectedRestoredValue[i]);
            }
        }




        /// <remarks>Currently, conversions to IEnumerable{T} are not possible. Instead, conversions to List{T} or T[] can be used.</remarks>
#if IncludeFailedTestsByDesign
        [Fact]
#endif
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArrayToIntIListObjectToIntArray()
        {
            int[] original = { 1, 2, 3, 4, 5, 6, 7, 8 };
            IList<int> expectedConvertedObject = original.ToList();
            int[] expectedRestoredValue = original;

            var result = TypeConverter_ConversionToObjectAndBackTest<
                int[], IList<int>, int[]>(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            IList<int> converted = result.Converted;
            int[] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimension of original:  {original?.Length}");
            Console.WriteLine($"  Dimension of converted: {converted?.Count}");
            Console.WriteLine($"  Dimension of restored:  {restored?.Length}");
            converted.Count.Should().Be(expectedConvertedObject.Count);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < expectedConvertedObject.Count; i++)
            {
                converted[i].Should().Be(expectedConvertedObject[i]);
            }
            Console.WriteLine("Checking the restored object's elements...");
            restored.Length.Should().Be(expectedRestoredValue.Length);
            for (int i = 0; i < original.Length; i++)
            {
                restored[i].Should().Be(expectedRestoredValue[i]);
            }
        }






        //[Fact]
        //protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArray2DToIntArrayToIntList()
        //{
        //    int[,] original = IntArray2x3;
        //    int[] expectedConvertedObject = original;
        //    int[,] expectedRestoredValue = original;

        //    var result = TypeConverter_ConversionToObjectAndBackTest<
        //        int[,], int[,], int[,]>(
        //        TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
        //    int[,] converted = result.Converted;
        //    int[,] restored = result.Restored;
        //    // Assert (additional to asserts in the generic test mwthod called above):
        //    Console.WriteLine("\nOutside the generic test function...");
        //    Console.WriteLine("Checking dimensions...");
        //    Console.WriteLine($"  Dimensions of original:  {original.GetLength(0)} x {original.GetLength(1)}");
        //    Console.WriteLine($"  Dimensions of converted: {converted.GetLength(0)} x {converted.GetLength(1)}");
        //    Console.WriteLine($"  Dimensions of restored:  {restored.GetLength(0)} x {restored.GetLength(1)}");
        //    converted.GetLength(0).Should().Be(expectedConvertedObject.GetLength(0));
        //    converted.GetLength(1).Should().Be(expectedConvertedObject.GetLength(1));
        //    Console.WriteLine("Checking the converted object's elements...");
        //    for (int i = 0; i < expectedConvertedObject.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < expectedConvertedObject.GetLength(1); j++)
        //            converted[i, j].Should().Be(expectedConvertedObject[i, j]);
        //    }
        //    Console.WriteLine("Checking the restored object's elements...");
        //    restored.GetLength(0).Should().Be(expectedRestoredValue.GetLength(0));
        //    restored.GetLength(1).Should().Be(expectedRestoredValue.GetLength(1));
        //    for (int i = 0; i < expectedRestoredValue.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < expectedRestoredValue.GetLength(1); j++)
        //            restored[i, j].Should().Be(expectedRestoredValue[i, j]);
        //    }
        //}

        //[Fact]
        //protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArray3DToIntArrayToIntList()
        //{
        //    int[,,] original = IntArray3x2x4;
        //    int[,,] expectedConvertedObject = original;
        //    int[,,] expectedRestoredValue = original;

        //    var result = TypeConverter_ConversionToObjectAndBackTest<
        //        int[,,], int[,,], int[,,]>(
        //        TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
        //    int[,,] converted = result.Converted;
        //    int[,,] restored = result.Restored;
        //    // Assert (additional to asserts in the generic test mwthod called above):
        //    Console.WriteLine("\nOutside the generic test function...");
        //    Console.WriteLine("Checking dimensions...");
        //    Console.WriteLine($"  Dimensions of original:  {original.GetLength(0)} x {original.GetLength(1)} x {original.GetLength(2)}");
        //    Console.WriteLine($"  Dimensions of converted: {converted.GetLength(0)} x {converted.GetLength(1)} x {converted.GetLength(2)}");
        //    Console.WriteLine($"  Dimensions of restored:  {restored.GetLength(0)} x {restored.GetLength(1)} x {restored.GetLength(2)}");
        //    converted.GetLength(0).Should().Be(expectedConvertedObject.GetLength(0));
        //    converted.GetLength(1).Should().Be(expectedConvertedObject.GetLength(1));
        //    converted.GetLength(2).Should().Be(expectedConvertedObject.GetLength(2));
        //    Console.WriteLine("Checking the converted object's elements...");
        //    for (int i = 0; i < expectedConvertedObject.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < expectedConvertedObject.GetLength(1); j++)
        //        {
        //            for (int k = 0; k < expectedConvertedObject.GetLength(2); k++)
        //            {
        //                converted[i, j, k].Should().Be(expectedConvertedObject[i, j, k]);
        //            }
        //        }
        //    }
        //    Console.WriteLine("Checking the restored object's elements...");
        //    restored.GetLength(0).Should().Be(expectedRestoredValue.GetLength(0));
        //    restored.GetLength(1).Should().Be(expectedRestoredValue.GetLength(1));
        //    restored.GetLength(2).Should().Be(expectedRestoredValue.GetLength(2));
        //    for (int i = 0; i < expectedRestoredValue.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < expectedRestoredValue.GetLength(1); j++)
        //        {
        //            for (int k = 0; k < expectedRestoredValue.GetLength(2); k++)
        //            {
        //                restored[i, j, k].Should().Be(expectedRestoredValue[i, j, k]);
        //            }
        //        }
        //    }
        //}







        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArray2DToIntArray2DToIntArray2D()
        {
            int[,] original = IntArray2x3;
            int[,] expectedConvertedObject = original;
            int[,] expectedRestoredValue = original;

            var result = TypeConverter_ConversionToObjectAndBackTest<
                int[,], int[,], int[,] >(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            int[,] converted = result.Converted;
            int[,] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimensions of original:  {original.GetLength(0)} x {original.GetLength(1)}");
            Console.WriteLine($"  Dimensions of converted: {converted.GetLength(0)} x {converted.GetLength(1)}");
            Console.WriteLine($"  Dimensions of restored:  {restored.GetLength(0)} x {restored.GetLength(1)}");
            converted.GetLength(0).Should().Be(expectedConvertedObject.GetLength(0));
            converted.GetLength(1).Should().Be(expectedConvertedObject.GetLength(1));
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < expectedConvertedObject.GetLength(0); i++)
            {
                for (int j = 0; j <  expectedConvertedObject.GetLength(1); j++)
                converted[i,j].Should().Be(expectedConvertedObject[i,j]);
            }
            Console.WriteLine("Checking the restored object's elements...");
            restored.GetLength(0).Should().Be(expectedRestoredValue.GetLength(0));
            restored.GetLength(1).Should().Be(expectedRestoredValue.GetLength(1));
            for (int i = 0; i < expectedRestoredValue.GetLength(0); i++)
            {
                for (int j = 0;j < expectedRestoredValue.GetLength(1); j++)
                    restored[i, j].Should().Be(expectedRestoredValue[i, j]);
            }
        }

        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArray3DToIntArray3DToIntArray3D()
        {
            int[,,] original = IntArray3x2x4;
            int[,,] expectedConvertedObject = original;
            int[,,] expectedRestoredValue = original;

            var result = TypeConverter_ConversionToObjectAndBackTest<
                int[,,], int[,,], int[,,] >(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            int[,,] converted = result.Converted;
            int[,,] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimensions of original:  {original.GetLength(0)} x {original.GetLength(1)} x {original.GetLength(2)}");
            Console.WriteLine($"  Dimensions of converted: {converted.GetLength(0)} x {converted.GetLength(1)} x {converted.GetLength(2)}");
            Console.WriteLine($"  Dimensions of restored:  {restored.GetLength(0)} x {restored.GetLength(1)} x {restored.GetLength(2)}");
            converted.GetLength(0).Should().Be(expectedConvertedObject.GetLength(0));
            converted.GetLength(1).Should().Be(expectedConvertedObject.GetLength(1));
            converted.GetLength(2).Should().Be(expectedConvertedObject.GetLength(2));
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < expectedConvertedObject.GetLength(0); i++)
            {
                for (int j = 0; j < expectedConvertedObject.GetLength(1); j++)
                {
                    for (int k = 0; k < expectedConvertedObject.GetLength(2); k++)
                    {
                        converted[i, j, k].Should().Be(expectedConvertedObject[i, j, k]);
                    }
                }
            }
            Console.WriteLine("Checking the restored object's elements...");
            restored.GetLength(0).Should().Be(expectedRestoredValue.GetLength(0));
            restored.GetLength(1).Should().Be(expectedRestoredValue.GetLength(1));
            restored.GetLength(2).Should().Be(expectedRestoredValue.GetLength(2));
            for (int i = 0; i < expectedRestoredValue.GetLength(0); i++)
            {
                for (int j = 0; j < expectedRestoredValue.GetLength(1); j++)
                {
                    for (int k = 0; k < expectedRestoredValue.GetLength(2); k++)
                    {
                        restored[i, j, k].Should().Be(expectedRestoredValue[i, j, k]);
                    }
                }
            }
        }



        #endregion CollectionConversions.SameElementTypes







        #region CollectionConversions.DifferentElementTypes



        #endregion CollectionConversions.DifferentElementTypes


    }



}


