
#nullable disable

using FluentAssertions;
using IGLib.Core;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;


namespace IGLib.Core.Tests
{

    public class ToStringTypeConverterViaJsonTests : TypeConverterTestsBase<ToStringTypeConverterViaJsonTests>
    {

        /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
        /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
        /// <param name="output">This parameter will be provided to constructor (injected via 
        /// constructor) by the test framework. I is also stored to Console property, such that
        /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
        /// method to generate test output.</param>
        public ToStringTypeConverterViaJsonTests(ITestOutputHelper output) : base(output)
        { }


        [Theory]
        [InlineData(123)]
        [InlineData(3.14159)]
        [InlineData(true)]
        [InlineData("String to be \n\tconverted to \"JSON\".")]
        public void ToStringJson_ShouldConvertAndBeRoundTrippable<T>(T value)
        {
            Console.WriteLine($"Testing round-trip conversion via {nameof(ToStringTypeConverterViaJson)} and {nameof(FromStringTypeConverterViaJson)}:");
            Console.WriteLine($"\nConverted value of type {typeof(T).Name}: {value}");

            var toConverter = new ToStringTypeConverterViaJson();
            var fromConverter = new FromStringTypeConverterViaJson();

            bool successToString = toConverter.TryConvertTyped(value, out string json);
            Console.WriteLine($"\nConversion to string: success: {successToString}, resulting string: \"{json}\"");
            successToString.Should().BeTrue();
            bool successFromString = fromConverter.TryConvertTyped<T>(json, out var result);
            Console.WriteLine($"\nConversion back to original type: success: {successFromString}, restored value: {result}");
            successFromString.Should().BeTrue();
            result.Should().BeEquivalentTo(value);
        }

        [Fact]
        public void RoundTripConversion_ShouldWorkForGuid()
        {
            var guid = Guid.NewGuid();
            var toConverter = new ToStringTypeConverterViaJson();
            var fromConverter = new FromStringTypeConverterViaJson();

            toConverter.TryConvertTyped(guid, out string json).Should().BeTrue();
            fromConverter.TryConvertTyped<Guid>(json, out var result).Should().BeTrue();

            result.Should().Be(guid);
        }

        [Fact]
        public void RoundTripConversion_ShouldWorkForDoubleArray()
        {
            var input = new double[] { 1.1, 2.2, 3.3 };
            var toConverter = new ToStringTypeConverterViaJson();
            var fromConverter = new FromStringTypeConverterViaJson();

            toConverter.TryConvertTyped(input, out string json).Should().BeTrue();
            fromConverter.TryConvertTyped<double[]>(json, out var result).Should().BeTrue();

            result.Should().BeEquivalentTo(input);
        }

        [Fact]
        public void RoundTripConversion_ShouldWorkForListOfDouble()
        {
            var input = new List<double> { 1.1, 2.2, 3.3 };
            var toConverter = new ToStringTypeConverterViaJson();
            var fromConverter = new FromStringTypeConverterViaJson();

            toConverter.TryConvertTyped(input, out string json).Should().BeTrue();
            fromConverter.TryConvertTyped<List<double>>(json, out var result).Should().BeTrue();

            result.Should().BeEquivalentTo(input);
        }

        [Fact]
        public void RoundTripConversion_ShouldWorkForMyClass()
        {
            var obj = new MyClass { Id = 1, Name = "Test" };
            var toConverter = new ToStringTypeConverterViaJson();
            var fromConverter = new FromStringTypeConverterViaJson();

            toConverter.TryConvertTyped(obj, out string json).Should().BeTrue();
            fromConverter.TryConvertTyped<MyClass>(json, out var result).Should().BeTrue();

            result.Should().BeEquivalentTo(obj);
        }

        [Fact]
        public void RoundTripConversion_ShouldWorkForArrayOfMyClass()
        {
            var array = new MyClass[]
            {
            new MyClass { Id = 1, Name = "Alice" },
            new MyClass { Id = 2, Name = "Bob" }
            };

            var toConverter = new ToStringTypeConverterViaJson();
            var fromConverter = new FromStringTypeConverterViaJson();

            toConverter.TryConvertTyped(array, out string json).Should().BeTrue();
            fromConverter.TryConvertTyped<MyClass[]>(json, out var result).Should().BeTrue();

            result.Should().BeEquivalentTo(array);
        }

        [Fact]
        public void RoundTripConversion_ShouldWorkForListOfMyClass()
        {
            var list = new List<MyClass>
        {
            new MyClass { Id = 4, Name = "Swansea" },
            new MyClass { Id = 5, Name = "Birmingham" }
        };

            var toConverter = new ToStringTypeConverterViaJson();
            var fromConverter = new FromStringTypeConverterViaJson();

            toConverter.TryConvertTyped(list, out string json).Should().BeTrue();
            fromConverter.TryConvertTyped<List<MyClass>>(json, out var result).Should().BeTrue();

            result.Should().BeEquivalentTo(list);
        }

    }


    /// <summary>
    /// Sample class used for round-trip serialization tests.
    /// </summary>
    public class MyClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


}