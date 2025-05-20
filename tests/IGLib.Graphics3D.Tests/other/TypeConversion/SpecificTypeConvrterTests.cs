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

        #region CapturedVarTests

        [Fact]
        protected void CapturedVar_StaticVarType_WorksCorrectlyWithoutStatingVariableTye()
        {
            // Arrange:
            IList<int> variable = new List<int>() { 1, 2, 3, 4, 5 } ;
            Type actualVariableType = variable?.GetType();
            Type declaredVadiableType = typeof(IList<int>);
            // Type 
            Console.WriteLine($"Test: getting declared type of a variable with static method VarType(variable)...\n");
            Console.WriteLine($"Actual type of variable's value:          {actualVariableType.Name}");
            Console.WriteLine($"Declared type of the variable:            {declaredVadiableType.Name}");
            // Act:
            Type discoveredDeclaredVariableType = VarType(variable);
            Console.WriteLine($"Discovered declared type of the variable: {discoveredDeclaredVariableType.Name}");
            actualVariableType.Should().NotBe(declaredVadiableType, because: "PRECOND: Declared variable type should be different than actual type of its value for this test to be relevant.");
            // Assert:
            variable.GetType().Should().Be(typeof(List<int>), because: "Actual type of variable's value should be List of int and should be different than declared value.");
            discoveredDeclaredVariableType.Should().Be(typeof(IList<int>), because: "VarType(variable) should returne declared type of the variable, NOT actual type of variable value.");
        }

        [Fact]
        protected void CapturedVar_StaticCapture_WorksCorrectlyWithoutStatingVariableTye()
        {
            // Arrange:
            IList<int> variable = new List<int>() { 1, 2, 3, 4, 5 } ;
            Console.WriteLine($"Test: getting declared type of a variable with static method VarType(variable)...\n");
            Console.WriteLine($"Actual type of variable's value:          {variable.GetType().Name}");
            Console.WriteLine($"Declared type of the variable:            {typeof(IList<int>).Name}");
            // Act:

            ICapturedVar<IList<int>> capturedVarTyped = CaptureVar(variable);
            ICapturedVar capturedVar = capturedVarTyped;
            // Assert:
            
            
            variable.GetType().Should().Be(typeof(List<int>), because: "Actual type of variable's value should be List of int and should be different than declared value.");
            //declaredVariableType.Should().Be(typeof(IList<int>), because: "VarType(variable) should returne declared type of the variable, NOT actual type of variable value.");
        }

        #endregion CapturedVarTests


            #region ParseHelperTests

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

        #endregion ParseHelperTests

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


    public static class CapturedVar
    {

        public static CapturedVar<CapturedVariableType>
            CaptureVar<CapturedVariableType>(CapturedVariableType var)
        {
            return new CapturedVar<CapturedVariableType>(var);
        }

        public static Type VarType<TypeVar>(TypeVar var)
        {
            return CaptureVar(var)?.Type;
        }

    }


    public interface ICapturedVar
    {

        object ValueObject { get; }

        Type Type { get; }
        
        string TypeName { get; }
        
        string TypeFullName { get; }
        
        string TypeString { get; }


        Type ValueType { get; }

        string ValueTypeName { get; }

        string ValueTypeFullName { get; }

        string ValueTypeString { get; }


        string ToString();

    }

    public interface ICapturedVar<DeclaredType>: ICapturedVar
    {
        
        DeclaredType Value { get; init; }
    
    }

    public class CapturedVar<DeclaredType> : ICapturedVar<DeclaredType>
    {

        public CapturedVar(DeclaredType variable)
        {
            Value = variable;
        }

        public DeclaredType Value { get; init; }

        public object ValueObject => Value;

        public Type Type => typeof(DeclaredType);

        public string TypeName => Type.Name;

        public string TypeFullName => Type.FullName;

        public string TypeString => Type.ToString();

        public Type ValueType => Value?.GetType();

        public string ValueTypeName => ValueType?.Name;

        public string ValueTypeFullName => ValueType?.FullName;

        public string ValueTypeString => ValueType?.ToString();


        public override string ToString()
        {
            return $"Variable: type = {TypeString}, actual type = {ValueTypeString}, "
                + $"\n    value = {CollectionExtensions.CollectionExtensions.ToReadableString(Value)}";
        }

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


