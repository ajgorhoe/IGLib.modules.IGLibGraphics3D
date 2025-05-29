using FluentAssertions;
using System;
using Xunit;
using Xunit.Abstractions;
using IGLib.Core;

namespace IGLib.Core.Tests
{

    public class ToStringTypeConverterViaIConvertibleTests : TypeConverterTestsBase<ToStringTypeConverterViaIConvertibleTests>
    {

        /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
        /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
        /// <param name="output">This parameter will be provided to constructor (injected via 
        /// constructor) by the test framework. I is also stored to Console property, such that
        /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
        /// method to generate test output.</param>
        public ToStringTypeConverterViaIConvertibleTests(ITestOutputHelper output) : base(output)
        { }

        [Theory]
        [InlineData("123", 123)]
        [InlineData("-456", -456)]
        public void FromStringIConvertible_ShouldConvertToInt(string input, int expected)
        {
            var converter = new FromStringTypeConverterViaIConvertible();
            converter.TryConvertTyped<int>(input, out var result).Should().BeTrue();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("3.14", 3.14)]
        [InlineData("-2.71", -2.71)]
        public void FromStringIConvertible_ShouldConvertToDouble(string input, double expected)
        {
            var converter = new FromStringTypeConverterViaIConvertible();
            converter.TryConvertTyped<double>(input, out var result).Should().BeTrue();
            result.Should().BeApproximately(expected, 0.00001);
        }

        [Theory]
        [InlineData(123)]
        [InlineData(3.14159)]
        [InlineData(true)]
        public void ToStringIConvertible_ShouldConvertAndBeRoundTrippable<T>(T value)
        {
            var toConverter = new ToStringTypeConverterViaIConvertible();
            var fromConverter = new FromStringTypeConverterViaIConvertible();
            toConverter.TryConvertTyped(value, out string stringValue).Should().BeTrue();
            fromConverter.TryConvertTyped<T>(stringValue, out var result).Should().BeTrue();
            result.Should().Be(value);
        }

        [Fact]
        public void ToStringIConvertible_ShouldReturnFalseForUnsupportedType()
        {
            var converter = new ToStringTypeConverterViaIConvertible();
            var unsupported = new object(); // object does not implement IConvertible

            converter.TryConvert(unsupported, out var result).Should().BeFalse();
            result.Should().BeNull();
        }

        [Fact]
        public void FromStringIConvertible_ShouldFailOnInvalidInput()
        {
            var converter = new FromStringTypeConverterViaIConvertible();
            converter.TryConvertTyped<int>("notanumber", out _).Should().BeFalse();
        }

        [Fact]
        public void RoundTripConversion_ShouldWorkForDateTime()
        {
            var now = DateTime.UtcNow;
            Console.WriteLine($"Testing round trip conversion from {nameof(DateTime)} to string and back...");
            Console.WriteLine($"\nConverted value: {now}");
            var toConverter = new ToStringTypeConverterViaIConvertible();
            var fromConverter = new FromStringTypeConverterViaIConvertible();
            bool successToString = toConverter.TryConvertTyped(now, out string str);
            Console.WriteLine($"\nConversion to string: success: {successToString}, resulting string: \"{str}\"");
            successToString.Should().BeTrue();
            bool successFromString = fromConverter.TryConvertTyped<DateTime>(str, out var result);
            Console.WriteLine($"\nConversion back to DateTimme: success: {successFromString}, restored value: {result}");
            successFromString.Should().BeTrue();
            result.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
        }

#if false  // This converter does not work for Guid because Guid does not implement the IConvertible interface.
        [Fact]
#endif
        public void RoundTripConversion_ShouldWorkForGuid()
        {
            Guid guid = Guid.NewGuid();
            Console.WriteLine($"Testing round trip conversion from {nameof(DateTime)} to string and back...");
            Console.WriteLine($"\nConverted value: {guid}");
            var toConverter = new ToStringTypeConverterViaIConvertible();
            var fromConverter = new FromStringTypeConverterViaIConvertible();

            bool successToString = toConverter.TryConvertTyped(guid, out string str);
            successToString.Should().BeTrue();
            Console.WriteLine($"\nConversion to string: success: {successToString}, resulting string: \"{str}\"");
            bool successFromString = fromConverter.TryConvertTyped<Guid>(str, out var result);
            Console.WriteLine($"\nConversion back to Guid: success: {successFromString}, restored value: {result}");
            successFromString.Should().BeTrue();

            result.Should().Be(guid);
        }

    }

}
