
#nullable disable

// Uncomment the definition below in order to include tests that fail at this hierarchy level by design!
// #define IncludeFailedTestsByDesign

using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using IGLib.Tests.Base;
using System.Linq;
using IGLib.Core;
using IGLib.Core.Extended;
using IGLib.Tests.Base.SampleClasses;
using IGLib.Core.CollectionExtensions;


namespace IGLib.Core.Tests
{

    using static CapturedVar;


    /// <summary>Sandbox for quick tests related to type conversion.</summary>
    public class SpecificTypeConvrterTests : TypeConverterTestsBase<SandboxTypeconversionTests>
    {

        /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
        /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
        /// <param name="output">This parameter will be provided to constructor (injected via 
        /// constructor) by the test framework. I is also stored to Console property, such that
        /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
        /// method to generate test output.</param>
        public SpecificTypeConvrterTests(ITestOutputHelper output) : base(output)
        {  }


        #region SpecificTypeConverterTests

        [Fact]
        public void SingleTypeCnverter_WhenTypesMatch_ThenConvertWorksCorrectly()
        {
            // Arrange:
            ISingleTypeConverter<int[], string> converterTyped = new IntArrayToStringConverter_Testing();
            ISingleTypeConverter converter = converterTyped;
            Console.WriteLine($"Testing Convert method of {converter.GetType().ToString()} for matching types:");
            int[] intArray = new int[] { 1, 2, 3, 4, 5 };
            // Act:
            converter.Should().Be(converterTyped, because: "PRECOND: assignment of typed converter to untyped works.");
            string result = (string)converterTyped.Convert(intArray);
            // Assert:
            result.Should().NotBeNull(because: "A valid result must be produced.");

        }



        #endregion SpecificTypeConverterTests


    }


    #region SamplClassesForTests


    /// <summary>Sample implementation of <see cref="ISingleTypeConverter{SourceType, TargetType}"/>
    /// for testing.</summary>
    public class StringToIntArrayConverter_Testing : SingleTypeConverterBase<string, int[]>,
        ISingleTypeConverter<string, int[]>, ISingleTypeConverter
    {
        public override int[] ConvertTyped(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException($"Cannot convert a null or whitespace string to an integer array.", nameof(source));
            }
            source = source.Trim();
            source = source.TrimStart('[').TrimEnd(']');
            source = source.Trim();
            string[] numStrings = source.Split(",");
            return numStrings.Select((str) => int.Parse(str)).ToArray();
        }
    }

    /// <summary>Sample implementation of <see cref="ISingleTypeConverter{SourceType, TargetType}"/>
    /// for testing.</summary>
    public class IntArrayToStringConverter_Testing : SingleTypeConverterBase<int[], string>,
        ISingleTypeConverter<int[], string>, ISingleTypeConverter
    {
        public override string ConvertTyped(int[] source)
        {
            return "[" + string.Join(", ", source) + "]";
        }
    }



    #endregion SampleClassesForTests


}


