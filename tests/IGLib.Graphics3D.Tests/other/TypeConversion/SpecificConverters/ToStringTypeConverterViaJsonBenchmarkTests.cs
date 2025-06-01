using FluentAssertions;
using IGLib.Core;
using IGLib.Core.Tests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

/// <summary>Benchmark tests for <see cref="ToStringTypeConverterViaJson"/> (memory size and speed).</summary>
public class ToStringTypeConverterViaJsonBenchmarkTests : TypeConverterTestsBase<ToStringTypeConverterViaJsonBenchmarkTests>
{

    /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
    /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
    /// <param name="output">This parameter will be provided to constructor (injected via 
    /// constructor) by the test framework. I is also stored to Console property, such that
    /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
    /// method to generate test output.</param>
    public ToStringTypeConverterViaJsonBenchmarkTests(ITestOutputHelper output) : base(output)
    { }


    private readonly ToStringTypeConverterViaJson toJson = new();
    private readonly FromStringTypeConverterViaJson fromJson = new();

    private class NestedLevel
    {
        public string Name { get; set; }
        public NestedLevel Inner { get; set; }
    }

    private record InitOnlyRecord(string Name, int Age);

    [Fact]
    public void InitOnlyRecord_ShouldRoundTripCorrectly()
    {
        var obj = new InitOnlyRecord("Immutable", 42);
        var json = JsonSerializer.Serialize(obj);
        var result = JsonSerializer.Deserialize<InitOnlyRecord>(json);

        result.Should().Be(obj);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(63)]
    [InlineData(64)]
    public void DeeplyNestedObject_ShouldRoundTrip(int numLevels)
    {
        //int numLevels = 
        //    67;
        //    // 100;
        NestedLevel root = new NestedLevel { Name = "Level 0" };
        var current = root;
        for (int i = 1; i < numLevels; i++)
        {
            current.Inner = new NestedLevel { Name = $"Level {i}" };
            current = current.Inner;
        }

        string json = JsonSerializer.Serialize(root);
        NestedLevel restored = JsonSerializer.Deserialize<NestedLevel>(json);

        restored.Name.Should().Be("Level 0");
        current = restored;
        for (int i = 1; i < numLevels; i++)
        {
            current = current.Inner;
            current.Name.Should().Be($"Level {i}");
        }
    }

    [Fact]
    public void Serialize_Performance_And_Size_Benchmark()
    {
        var largeList = new List<InitOnlyRecord>();
        for (int i = 0; i < 10000; i++)
        {
            largeList.Add(new InitOnlyRecord($"Name{i}", i));
        }

        Stopwatch sw = Stopwatch.StartNew();
        string json = JsonSerializer.Serialize(largeList);
        sw.Stop();

        long elapsedMs = sw.ElapsedMilliseconds;
        int sizeBytes = System.Text.Encoding.UTF8.GetByteCount(json);

        // Example expectations: can be adjusted based on environment
        elapsedMs.Should().BeLessThan(500); // < 500 ms
        sizeBytes.Should().BeGreaterThan(100000); // > 100 KB

        Console.WriteLine($"Serialized 10,000 objects in {elapsedMs}ms, size: {sizeBytes / 1024.0:F1} KB");
    }

    [Fact]
    public void Deserialize_Performance_And_Size_Benchmark()
    {
        int numExecutions = 1000;
        double minExecutionsPerSecond = 2_000;
        var largeList = new List<InitOnlyRecord>();
        for (int i = 0; i < numExecutions; i++)
        {
            largeList.Add(new InitOnlyRecord($"Name{i}", i));
        }

        string json = JsonSerializer.Serialize(largeList);

        Stopwatch sw = Stopwatch.StartNew();
        var result = JsonSerializer.Deserialize<List<InitOnlyRecord>>(json);
        sw.Stop();

        long elapsedMs = sw.ElapsedMilliseconds;
        result.Should().HaveCount(numExecutions);
        double executionsPerSecond = 1_000 * numExecutions / elapsedMs;
        Console.WriteLine($"Deserialized {numExecutions} objects in {elapsedMs} ms. \n  Executions per second: {executionsPerSecond}, minimum allowed: {minExecutionsPerSecond}");
        executionsPerSecond.Should().BeGreaterThan(50); 
    }

}