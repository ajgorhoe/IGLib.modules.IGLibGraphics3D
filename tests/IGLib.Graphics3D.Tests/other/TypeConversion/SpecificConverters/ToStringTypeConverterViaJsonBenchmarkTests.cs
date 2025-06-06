
// Uncomment the definition below in order to include tests that fail at this hierarchy level by design!
// #define IncludeFailedTestsByDesign

using FluentAssertions;
using IGLib.Core;
using IGLib.Core.Tests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace IGLib.Core.Tests
{

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


        protected readonly ToStringTypeConverterViaJson toJson = new();
        protected readonly FromStringTypeConverterViaJson fromJson = new();

        /// <summary>Used in warm up stage of some tests that measure CPU efficiency.
        /// <para>Warm up stage performs similar uperations as the actual test, and its aim is to cause
        /// the necessary JIT compilations being performed before the time is measured, to help boost
        /// the fast CPU cores and to help put the relevant memory to cache. This solves the frequently
        /// observed problem that the test that is executed first usually reports much longer execution
        /// times, which is due to reasons related to the above: JIT-ing, CPU boost, and cache alignment
        /// are done the first time these things are needed, which may add the latencies caused by these
        /// things to the measured time for operations, and this can result in 10 or 100 times worse
        /// results than expected.</para></summary>
        protected internal const int NumWarmupCycles = 5;

        /// <summary>Numer of serialized objects or number of executions in performance tests.</summary>
        protected internal const int NumObjectsOrExecutions = 1_000;

        /// <summary>Minimal requested number of serialized objects or number of executions per second in performance tests.</summary>
        protected internal const int MinObjectsOrxecutionsPerSecond = 1_000;

        /// <summary>The default maximum nesting levels of the JSON serialization (the property of .NET JSON serialization).</summary>
        public const int DefaultMaxNestingLevels = 64;


        private class NestedLevel
        {
            public string Name { get; set; }
            public NestedLevel Inner { get; set; }
        }

        /// <summary>Used in serialization / deserialization benchmark tests.</summary>
        protected record InitOnlyRecord(string Name, int Id)
        {
            public override string ToString()
            {
                return $"{GetType().Name} object:\n  Name: {Name}\n  Id: {Id}\n";
            }

        };


        /// <summary>Used in serialization / deserialization benchmark tests.
        /// <para>Has the same fields as <see cref="InitOnlyRecord"/>.</para></summary>
        protected class InitOnlyClass
        {
            public InitOnlyClass(string name, int id)
            {
                Name = name;
                Id = id;
            }
            public string Name { get; set; }
            public int Id { get; set; }
            public override string ToString()
            {
                return $"{GetType().Name} object:\n  Name: {Name}\n  Id: {Id}\n";
            }
        }



        [Theory]
        [InlineData(DefaultMaxNestingLevels - 1)]
#if IncludeFailedTestsByDesign
        [InlineData(MaxNestingLevels)]
        [InlineData(MaxNestingLevels + 1)]
#endif
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
        public void Json_InitOnlyRecord_ShouldRoundTripCorrectly()
        {
            Console.WriteLine($"Testing round trip serializatio/deserialization of {nameof(InitOnlyRecord)}...");
            var obj = new InitOnlyRecord("Immutable", 42);
            Console.WriteLine($"Obect to be serialized/seseerialized: \n{obj}");
            var json = JsonSerializer.Serialize(obj);
            Console.WriteLine($"Serialized objct: \n{json}\n");
            var result = JsonSerializer.Deserialize<InitOnlyRecord>(json);
            Console.WriteLine($"Deserialized object: \n{result}");

            result.Should().Be(obj);
        }

        [Fact]
        public void Json_InitOnlyRecord_Serialize_Performance_And_Size_Benchmark()
        {
            Console.WriteLine($"Benchmarking serialization of large lists of objexts...");
            int numObjects = NumObjectsOrExecutions;
            int minSize = numObjects * 10;
            double minObjectsPerSecond = MinObjectsOrxecutionsPerSecond;
            Console.WriteLine($"Number of objects: {(double)numObjects / 1_000} k, min. expected size: {(double)minSize / 1_000.0}, min. exp. objecst / s: {(double)minObjectsPerSecond / 1_000} k");
            Console.WriteLine("Warning: If this is the first test executed, the results can be much worse (easily by a factor of 100 or more).");
            Console.WriteLine("");
            var largeList = new List<InitOnlyRecord>();
            for (int i = 0; i < numObjects; i++)
            {
                largeList.Add(new InitOnlyRecord($"Name{i}", i));
            }
            if (NumWarmupCycles > 0)
            {
                var warmupList = new List<InitOnlyRecord>();
                for (int i = 0; i < NumWarmupCycles; i++)
                {
                    warmupList.Add(new InitOnlyRecord($"Name{i}", i));
                }
                string json1 = JsonSerializer.Serialize(warmupList);
                if (NumWarmupCycles <= 10)
                {
                    Console.WriteLine($"\nSerialized warmup list of objects (much shorter than the real list:");
                    Console.WriteLine($"{json1}\n");
                }
            }

            Stopwatch sw = Stopwatch.StartNew();
            string json = JsonSerializer.Serialize(largeList);
            sw.Stop();

            double elapsedSeconds = sw.Elapsed.TotalSeconds;
            int sizeBytes = System.Text.Encoding.UTF8.GetByteCount(json);
            double objectsPerSecond = (double)numObjects / elapsedSeconds;
            Console.WriteLine($"Size: {sizeBytes / 1024.0:F1} kB");
            Console.WriteLine($"Serialized {numObjects} objects in {elapsedSeconds / 1_000.0} ms.");
            Console.WriteLine($"  Objects per second: {objectsPerSecond / 1_000_000.0} M, minimum required: {(double)minObjectsPerSecond / 1_000} k");

            // Example expectations: can be adjusted based on environment
            objectsPerSecond.Should().BeGreaterThan(minObjectsPerSecond);
            sizeBytes.Should().BeGreaterThan(minSize);
        }

        [Fact]
        public void Json_InitOnlyRecord_Deserialize_Performance_And_Size_Benchmark()
        {
            int numObjects = NumObjectsOrExecutions;
            double minObjectsPerSecond = MinObjectsOrxecutionsPerSecond;
            Console.WriteLine("Benchmarking deserialization of a large list of objects...");
            Console.WriteLine($"Number of objects: {numObjects / 1_000} k. Minimal number of objects per second: {minObjectsPerSecond / 1_000} k");
            var largeList = new List<InitOnlyRecord>();
            for (int i = 0; i < numObjects; i++)
            {
                largeList.Add(new InitOnlyRecord($"Name{i}", i));
            }
            string json = JsonSerializer.Serialize(largeList);

            if (NumWarmupCycles > 0)
            {
                // Perform a warmup loop similar to what is measured, in order to perfotm the warmup: have the methods used
                // JIT-ed, busting the CPU speed, having relevant memory in the cache, etc.
                var warmupList = new List<InitOnlyRecord>();
                for (int i = 0; i < NumWarmupCycles; i++)
                {
                    warmupList.Add(new InitOnlyRecord($"Name{i}", i));
                }
                string json1 = JsonSerializer.Serialize(warmupList);
                Stopwatch sw1 = Stopwatch.StartNew();
                var result1 = JsonSerializer.Deserialize<List<InitOnlyRecord>>(json1);
                sw1.Stop();
                result1.Should().HaveCount(NumWarmupCycles, "PRECOND: warm up works correctly.");
                if (NumWarmupCycles <= 10)
                {
                    Console.WriteLine($"\nSerialized warmup list of objects (much shorter than the real list:");
                    Console.WriteLine($"{json1}\n");
                }
            }
            Stopwatch sw = Stopwatch.StartNew();
            var result = JsonSerializer.Deserialize<List<InitOnlyRecord>>(json);
            sw.Stop();

            double elapsedSeconds = sw.Elapsed.TotalSeconds;
            result.Should().HaveCount(numObjects);
            for (int i = 0; i < numObjects; i++)
            {
                result[i].Id.Should().Be(i, because: $"PRECOND: Deserialization should correctly restored ID property of objects on the deserialized list.");
            }
            double executionsPerSecond = numObjects / elapsedSeconds;
            Console.WriteLine($"Deserialized {numObjects} objects in {elapsedSeconds * 1_000} ms.");
            Console.WriteLine($"  Objects per second: {executionsPerSecond / 1_000} k, minimum required: {minObjectsPerSecond / 1_000} k");
            executionsPerSecond.Should().BeGreaterThan(minObjectsPerSecond);
        }


        [Fact]
        public void Json_InitOnlyClass_ShouldRoundTripCorrectly()
        {
            Console.WriteLine($"Testing round trip serializatio/deserialization of {nameof(InitOnlyClass)}...");
            var obj = new InitOnlyClass("InitOnlyClass_Object1", 425);
            Console.WriteLine($"Obect to be serialized/seseerialized: \n{obj}");
            var json = JsonSerializer.Serialize(obj);
            Console.WriteLine($"Serialized objct: \n{json}\n");
            var result = JsonSerializer.Deserialize<InitOnlyClass>(json);
            Console.WriteLine($"Deserialized object: \n{result}");

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(obj);
        }

        [Fact]
        public void Json_InitOnlyClas_Serialize_Performance_And_Size_Benchmark()
        {
            Console.WriteLine($"Benchmarking serialization of large lists of objects...");
            int numObjects = NumObjectsOrExecutions;
            int minSize = numObjects * 10;
            double minObjectsPerSecond = MinObjectsOrxecutionsPerSecond;
            Console.WriteLine($"Number of objects: {(double)numObjects / 1_000} k, min. expected size: {(double)minSize / 1_000.0}, min. exp. objecst / s: {(double)minObjectsPerSecond / 1_000} k");
            Console.WriteLine("");
            var largeList = new List<InitOnlyClass>();
            for (int i = 0; i < numObjects; i++)
            {
                largeList.Add(new InitOnlyClass($"Name{i}", i));
            }
            if (NumWarmupCycles > 0)
            {
                var warmupList = new List<InitOnlyClass>();
                for (int i = 0; i < NumWarmupCycles; i++)
                {
                    warmupList.Add(new InitOnlyClass($"Name{i}", i));
                }
                string json1 = JsonSerializer.Serialize(warmupList);
                if (NumWarmupCycles <= 10)
                {
                    Console.WriteLine($"\nSerialized warmup list of objects (much shorter than the real list:");
                    Console.WriteLine($"{json1}\n");
                }
            }

            Stopwatch sw = Stopwatch.StartNew();
            string json = JsonSerializer.Serialize(largeList);
            sw.Stop();

            double elapsedSeconds = sw.Elapsed.TotalSeconds;
            int sizeBytes = System.Text.Encoding.UTF8.GetByteCount(json);
            double objectsPerSecond = (double)numObjects / elapsedSeconds;
            Console.WriteLine($"Size: {sizeBytes / 1024.0:F1} kB");
            Console.WriteLine($"Serialized {numObjects} objects in {elapsedSeconds / 1_000.0} ms.");
            Console.WriteLine($"  Objects per second: {objectsPerSecond / 1_000_000.0} M, minimum required: {(double)minObjectsPerSecond / 1_000} k");

            // Example expectations: can be adjusted based on environment
            objectsPerSecond.Should().BeGreaterThan(minObjectsPerSecond);
            sizeBytes.Should().BeGreaterThan(minSize);
        }

        [Fact]
        public void Json_InitOnlyClas_Deserialize_Performance_And_Size_Benchmark()
        {
            int numObjects = NumObjectsOrExecutions;
            double minObjectsPerSecond = MinObjectsOrxecutionsPerSecond;
            Console.WriteLine("Benchmarking deserialization of a large list of objects...");
            Console.WriteLine($"Number of objects: {numObjects / 1_000} k. Minimal number of objects per second: {minObjectsPerSecond / 1_000} k");
            var largeList = new List<InitOnlyClass>();
            for (int i = 0; i < numObjects; i++)
            {
                largeList.Add(new InitOnlyClass($"Name{i}", i));
            }
            string json = JsonSerializer.Serialize(largeList);

            if (NumWarmupCycles > 0)
            {
                // Perform a warmup loop similar to what is measured, in order to perfotm the warmup: have the methods used
                // JIT-ed, busting the CPU speed, having relevant memory in the cache, etc.
                var warmupList = new List<InitOnlyClass>();
                for (int i = 0; i < NumWarmupCycles; i++)
                {
                    warmupList.Add(new InitOnlyClass($"Name{i}", i));
                }
                string json1 = JsonSerializer.Serialize(warmupList);
                Stopwatch sw1 = Stopwatch.StartNew();
                var result1 = JsonSerializer.Deserialize<List<InitOnlyClass>>(json1);
                sw1.Stop();
                result1.Should().HaveCount(NumWarmupCycles, "PRECOND: warm up works correctly.");
                if (NumWarmupCycles <= 10)
                {
                    Console.WriteLine($"\nSerialized warmup list of objects (much shorter than the real list:");
                    Console.WriteLine($"{json1}\n");
                }
            }
            Stopwatch sw = Stopwatch.StartNew();
            var result = JsonSerializer.Deserialize<List<InitOnlyClass>>(json);
            sw.Stop();

            double elapsedSeconds = sw.Elapsed.TotalSeconds;
            result.Should().HaveCount(numObjects);
            for (int i = 0; i < numObjects; i++)
            {
                result[i].Id.Should().Be(i, because: $"PRECOND: Deserialization should correctly restored ID property of objects on the deserialized list.");
            }
            double executionsPerSecond = numObjects / elapsedSeconds;
            Console.WriteLine($"Deserialized {numObjects} objects in {elapsedSeconds * 1_000} ms.");
            Console.WriteLine($"  Objects per second: {executionsPerSecond / 1_000} k, minimum required: {minObjectsPerSecond / 1_000} k");
            executionsPerSecond.Should().BeGreaterThan(minObjectsPerSecond);
        }


        [Fact]
        public void Json_InitOnlyClas_WithReferenceHandler_Serialize_Performance_And_Size_Benchmark()
        {
            Console.WriteLine($"Benchmarking serialization of large lists of objexts...");
            var serializationOptions = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                WriteIndented = false
            };

            int numObjects = NumObjectsOrExecutions;
            int minSize = numObjects * 10;
            double minObjectsPerSecond = MinObjectsOrxecutionsPerSecond;
            Console.WriteLine($"Number of objects: {(double)numObjects / 1_000} k, min. expected size: {(double)minSize / 1_000.0}, min. exp. objecst / s: {(double)minObjectsPerSecond / 1_000} k");
            Console.WriteLine("");
            var largeList = new List<InitOnlyClass>();
            for (int i = 0; i < numObjects; i++)
            {
                largeList.Add(new InitOnlyClass($"Name{i}", i));
            }
            if (NumWarmupCycles > 0)
            {
                var warmupList = new List<InitOnlyClass>();
                for (int i = 0; i < NumWarmupCycles; i++)
                {
                    warmupList.Add(new InitOnlyClass($"Name{i}", i));
                }
                string json1 = JsonSerializer.Serialize(warmupList, serializationOptions);
                if (NumWarmupCycles <= 10)
                {
                    Console.WriteLine($"\nSerialized warmup list of objects (much shorter than the real list:");
                    Console.WriteLine($"{json1}\n");
                }
            }

            Stopwatch sw = Stopwatch.StartNew();
            string json = JsonSerializer.Serialize(largeList, serializationOptions);
            sw.Stop();

            double elapsedSeconds = sw.Elapsed.TotalSeconds;
            int sizeBytes = System.Text.Encoding.UTF8.GetByteCount(json);
            double objectsPerSecond = (double)numObjects / elapsedSeconds;
            Console.WriteLine($"Size: {sizeBytes / 1024.0:F1} kB");
            Console.WriteLine($"Serialized {numObjects} objects in {elapsedSeconds / 1_000.0} ms.");
            Console.WriteLine($"  Objects per seconds: {objectsPerSecond / 1_000_000.0} M. Minimum required: {(double)minObjectsPerSecond / 1_000} k.");

            // Example expectations: can be adjusted based on environment
            objectsPerSecond.Should().BeGreaterThan(minObjectsPerSecond);
            sizeBytes.Should().BeGreaterThan(minSize);
        }

        [Fact]
        public void Json_InitOnlyClas_WithReferenceHandler_Deserialize_Performance_And_Size_Benchmark()
        {
            int numObjects = NumObjectsOrExecutions;
            double minObjectsPerSecond = MinObjectsOrxecutionsPerSecond;
            Console.WriteLine("Benchmarking deserialization of a large list of objects...");
            Console.WriteLine($"Number of objects: {numObjects / 1_000} k. Minimal number of objects per second: {minObjectsPerSecond / 1_000} k");
            var serializationOptions = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                WriteIndented = false
            };
            var largeList = new List<InitOnlyClass>();
            for (int i = 0; i < numObjects; i++)
            {
                largeList.Add(new InitOnlyClass($"Name{i}", i));
            }
            string json = JsonSerializer.Serialize(largeList, serializationOptions);

            if (NumWarmupCycles > 0)
            {
                // Perform a warmup loop similar to what is measured, in order to perfotm the warmup: have the methods used
                // JIT-ed, busting the CPU speed, having relevant memory in the cache, etc.
                var warmupList = new List<InitOnlyClass>();
                for (int i = 0; i < NumWarmupCycles; i++)
                {
                    warmupList.Add(new InitOnlyClass($"Name{i}", i));
                }
                string json1 = JsonSerializer.Serialize(warmupList, serializationOptions);
                Stopwatch sw1 = Stopwatch.StartNew();
                var result1 = JsonSerializer.Deserialize<List<InitOnlyClass>>(json1, serializationOptions);
                sw1.Stop();
                result1.Should().HaveCount(NumWarmupCycles, "PRECOND: warm up works correctly.");
                if (NumWarmupCycles <= 10)
                {
                    Console.WriteLine($"\nSerialized warmup list of objects (much shorter than the real list:");
                    Console.WriteLine($"{json1}\n");
                }
            }
            Stopwatch sw = Stopwatch.StartNew();
            var result = JsonSerializer.Deserialize<List<InitOnlyClass>>(json, serializationOptions);
            sw.Stop();

            double elapsedSeconds = sw.Elapsed.TotalSeconds;
            result.Should().HaveCount(numObjects);
            for (int i = 0; i < numObjects; i++)
            {
                result[i].Id.Should().Be(i, because: $"PRECOND: Deserialization should correctly restored ID property of objects on the deserialized list.");
            }
            double executionsPerSecond = numObjects / elapsedSeconds;
            Console.WriteLine($"Deserialized {numObjects} objects in {elapsedSeconds * 1_000} ms.");
            Console.WriteLine($"  Objects per second: {executionsPerSecond / 1_000} k, minimum required: {minObjectsPerSecond / 1_000} k");
            executionsPerSecond.Should().BeGreaterThan(minObjectsPerSecond);
        }


    }

}