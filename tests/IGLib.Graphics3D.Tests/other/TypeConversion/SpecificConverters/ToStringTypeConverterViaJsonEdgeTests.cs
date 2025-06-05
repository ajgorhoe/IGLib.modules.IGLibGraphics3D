using FluentAssertions;
using IGLib.Core;
using IGLib.Core.Tests;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
// using System.Text.Json.Serialization;
using Xunit;
using Xunit.Abstractions;


namespace IGLib.Core.Tests
{

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
            Console.WriteLine($"Testing circular references with JSON serializerr with ReferenceHandler...");
            var parent = new RecursiveNode { Name = "Parent" };
            var child = new RecursiveNode { Name = "Child", Child = parent };
            parent.Child = child;

            var serializationOptions = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(parent, serializationOptions);
            Console.WriteLine($"JSON of object tree with circular references, using ReferenceHandler.Preserve:{json}");
            json.Should().Contain("$id").And.Contain("$ref");

            var result = JsonSerializer.Deserialize<RecursiveNode>(json, serializationOptions);
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


        public const int DefaultHiddenFieldValue = 5;

        /// <summary>Class thatt contain a private field and readonly public property based on hat field.</summary>
        public class NonSerializableWithPrivateFields
        {
            private int hidden = DefaultHiddenFieldValue;
            public int Visible => hidden;

            public override string ToString()
            {
                return $"{nameof(NonSerializableWithPrivateFields)} object:\n  hidden: {hidden}\n  Visible: {Visible}\n";
            }
        }

        /// <summary>Class thatt contain a private field and readonly public property based on hat field.</summary>
        public class NonSerializableWithPrivateFieldsSetByConstructor
        {
            public NonSerializableWithPrivateFieldsSetByConstructor(int hidden)
            {
                this.hidden = hidden;
            }

            private int hidden;
            public int Visible => hidden;

            public override string ToString()
            {
                return $"{nameof(NonSerializableWithPrivateFields)} object:\n  hidden: {hidden}\n  Visible: {Visible}\n";
            }
        }

        [Fact]
        public void PrivateFields_ShouldNotBeSerialized()
        {
            Console.WriteLine($"Testing serialization / deserialization of objectts with private fields...");
            Console.WriteLine($"Default hidden field value: {DefaultHiddenFieldValue}");
            var obj = new NonSerializableWithPrivateFields();
            Console.WriteLine($"\nObject to be serialized: \n{obj}\n");
            var json = JsonSerializer.Serialize(obj);
            Console.WriteLine($"JSON-serialized object: {json}");

            json.Should().Contain("Visible");
            json.Should().NotContain("hidden");
            
            Console.WriteLine($"\nDeserializing the object...");
            var result = JsonSerializer.Deserialize<NonSerializableWithPrivateFields>(json);
            Console.WriteLine($"Deserialized object:  \n{result}\n");
            result.Visible.Should().Be(DefaultHiddenFieldValue); // default int, field was not restored
        }

        

        [Fact]
        public void PrivateFields_ShouldNotBeSerializedWhenPublicConstructor()
        {
            JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
            {
                // IncludeFields = true,
                MaxDepth = 100,
                PropertyNameCaseInsensitive = true,
            };
            

            Console.WriteLine($"Testing serialization / deserialization of objectts with private fields...");
            // Console.WriteLine($"Default hidden field value: {DefaultHiddenFieldValue}");
            var obj = new NonSerializableWithPrivateFieldsSetByConstructor(DefaultHiddenFieldValue + 1);
            Console.WriteLine($"\nObject to be serialized: \n{obj}\n");
            var json = JsonSerializer.Serialize(obj, serializerOptions);
            Console.WriteLine($"JSON-serialized object: {json}");

            json.Should().Contain("Visible");
            json.Should().NotContain("hidden");
            
            Console.WriteLine($"\nDeserializing the object...");
            var result = JsonSerializer.Deserialize<NonSerializableWithPrivateFieldsSetByConstructor>(json, serializerOptions);
            Console.WriteLine($"Deserialized object:  \n{result}\n");
            result.Visible.Should().Be(DefaultHiddenFieldValue); // default int, field was not restored
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

}