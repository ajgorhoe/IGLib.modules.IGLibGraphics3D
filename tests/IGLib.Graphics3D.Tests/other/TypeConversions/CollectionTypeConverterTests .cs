// Uncomment the definition below in order to include tests that fail for BasicTypeConverter!
# define IncludeFailedTests

using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IGLib.Tests.Base.SampleClasses;
using static IGLib.Tests.Base.SampleCollsctions.SampleCollections;

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


        //#region SampleArrayValues

        //// Some hard-coded definitions of array values:

        //// 1. Single-dimensional array
        //int[] intArray = { 1, 2, 3 };

        //// 2. Two-dimensional array
        //int[,] intArray_2_3 =
        //{
        //    { 11, 12, 13 },
        //    { 21, 22, 23 }
        //};

        //// 3. Three-dimensional array
        //int[,,] intArray2_3_2 =
        //{
        //    {
        //        { 111, 112 },
        //        { 121, 122 },
        //        { 131, 132 }
        //    },
        //    {
        //        { 211, 212 },
        //        { 221, 222 },
        //        { 231, 232 }
        //    }
        //};

        //// 4. Jagged array
        //int[][] intJaggedArray_2_3 =
        //{
        //    new int[] { 11, 12, 13 },
        //    new int[] { 21, 22, 23 }
        //};

        //#endregion SampleArrayValues


        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_IntIListToArrayObjectToIEnumerable()
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



        #region CollectionConversions.SameElementTypes




        #endregion CollectionConversions.SameElementTypes





        #region CollectionConversions.DifferentElementTypes



        #endregion CollectionConversions.DifferentElementTypes


    }



}


