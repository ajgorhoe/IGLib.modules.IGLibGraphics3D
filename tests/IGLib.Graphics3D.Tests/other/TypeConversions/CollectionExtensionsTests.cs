using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using IGLib.Tests.Base;
using System.Linq;
using System.Collections;
using IGLib.Core;
using IGLib.Core.CollectionExtensions;

namespace IGLib.Core.Tests
{

    /// <summary>Tests utilities from the <see cref="CollectionExtensions"/> class.</summary>
    public class CollectionExtensionsTests : TestBase<CollectionExtensionsTests>
    {

        /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
        /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
        /// <param name="output">This parameter will be provided to constructor (injected via 
        /// constructor) by the test framework. I is also stored to Console property, such that
        /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
        /// method to generate test output.</param>
        public CollectionExtensionsTests(ITestOutputHelper output) : base(output)
        { }

        #region ToReadableString_ForCollections_Tests

        [Fact]
        protected void CollectionExtensoins_ToReadableString_Array_WorksCorrectly()
        {
            Console.WriteLine("Converting an integer arry to a readable sring...");
            int[] collection = { 1, 2, 3, 4, 5 };
            string stringRepresentation = intArray.ToReadableString();
            Console.WriteLine($"Produced string:<<\n{stringRepresentation}\n>>");
            stringRepresentation.Should().NotBeNullOrEmpty();
            stringRepresentation.Should().Contain("{");
            stringRepresentation.Should().Contain("}");
            foreach (int i in collection)
            {
                stringRepresentation.Should().Contain(i.ToString());
            }
        }



        #endregion ToReadableString_ForCollections_Tests




        #region SampleCollectionValues

        // Some hard-coded definitions of array values:

        // 1. Single-dimensional array
        int[] intArray = { 1, 2, 3 };

        // 2. Two-dimensional array
        int[,] intArray_2_3 =
        {
            { 11, 12, 13 },
            { 21, 22, 23 }
        };

        // 3. Three-dimensional array
        int[,,] intArray2_3_2 =
        {
            {
                { 111, 112 },
                { 121, 122 },
                { 131, 132 }
            },
            {
                { 211, 212 },
                { 221, 222 },
                { 231, 232 }
            }
        };

        // 4. Jagged array
        int[][] intJaggedArray_2_3 =
        {
            new int[] { 11, 12, 13 },
            new int[] { 21, 22, 23 }
        };

        #endregion SampleCollectionValues



    }

}


