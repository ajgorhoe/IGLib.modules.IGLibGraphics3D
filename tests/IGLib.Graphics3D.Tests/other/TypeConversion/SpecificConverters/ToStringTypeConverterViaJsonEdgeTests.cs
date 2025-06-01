using FluentAssertions;
using IGLib.Core;
using IGLib.Core.Tests;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;


/// <summary>Tests edge cases for <see cref="ToStringTypeConverterViaJson"/> converter.</summary>
public class ToStringTypeConverterViaJsonEdgeTests : TypeConverterTestsBase<ToStringTypeConverterViaJsonEdgeTests>
{

    /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
    /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
    /// <param name="output">This parameter will be provided to constructor (injected via 
    /// constructor) by the test framework. I is also stored to Console property, such that
    /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
    /// method to generate test output.</param>
    public ToStringTypeConverterViaJsonEdgeTests(ITestOutputHelper output) : base(output)
    { }



    private readonly ToStringTypeConverterViaJson toJson = new();
    private readonly FromStringTypeConverterViaJson fromJson = new();

    private class RecursiveNode
    {
        public string Name { get; set; }
        public RecursiveNode Child { get; set; }
    }

    [Fact]
    public void CircularReference_ShouldThrowWithoutReferenceHandler()
    {
        var parent = new RecursiveNode { Name = "Parent" };
        var child = new RecursiveNode { Name = "Child", Child = parent };
        parent.Child = child;

        Action act = () => JsonSerializer.Serialize(parent); // Default options
        act.Should().Throw<JsonException>().WithMessage("*cycle*");
    }

    [Fact]
    public void CircularReference_ShouldSerializeWithReferenceHandler()
    {
        var parent = new RecursiveNode { Name = "Parent" };
        var child = new RecursiveNode { Name = "Child", Child = parent };
        parent.Child = child;

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(parent, options);
        json.Should().Contain("$id").And.Contain("$ref");

        var result = JsonSerializer.Deserialize<RecursiveNode>(json, options);
        result.Name.Should().Be("Parent");
        result.Child.Name.Should().Be("Child");
        result.Child.Child.Should().BeSameAs(result); // same reference
    }

    private class ReadOnlyWithConstructor
    {
        public string Name { get; }
        public int Age { get; }

        public ReadOnlyWithConstructor(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }

    [Fact]
    public void ReadOnlyPropertiesWithConstructor_ShouldRoundTrip()
    {
        var obj = new ReadOnlyWithConstructor("Eva", 29);
        var json = JsonSerializer.Serialize(obj);
        var result = JsonSerializer.Deserialize<ReadOnlyWithConstructor>(json);

        result.Name.Should().Be("Eva");
        result.Age.Should().Be(29);
    }

    private class NonSerializable
    {
        private int hidden = 5;
        public int Visible => hidden;
    }

    [Fact]
    public void PrivateFields_ShouldNotBeSerialized()
    {
        var obj = new NonSerializable();
        var json = JsonSerializer.Serialize(obj);

        json.Should().Contain("Visible");
        json.Should().NotContain("hidden");

        var result = JsonSerializer.Deserialize<NonSerializable>(json);
        result.Visible.Should().Be(0); // default int, field was not restored
    }

    private class NullProperties
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
    }

    [Fact]
    public void NullProperties_ShouldRoundTrip()
    {
        var obj = new NullProperties();
        var json = JsonSerializer.Serialize(obj);
        var result = JsonSerializer.Deserialize<NullProperties>(json);

        result.Name.Should().BeNull();
        result.Tags.Should().BeNull();
    }

    [Fact]
    public void Deserialize_InvalidJson_ShouldFail()
    {
        string invalidJson = "{ not: valid json }";
        fromJson.TryConvertTyped<NullProperties>(invalidJson, out _).Should().BeFalse();
    }

    [Fact]
    public void Deserialize_MismatchedType_ShouldFail()
    {
        string json = JsonSerializer.Serialize(new { Name = "John", Age = "Thirty" });
        fromJson.TryConvertTyped<ReadOnlyWithConstructor>(json, out _).Should().BeFalse();
    }
}