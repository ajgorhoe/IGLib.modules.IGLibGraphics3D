using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using IGLib.Core;

/// <summary>
/// Sample class used for round-trip serialization tests.
/// </summary>
public class MyClass
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class TypeConverterJsonTests
{
    [Theory]
    [InlineData(123)]
    [InlineData(3.14159)]
    [InlineData(true)]
    [InlineData("Hello, JSON!")]
    public void ToStringJson_ShouldConvertAndBeRoundTrippable<T>(T value)
    {
        var toConverter = new ToStringTypeConverterViaJson();
        var fromConverter = new FromStringTypeConverterViaJson();

        toConverter.TryConvertTyped(value, out string json).Should().BeTrue();
        fromConverter.TryConvertTyped<T>(json, out var result).Should().BeTrue();
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
            new MyClass { Id = 1, Name = "Alice" },
            new MyClass { Id = 2, Name = "Bob" }
        };

        var toConverter = new ToStringTypeConverterViaJson();
        var fromConverter = new FromStringTypeConverterViaJson();

        toConverter.TryConvertTyped(list, out string json).Should().BeTrue();
        fromConverter.TryConvertTyped<List<MyClass>>(json, out var result).Should().BeTrue();

        result.Should().BeEquivalentTo(list);
    }
}