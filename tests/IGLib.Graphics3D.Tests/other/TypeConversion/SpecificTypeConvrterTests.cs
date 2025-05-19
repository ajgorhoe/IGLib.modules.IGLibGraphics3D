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

namespace IGLib.Core.Tests
{

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

        [Fact]
        protected virtual void ParseHelperTest()
        {
            string parsedString = "1976";
            Console.WriteLine($"Testing ParseHelper: trying to parse string \"{parsedString}\" into type {typeof(int).Name}:");
            bool successful = ParseHelper.TryParse<int>(parsedString, out int number);
            if (successful)
            {
                Console.WriteLine($"Parsing was successful. Result: type = {number.GetType().Name}, value: {number}");
            }
            successful.Should().Be(true);
            number.ToString().Should().Be(parsedString);
        }



        #region Examples


        #endregion Examples


    }

}


