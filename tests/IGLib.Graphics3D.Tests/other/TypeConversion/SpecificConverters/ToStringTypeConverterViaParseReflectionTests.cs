using FluentAssertions;
using IGLib.Core;
using IGLib.Core.Tests;
using System;
using Xunit;
using Xunit.Abstractions;

public class ToStringTypeConverterViaParseReflectionTests : TypeConverterTestsBase<ToStringTypeConverterViaParseReflectionTests>
{

    /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
    /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
    /// <param name="output">This parameter will be provided to constructor (injected via 
    /// constructor) by the test framework. I is also stored to Console property, such that
    /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
    /// method to generate test output.</param>
    public ToStringTypeConverterViaParseReflectionTests(ITestOutputHelper output) : base(output)
    { }

    [Theory]
    [InlineData("123", 123)]
    [InlineData("-456", -456)]
    public void FromString_ShouldConvertToInt(string input, int expected)
    {
        var converter = new FromStringTypeConverterViaParseReflection();
        converter.TryConvertTyped<int>(input, out var result).Should().BeTrue();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("3.14", 3.14)]
    [InlineData("-2.71", -2.71)]
    public void FromString_ShouldConvertToDouble(string input, double expected)
    {
        var converter = new FromStringTypeConverterViaParseReflection();
        converter.TryConvertTyped<double>(input, out var result).Should().BeTrue();
        result.Should().BeApproximately(expected, 0.00001);
    }

    [Theory]
    [InlineData(123)]
    [InlineData(3.14159)]
    [InlineData(true)]
    public void ToString_ShouldConvertAndBeRoundTrippable<T>(T value)
    {
        var toConverter = new ToStringTypeConverterViaParseReflection();
        var fromConverter = new FromStringTypeConverterViaParseReflection();

        toConverter.TryConvertTyped(value, out string stringValue).Should().BeTrue();
        fromConverter.TryConvertTyped<T>(stringValue, out var result).Should().BeTrue();
        result.Should().Be(value);
    }

    [Fact]
    public void ToString_ShouldReturnFalseForUnsupportedType()
    {
        var converter = new ToStringTypeConverterViaParseReflection();
        var unsupported = new object(); // No TryParse or Parse methods

        converter.TryConvert(unsupported, out var result).Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void FromString_ShouldFailOnInvalidInput()
    {
        var converter = new FromStringTypeConverterViaParseReflection();
        converter.TryConvertTyped<int>("notanumber", out _).Should().BeFalse();
    }

}