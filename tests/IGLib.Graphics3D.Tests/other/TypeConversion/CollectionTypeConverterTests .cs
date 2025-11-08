
#nullable disable

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
using IGLib.Core;
using IGLib.Core.CollectionExtensions;

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
        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntIListToIntIEnumerableObjectToIntArray()
        {
            IList<int> originalObject = new CustomList<int>(1, 2, 3);
            IEnumerable<int> expectedAssignedObject = new CustomEnumerable<int>(originalObject);
            int[] expectedRestoredValue = originalObject.ToArray();
            TypeConverter_ConversionToObjectAndBackTest<
                IList<int>, IEnumerable<int>, int[] > (
                TypeConverter, originalObject, expectedAssignedObject, expectedRestoredValue);
        }

        /// <remarks>Currently, conversions to IEnumerable{T} are not possible. Instead, conversions to List{T} can be used.</remarks>
        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArrayToIntIEnumerableObjectToIntIList()
        {
            int[] originalObject = { 1, 2, 3 }; // new CustomList<int>(1, 2, 3);
            IEnumerable<int> expectedAssignedObject = new CustomEnumerable<int>(originalObject);
            IList<int> expectedRestoredValue = new CustomList<int>(originalObject);
            TypeConverter_ConversionToObjectAndBackTest<
                int[], IEnumerable<int>, IList<int>>(
                TypeConverter, originalObject, expectedAssignedObject, expectedRestoredValue);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntIListToIntArrayObjectToIntIList()
        {
            int[] values = { 1, 2, 3, 4, 5 };
            IList<int> original = new CustomList<int>(values);
            int[] expectedConvertedObject = original.ToArray();
            IList<int> expectedRestoredValue = new List<int>(values);

            var result = TypeConverter_ConversionToObjectAndBackTest(
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
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntIEnumerableToIntListObjectToIntArray()
        {
            IEnumerable<int> original = new CustomEnumerable<int>{ 1, 2, 3, 4 };
            List<int> expectedConvertedObject = original.ToList();
            int[] expectedRestoredValue = original.ToArray();

            var result = TypeConverter_ConversionToObjectAndBackTest(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            List<int> converted = result.Converted;
            int[] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimension of original:  {original?.ToList().Count}");
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

        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntIEnumerableToIntArrayObjectToIntList()
        {
            IEnumerable<int> original = new CustomEnumerable<int>{ 1, 2, 3, 4 };
            int[] expectedConvertedObject =  original.ToArray();
            List<int> expectedRestoredValue = original.ToList();

            var result = TypeConverter_ConversionToObjectAndBackTest<
                IEnumerable<int>, int[], List<int>>(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            int[] converted = result.Converted;
            List<int> restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimension of original:  {original?.ToList().Count}");
            Console.WriteLine($"  Dimension of converted: {converted?.Length}");
            Console.WriteLine($"  Dimension of restored:  {restored?.Count}");
            converted.Length.Should().Be(expectedConvertedObject.Length);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < expectedConvertedObject.Length; i++)
            {
                converted[i].Should().Be(expectedConvertedObject[i]);
            }
            Console.WriteLine("Checking the restored object's elements...");
            restored.Count.Should().Be(expectedRestoredValue.Count);
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

            var result = TypeConverter_ConversionToObjectAndBackTest(
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
        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArrayToIntIListObjectToIntArray()
        {
            int[] original = { 1, 2, 3, 4, 5, 6, 7, 8 };
            IList<int> expectedConvertedObject = original.ToList();
            int[] expectedRestoredValue = original;

            var result = TypeConverter_ConversionToObjectAndBackTest(
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


        #region CollectionConversions.SameElementTypes.RectangularArrayTo1dOrRectangularArray
        // // Cnversions of rectangular arrays (T[, ... ,]) to 1D arrays (Ttarget[]), Lists (List<Ttarget>, IList<Ttarget]), enumerables (IEnumerable[Ttarget]):

        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArray2DToIntArrayObjectToIntList()
        {
            int[,] original = IntArray2x3;
            int[] expectedConvertedObject = null;
            List<int> expectedRestoredValue = null;
            expectedRestoredValue = new List<int>();
            foreach(int element in original)
            {
                expectedRestoredValue.Add(element);
            }
            expectedConvertedObject = expectedRestoredValue.ToArray();

            var result = TypeConverter_ConversionToObjectAndBackTest(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            int[] converted = result.Converted;
            List<int> restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimensions of original:  {original.GetLength(0)} x {original.GetLength(1)}");
            Console.WriteLine($"  Dimensions of converted: {converted.Length}");
            Console.WriteLine($"  Dimensions of restored:  {restored.Count}");
            converted.Length.Should().Be(expectedConvertedObject.Length);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < original.GetLength(0); i++)
            {
                converted[i].Should().Be(expectedConvertedObject[i]);
            }
            Console.WriteLine("Checking the restored object's elements...");
            restored.Count.Should().Be(expectedRestoredValue.Count);
            for (int i = 0; i < expectedRestoredValue.Count; i++)
            {
                restored[i].Should().Be(expectedRestoredValue[i]);
            }
        }

        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArray3DToIntIListObjectToIntArray()
        {
            int[,,] original = IntArray3x2x4;
            IList<int> expectedConvertedObject = null;
            int[] expectedRestoredValue = null;
            expectedConvertedObject = new CustomList<int>();
            foreach (int element in original)
            {
                expectedConvertedObject.Add(element);
            }
            expectedRestoredValue = expectedConvertedObject.ToArray();

            var result = TypeConverter_ConversionToObjectAndBackTest(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            IList<int> converted = result.Converted;
            int[] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimensions of original:  {original.GetLength(0)} x {original.GetLength(1)}");
            Console.WriteLine($"  Dimensions of converted: {converted.Count}");
            Console.WriteLine($"  Dimensions of restored:  {restored.Length}");
            converted.Count.Should().Be(expectedConvertedObject.Count);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < original.GetLength(0); i++)
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

        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArray3DToIntListObjectToIntArray()
        {
            int[,,] original = IntArray3x2x4;
            List<int> expectedConvertedObject = null;
            int[] expectedRestoredValue = null;
            expectedConvertedObject = new List<int>();
            foreach (int element in original)
            {
                expectedConvertedObject.Add(element);
            }
            expectedRestoredValue = expectedConvertedObject.ToArray();

            var result = TypeConverter_ConversionToObjectAndBackTest(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            List<int> converted = result.Converted;
            int[] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimensions of original:  {original.GetLength(0)} x {original.GetLength(1)}");
            Console.WriteLine($"  Dimensions of converted: {converted.Count}");
            Console.WriteLine($"  Dimensions of restored:  {restored.Length}");
            converted.Count.Should().Be(expectedConvertedObject.Count);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < original.GetLength(0); i++)
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

        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArray3DToIntIEnumerableObjectToIntArray()
        {
            int[,,] original = IntArray3x2x4;
            IEnumerable<int> expectedConvertedObject = null;
            int[] expectedRestoredValue = null;
            IList<int> elements = new List<int>();
            foreach (int element in original)
            {
                elements.Add(element);
            }
            expectedConvertedObject = new CustomEnumerable<int>(elements);
            expectedRestoredValue = elements.ToArray();

            var result = TypeConverter_ConversionToObjectAndBackTest(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            IEnumerable<int> converted = result.Converted;
            // Conversion of IEnumerable to List will make comparisons easier (the 2 ines below)
            IList<int> convertedAsList = converted.ToList();
            IList<int> expectedConvertedObjectAsList = expectedConvertedObject.ToList();
            int[] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimensions of original:  {original.GetLength(0)} x {original.GetLength(1)}");
            Console.WriteLine($"  Dimensions of converted: {convertedAsList.Count}");
            Console.WriteLine($"  Dimensions of restored:  {restored.Length}");
            converted.Count().Should().Be(expectedConvertedObject.Count());
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < original.GetLength(0); i++)
            {
                convertedAsList[i].Should().Be(expectedConvertedObjectAsList[i]);
            }
            Console.WriteLine("Checking the restored object's elements...");
            restored.Length.Should().Be(expectedRestoredValue.Length);
            for (int i = 0; i < expectedRestoredValue.Length; i++)
            {
                restored[i].Should().Be(expectedRestoredValue[i]);
            }
        }

        // Rectangular Array to rectangular Array:

        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArray2DToIntArray2DObjectToIntArray2D()
        {
            int[,] original = IntArray2x3;
            int[,] expectedConvertedObject = original;
            int[,] expectedRestoredValue = original;

            var result = TypeConverter_ConversionToObjectAndBackTest(
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
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArray3DToIntArray3DObjectToIntArray3D()
        {
            int[,,] original = IntArray3x2x4;
            int[,,] expectedConvertedObject = original;
            int[,,] expectedRestoredValue = original;

            var result = TypeConverter_ConversionToObjectAndBackTest(
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

        #endregion CollectionConversions.SameElementTypes.RectangularArrayTo1dOrRectangularArray


        #region CollectionConversions.SameElementTypes.JaggedArrayTo1dOrJaggedArray
        // Cnversions of jagged arrays (T[][]...) to 1D arrays (Ttarget[]), Lists (List<Ttarget>, IList<Ttarget]), enumerables (IEnumerable[Ttarget]):


        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntJaggedArrayNonrectangular2DToIntArrayObjectToIntList()
        {
            int[][] original = IntJaggedArrayNonrectangular2x3;
            int[] expectedConvertedObject = null;
            List<int> expectedRestoredValue = null;

            List<int> elements = new List<int>();
            foreach (int[] subArray1 in original)
            {
                foreach (int element in subArray1)
                {
                    elements.Add(element);
                }
            }
            expectedConvertedObject = elements.ToArray();
            expectedRestoredValue = elements.ToList();

            var result = TypeConverter_ConversionToObjectAndBackTest(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            int[] converted = result.Converted;
            List<int> restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimensions of original:  {original.Length} x {original[0].Length}, NonRectangular!");
            Console.WriteLine($"  Dimensions of converted: {converted.Length}");
            Console.WriteLine($"  Dimensions of restored:  {restored.Count}");
            Console.WriteLine($"    expected dimensions:   {elements.Count}");
            converted.Length.Should().Be(elements.Count);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < elements.Count; i++)
            {
                converted[i].Should().Be(expectedConvertedObject[i]);
            }
            Console.WriteLine("Checking the restored object's elements...");
            restored.Count.Should().Be(elements.Count);
            for (int i = 0; i < elements.Count; i++)
            {
                restored[i].Should().Be(expectedRestoredValue[i]);
            }
        }

        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntJaggedArrayNonrectangular3DToIntIListObjectToIntArray()
        {
            int[][][] original = IntJaggedArrayNonrectangular3x2x4;
            IList<int> expectedConvertedObject = null;
            int[] expectedRestoredValue = null;
            expectedConvertedObject = new List<int>();
            List<int> elements = new List<int>();
            foreach (int[][] subArray1 in original)
            {
                foreach (int[] subArray2 in subArray1)
                {
                    foreach (int element in subArray2)
                    {
                        elements.Add(element);
                    }
                }
            }
            expectedConvertedObject = elements;
            expectedRestoredValue = expectedConvertedObject.ToArray();

            var result = TypeConverter_ConversionToObjectAndBackTest(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            IList<int> converted = result.Converted;
            int[] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimensions of original:  {original.Length} x {original[0].Length} x {original[0][0].Length}, NonRectangular!");
            Console.WriteLine($"  Dimensions of converted: {converted.Count}");
            Console.WriteLine($"  Dimensions of restored:  {restored.Length}");
            Console.WriteLine($"    expected dimensions:   {elements.Count}");
            converted.Count.Should().Be(elements.Count);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < elements.Count; i++)
            {
                converted[i].Should().Be(expectedConvertedObject[i]);
            }
            Console.WriteLine("Checking the restored object's elements...");
            restored.Length.Should().Be(elements.Count);
            for (int i = 0; i < elements.Count; i++)
            {
                restored[i].Should().Be(expectedRestoredValue[i]);
            }
        }

        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntJaggedArrayNonrectangular3DToIntIEnumerableObjectToIntArray()
        {
            int[][][] original = IntJaggedArrayNonrectangular3x2x4;
            IEnumerable<int> expectedConvertedObject = null;
            int[] expectedRestoredValue = null;
            List<int> elements = new List<int>();
            foreach (int[][] subArray1 in original)
            {
                foreach (int[] subArray2 in subArray1)
                {
                    foreach (int element in subArray2)
                    {
                        elements.Add(element);
                    }
                }
            }
            expectedConvertedObject = new CustomEnumerable<int>(elements);
            expectedRestoredValue = elements.ToArray();

            var result = TypeConverter_ConversionToObjectAndBackTest(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            IEnumerable<int> converted = result.Converted;
            // Conversion of IEnumerable to List will make comparisons easier (the 2 ines below)
            IList<int> convertedAsList = converted.ToList();
            IList<int> expectedConvertedObjectAsList = expectedConvertedObject.ToList();
            int[] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimensions of original: {original.Length} x {original[0].Length} x {original[0][0].Length}, NonRectangular!");
            Console.WriteLine($"  Dimension of converted: {convertedAsList.Count}");
            Console.WriteLine($"  Dimension of restored:  {restored.Length}");
            Console.WriteLine($"    expected dimensions:  {elements.Count}");
            converted.Count().Should().Be(elements.Count);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < elements.Count; i++)
            {
                convertedAsList[i].Should().Be(expectedConvertedObjectAsList[i]);
            }
            Console.WriteLine("Checking the restored object's elements...");
            restored.Length.Should().Be(elements.Count);
            for (int i = 0; i < elements.Count; i++)
            {
                restored[i].Should().Be(expectedRestoredValue[i]);
            }
        }

        // Jagged array to jagged array:

        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntJaggedArray2DToIntJaggedArray2DObjectToIntJaggedArray2D()
        {
            int[][] original = IntJaggedArray2x3;
            int[][] expectedConvertedObject = original;
            int[][] expectedRestoredValue = original;

            var result = TypeConverter_ConversionToObjectAndBackTest(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            int[][] converted = result.Converted;
            int[][] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimensions of original:  {original.Length} x {original[0].Length}");
            Console.WriteLine($"  Dimensions of converted: {converted.Length} x {converted[0].Length}");
            Console.WriteLine($"  Dimensions of restored:  {restored.Length} x {restored[0].Length}");
            converted.Length.Should().Be(expectedConvertedObject.Length);
            converted[0].Length.Should().Be(expectedConvertedObject[0].Length);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < expectedConvertedObject.Length; i++)
            {
                // dimension check first:
                converted[i].Length.Should().Be(expectedConvertedObject[i].Length);
                // Check the next level:
                for (int j = 0; j < expectedConvertedObject[i].Length; j++)
                    converted[i][j].Should().Be(expectedConvertedObject[i][j]);
            }
            Console.WriteLine("Checking the restored object's elements...");
            restored.GetLength(0).Should().Be(expectedRestoredValue.GetLength(0));
            restored[0].Length.Should().Be(expectedRestoredValue[0].Length);
            for (int i = 0; i < expectedRestoredValue.Length; i++)
            {
                // dimension check first:
                restored[i].Length.Should().Be(expectedRestoredValue[i].Length);
                // Check the next level:
                for (int j = 0; j < expectedRestoredValue[i].Length; j++)
                    restored[i][j].Should().Be(expectedRestoredValue[i][j]);
            }
        }

        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntJaggedArray3DToIntJaggedArray3DObjectToIntJaggedArray3D()
        {
            int[][][] original = IntJaggedArray3x2x4;
            int[][][] expectedConvertedObject = original;
            int[][][] expectedRestoredValue = original;

            var result = TypeConverter_ConversionToObjectAndBackTest(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue);
            int[][][] converted = result.Converted;
            int[][][] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimensions of original:  {original.Length} x {original[0].Length} x {original[0][0].Length}");
            Console.WriteLine($"  Dimensions of converted: {converted.Length} x {converted[0].Length} x {converted[0][0].Length}");
            Console.WriteLine($"  Dimensions of restored:  {restored.Length} x {restored[0].Length} x {restored[0][0].Length}");
            converted.Length.Should().Be(expectedConvertedObject.Length);
            converted[0].Length.Should().Be(expectedConvertedObject[0].Length);
            converted[0][0].Length.Should().Be(expectedConvertedObject[0][0].Length);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < expectedConvertedObject.Length; i++)
            {
                // dimension check first:
                converted[i].Length.Should().Be(expectedConvertedObject[i].Length);
                // check the next level:
                for (int j = 0; j < expectedConvertedObject[i].Length; j++)
                {
                    // dimension check first:
                    converted[i][j].Length.Should().Be(expectedConvertedObject[i][j].Length);
                    // check the next level:
                    for (int k = 0; k < expectedConvertedObject[i][j].Length; k++)
                    {
                    converted[i][j][k].Should().Be(expectedConvertedObject[i][j][k]);
                    }
                }
            }
            Console.WriteLine("Checking the restored object's elements...");
            restored.Length.Should().Be(expectedRestoredValue.Length);
            restored[0].Length.Should().Be(expectedRestoredValue[0].Length);
            restored[0][0].Length.Should().Be(expectedRestoredValue[0][0].Length);
            for (int i = 0; i < expectedRestoredValue.Length; i++)
            {
                // dimension check first:
                restored[i].Length.Should().Be(expectedRestoredValue[i].Length);
                // check the next level:
                for (int j = 0; j < expectedRestoredValue[i].Length; j++)
                {
                    // dimension check first:
                    restored[i][j].Length.Should().Be(expectedRestoredValue[i][j].Length);
                    // check the next level:
                    for (int k = 0; k < expectedRestoredValue[i][j].Length; k++)
                    {
                        restored[i][j][k].Should().Be(expectedRestoredValue[i][j][k]);
                    }
                }
            }
        }


        #endregion CollectionConversions.SameElementTypes.JaggedArrayTo1dOrJaggedArray



        #region CollectionConversions.SameElementTypes.JaggedArrayToRectangularArrayOrOpposite
        // Conversiion of jagged arrays (T[][]...) to rectangular arrays (Ttarget[, ... ,]), and vice-versa:


        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntJaggedArray3DToIntArray3DObjectToIntJaggedArray3D()
        {
            bool restoreObjectBackToValue = true;
            int[][][] original = IntJaggedArray3x2x4;
            int[,,] expectedConvertedObject = null;
            int[][][] expectedRestoredValue = original;

            int dim1 = original.Length;
            int dim2 = original[0].Length;
            int dim3 = original[0][0].Length;
            expectedConvertedObject = new int[3, 2, 4];
            for (int i = 0; i < original.Length; ++i)
            {
                int[][] subArray1 = original[i];
                for (int j = 0; j < subArray1.Length; ++j)
                {
                    int[] subArray2 = subArray1[j];
                    for (int k = 0; k < subArray2.Length; ++k)
                    {
                        expectedConvertedObject[i, j, k] = subArray2[k];
                    }
                }
            }

            var result = TypeConverter_ConversionToObjectAndBackTest(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue, restoreObjectBackToValue: restoreObjectBackToValue);
            int[,,] converted = result.Converted;
            int[][][] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimensions of original:  {original.Length} x {original[0].Length} x {original[0][0].Length}");
            Console.WriteLine($"  Dimensions of converted: {converted.GetLength(0)} x {converted.GetLength(1)} x {converted.GetLength(2)}");
            if (restoreObjectBackToValue)
            {
                Console.WriteLine($"  Dimensions of restored:  {restored.Length} x {restored[0].Length} x {restored[0][0].Length}");
            }
            converted.Length.Should().Be(expectedConvertedObject.Length);
            converted.GetLength(0).Should().Be(dim1);
            converted.GetLength(1).Should().Be(dim2);
            converted.GetLength(2).Should().Be(dim3);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < dim1; ++i)
            {
                for (int j = 0; j < dim2; ++j)
                {
                    for (int k = 0; k < dim3; ++k)
                    {
                        converted[i, j, k].Should().Be(expectedConvertedObject[i, j, k]);
                    }
                }
            }
            if (restoreObjectBackToValue)
            {
                Console.WriteLine("Checking the restored object's dimenions and elements...");
                restored.Length.Should().Be(dim1);
                restored[0].Length.Should().Be(dim2);
                restored[0][0].Length.Should().Be(dim3);
                for (int i = 0; i < dim1; ++i)
                {
                    // Check dimension first:
                    restored[i].Length.Should().Be(dim2);
                    // Check the next level:
                    for (int j = 0; j < dim2; ++j)
                    {
                        // Check dimension first:
                        restored[i][j].Length.Should().Be(dim3);
                        // Check the next level:
                        for (int k = 0; k < dim3; ++k)
                        {
                            restored[i][j][k].Should().Be(expectedRestoredValue[i][j][k]);
                        }
                    }
                }
            }
        }


        [Fact]
        protected virtual void SpecificTypeConverter_CollectionRoundTripConversion_IntArray3DToIntJaggedArray3DObjectToIntArray3D()
        {
            bool restoreObjectBackToValue = true;
            int[,,] original = IntArray3x2x4;
            int[][][] expectedConvertedObject = null;
            int[,,] expectedRestoredValue = original;

            int dim1 = original.GetLength(0);
            int dim2 = original.GetLength(1);
            int dim3 = original.GetLength(2);
            expectedConvertedObject = new int[dim1][][];
            for (int i = 0; i < dim1; ++i)
            {
                expectedConvertedObject[i] = new int[dim1][];
                for (int j = 0; j < dim2; ++j)
                {
                    expectedConvertedObject[i][j] = new int[dim3];
                    for (int k = 0; k < dim3; ++k)
                    {
                        expectedConvertedObject[i][j][k] = original[i, j, k];
                    }
                }
            }

            var result = TypeConverter_ConversionToObjectAndBackTest(
                TypeConverter, original, expectedConvertedObject, expectedRestoredValue,  restoreObjectBackToValue: restoreObjectBackToValue);
            int[][][] converted = result.Converted;
            int[,,] restored = result.Restored;
            // Assert (additional to asserts in the generic test mwthod called above):
            Console.WriteLine("\nOutside the generic test function...");
            Console.WriteLine("Checking dimensions...");
            Console.WriteLine($"  Dimensions of original:  {original.GetLength(0)} x {original.GetLength(1)} x {original.GetLength(2)}");
            Console.WriteLine($"  Dimensions of converted: {converted.Length} x {converted[0].Length} x {converted[0][0].Length}");
            if (restoreObjectBackToValue)
            {
                Console.WriteLine($"  Dimensions of restored:  {restored.GetLength(0)} x {restored.GetLength(1)} x {restored.GetLength(2)}");
            }
            converted.Length.Should().Be(dim1);
            converted[0].Length.Should().Be(dim2);
            converted[0][0].Length.Should().Be(dim3);
            Console.WriteLine("Checking the converted object's elements...");
            for (int i = 0; i < dim1; ++i)
            {
                // Check dimension first:
                converted[i].Length.Should().Be(dim2);
                // Check the next level:
                for (int j = 0; j < dim2; ++j)
                {
                    // Check dimension first:
                    converted[i][j].Length.Should().Be(dim3);
                    // Check the next level:
                    for (int k = 0; k < dim3; ++k)
                    {
                        converted[i][j][k].Should().Be(original[i, j, k]);
                    }
                }
            }
            if (restoreObjectBackToValue)
            {
                Console.WriteLine("Checking the restored object's dimenions and elements...");
                restored.GetLength(0).Should().Be(dim1);
                restored.GetLength(1).Should().Be(dim2);
                restored.GetLength(2).Should().Be(dim3);
                for (int i = 0; i < dim1; ++i)
                {
                    for (int j = 0; j < dim2; ++j)
                    {
                        for (int k = 0; k < dim3; ++k)
                        {
                            restored[i,j,k].Should().Be(original[i, j, k]);
                        }
                    }
                } 
            }
        }


        #endregion CollectionConversions.SameElementTypes.JaggedArrayToRectangularArrayOrOpposite

        #endregion CollectionConversions.SameElementTypes







    }


}


