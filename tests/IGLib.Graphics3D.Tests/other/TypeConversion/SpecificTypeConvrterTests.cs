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
using Microsoft.Extensions.Configuration;


namespace IGLib.Core.Tests
{

    using static CapturedVarS;


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
            IList<int> variable = new List<int>() { 1, 2, 3 } ;
            Type actualVariableType = variable?.GetType();
            Type declaredVariableType = typeof(IList<int>);
            Console.WriteLine($"Test: getting variable's declared type with static method VarType(variable)...\n");
            Console.WriteLine($"Variable: actual type = {variable?.GetType()?.ToString()}, value = \n  {variable.ToReadableString()}\n");
            Console.WriteLine($"Actual type of variable's value:          {actualVariableType?.ToString()}");
            Console.WriteLine($"Declared type of the variable:            {declaredVariableType.ToString()}");
            // Act:
            Type discoveredDeclaredVariableType = VarType(variable);
            Console.WriteLine($"Discovered declared type of the variable: {discoveredDeclaredVariableType.ToString()}");
            declaredVariableType.Should().NotBeNull(because: "PRECOND: declared value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBeNull(because: "PRECOND: actual value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBe(declaredVariableType, 
                because: "PRECOND: declared variable type should be different than actual type of its value for this test to be relevant");
            // Assert:
            discoveredDeclaredVariableType.Should().Be(declaredVariableType, because: "VarType(variable) should returne declared type of the variable, NOT actual type of variable value");
        }

        [Fact]
        protected void CapturedVar_StaticCaptureVar_WorksCorrectlyWithoutStatingVariableTye()
        {
            // Arrange:
            IList<int> variable = new List<int>() { 1, 2, 3, 4 } ;
            Type actualVariableType = variable?.GetType();
            Type declaredVariableType = typeof(IList<int>);
            Console.WriteLine($"Test: capturing variable with static method CaptureVar(variable)...\n");
            Console.WriteLine($"Variable: actual type = {variable?.GetType()?.ToString()}, value = \n  {variable.ToReadableString()}\n");
            Console.WriteLine($"Actual type of variable's value: {actualVariableType?.ToString()}");
            Console.WriteLine($"Declared type of the variable:   {declaredVariableType.ToString()}\n");
            // Act:
            ICapturedVar<IList<int>> capturedVarTyped = CaptureVar(variable);
            ICapturedVar capturedVar = capturedVarTyped;
            Console.WriteLine($"Captured var, actual type:       {capturedVar.ValueTypeString}");
            Console.WriteLine($"Captured var, declared type:     {capturedVar.TypeString}");
            Console.WriteLine($"Captured var, value:             {capturedVar.ValueObject.ToReadableString()}\n");
            declaredVariableType.Should().NotBeNull(because: "PRECOND: declared value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBeNull(because: "PRECOND: actual value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBe(declaredVariableType,
                because: "PRECOND: Declared variable type should be different than actual type of its value for this test to be relevant.");
            variable.Should().NotBeNull(because: "PRECOND: value of captured variable shoulld not be null for this test to be relevant.");
            // Assert:
            capturedVar.Should().NotBeNull(because: "CaptureVar(variable) should produce a valid captured variable object.");
            capturedVar.ValueObject.Should().NotBeNull(because: "captured variable's value is not null");
            ((object)capturedVarTyped.Value).Should().Be(variable, because: "value of the captured variable should be correctly captured");
            capturedVar.ValueObject.Should().Be(variable, because: "value of the captured variable should be correctly captured and reflected even in non-generic ICapturedVar interface");

            capturedVar.Type.Should().Be(declaredVariableType,because: "Type property of captured variable should contain the declared variable type.");
            capturedVar.ValueType.Should().Be(actualVariableType,because: "ValueType property of captured variable should contain the actual type of variable value.");
        }

        [Fact]
        protected void CapturedVar_StaticCaptureVar_PropertiesShouldBeCorrect()
        {
            // Arrange:
            IList<int> variable = new List<int>() { 1, 2, 3, 4, 5 };
            Type actualVariableType = variable?.GetType();
            Type declaredVariableType = typeof(IList<int>);
            Console.WriteLine($"Test: capturing variable with static method CaptureVar(variable) and checking properties...\n");
            Console.WriteLine($"Variable: actual type = {variable?.GetType()?.ToString()}, value = \n  {variable.ToReadableString()}\n");
            Console.WriteLine($"Actual type of variable's value: {actualVariableType?.ToString()}");
            Console.WriteLine($"Declared type of the variable:   {declaredVariableType.ToString()}\n");

            // Act:
            ICapturedVar<IList<int>> capturedVarTyped = CaptureVar(variable);
            ICapturedVar capturedVar = capturedVarTyped;
            Console.WriteLine($"Captured var, actual type:       {capturedVar.ValueTypeString}");
            Console.WriteLine($"Captured var, declared type:     {capturedVar.TypeString}");
            Console.WriteLine($"Captured var, value:             {capturedVar.ValueObject.ToReadableString()}\n");
            Console.WriteLine($"Full captured variable object:\n{capturedVar?.ToStringLong("    ")}");

            declaredVariableType.Should().NotBeNull(because: "PRECOND: declared value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBeNull(because: "PRECOND: actual value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBe(declaredVariableType,
                because: "PRECOND: Declared variable type should be different than actual type of its value for this test to be relevant.");
            variable.Should().NotBeNull(because: "PRECOND: value of captured variable shoulld not be null for this test to be relevant.");
            // Assert:

            capturedVar.Type.Should().Be(declaredVariableType, 
                because: "Type property of captured variable should contain the declared variable type.");
            capturedVar.TypeName.Should().Be(declaredVariableType.Name, 
                because: "TypeName property of captured variable should contain name of the declared variable type.");
            capturedVar.TypeFullName.Should().Be(declaredVariableType.FullName, 
                because: "TypeFullName property of captured variable should contain full name of the declared variable type.");
            capturedVar.TypeString.Should().Be(declaredVariableType.ToString(), 
                because: "TypeString property of captured variable should contain result of ToString() of the declared variable type.");
            
            capturedVar.ValueType.Should().Be(actualVariableType, 
                because: "ValueType property of captured variable should contain the actual type of variable's value.");
            capturedVar.ValueTypeName.Should().Be(actualVariableType.Name, 
                because: "ValueTypeName property of captured variable should contain name of the actual variable value's type.");
            capturedVar.ValueTypeFullName.Should().Be(actualVariableType.FullName, 
                because: "ValueTypeFullName property of captured variable should contain full name of the actual variable value's type.");
            capturedVar.ValueTypeString.Should().Be(actualVariableType?.ToString(), 
                because: "ValueTypeString property of captured variable should contain result of ToString() of the actual variable value's type.");
        }


        [Fact]
        protected void CapturedVar_StaticVarType_WorksCorrectlyWithCastValue()
        {
            // Arrange:
            IList<int> variable = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
            Type actualVariableType = variable?.GetType();
            Type declaredVariableType = typeof(IList<int>);
            Console.WriteLine($"Test: getting variable's declared type with static method VarType(variable)...\n");
            Console.WriteLine($"Variable: actual type = {variable?.GetType()?.ToString()}, value = \n  {variable.ToReadableString()}\n");
            Console.WriteLine($"Actual type of variable's value:          {actualVariableType?.ToString()}");
            Console.WriteLine($"Declared type of the variable:            {declaredVariableType.ToString()}");
            // Act:
            Type discoveredDeclaredVariableType = VarType((IList<int>) new List<int>() { 1, 2, 3, 4, 5, 6, 7 });
            Console.WriteLine($"Discovered declared type of the variable: {discoveredDeclaredVariableType.ToString()}");
            declaredVariableType.Should().NotBeNull(because: "PRECOND: declared value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBeNull(because: "PRECOND: actual value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBe(declaredVariableType,
                because: "PRECOND: declared variable type should be different than actual type of its value for this test to be relevant");
            // Assert:
            discoveredDeclaredVariableType.Should().Be(declaredVariableType, because: "VarType(variable) should returne declared type of the variable, NOT actual type of variable value");
        }

        [Fact]
        protected void CapturedVar_StaticCaptureVar_WorksCorrectlyWithCastValue()
        {
            // Arrange:
            IList<int> variable = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            Type actualVariableType = variable?.GetType();
            Type declaredVariableType = typeof(IList<int>);
            Console.WriteLine($"Test: capturing variable with static method CaptureVar(variable)...\n");
            Console.WriteLine($"Variable: actual type = {variable?.GetType()?.ToString()}, value = \n  {variable.ToReadableString()}\n");
            Console.WriteLine($"Actual type of variable's value: {actualVariableType?.ToString()}");
            Console.WriteLine($"Declared type of the variable:   {declaredVariableType.ToString()}\n");
            // Act:
            ICapturedVar<IList<int>> capturedVarTyped = CaptureVar((IList<int>)new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 });
            ICapturedVar capturedVar = capturedVarTyped;
            Console.WriteLine($"Captured var, actual type:       {capturedVar.ValueTypeString}");
            Console.WriteLine($"Captured var, declared type:     {capturedVar.TypeString}");
            Console.WriteLine($"Captured var, value:             {capturedVar.ValueObject.ToReadableString()}\n");
            declaredVariableType.Should().NotBeNull(because: "PRECOND: declared value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBeNull(because: "PRECOND: actual value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBe(declaredVariableType,
                because: "PRECOND: Declared variable type should be different than actual type of its value for this test to be relevant.");
            variable.Should().NotBeNull(because: "PRECOND: value of captured variable shoulld not be null for this test to be relevant.");
            // Assert:
            capturedVar.Should().NotBeNull(because: "CaptureVar(variable) should produce a valid captured variable object.");
            capturedVar.ValueObject.Should().NotBeNull(because: "captured variable's value is not null");

            capturedVar.Type.Should().Be(declaredVariableType, because: "Type property of captured variable should contain the declared variable type.");
            capturedVar.ValueType.Should().Be(actualVariableType, because: "ValueType property of captured variable should contain the actual type of variable value.");
        }

        [Fact]
        protected void CapturedVar_StaticVarType_WorksCorrectlyWithCastValueNull()
        {
            // Arrange:
            IList<int> variable = null;
            Type actualVariableType = variable?.GetType();
            Type declaredVariableType = typeof(IList<int>);
            Console.WriteLine($"Test: getting variable's declared type with static method VarType(variable)...\n");
            Console.WriteLine($"Variable: actual type = {actualVariableType?.ToString()}, value = \n  {variable.ToReadableString()}\n");
            Console.WriteLine($"Actual type of variable's value:          {actualVariableType?.ToString()}");
            Console.WriteLine($"Declared type of the variable:            {declaredVariableType.ToString()}");
            // Act:
            Type discoveredDeclaredVariableType = VarType((IList<int>) null);
            Console.WriteLine($"Discovered declared type of the variable: {discoveredDeclaredVariableType.ToString()}");
            declaredVariableType.Should().NotBeNull(because: "PRECOND: declared value type should not be null for this test to be relevant.");
            actualVariableType.Should().BeNull(because: "PRECOND: actual value type should be null when value is null.");
            // Assert:
            discoveredDeclaredVariableType.Should().Be(declaredVariableType, because: "VarType(variable) should returne declared type of the variable, NOT actual type of variable value");
        }

        [Fact]
        protected void CapturedVar_StaticCaptureVar_WorksCorrectlyWithCastValueNull()
        {
            // Arrange:
            IList<int> variable = null;
            Type actualVariableType = variable?.GetType();
            Type declaredVariableType = typeof(IList<int>);
            Console.WriteLine($"Test: capturing variable with static method CaptureVar(variable)...\n");
            Console.WriteLine($"Variable: actual type = {actualVariableType?.ToString()}, value = \n  {variable.ToReadableString()}\n");
            Console.WriteLine($"Actual type of variable's value: {actualVariableType?.ToString()}");
            Console.WriteLine($"Declared type of the variable:   {declaredVariableType.ToString()}\n");
            // Act:
            ICapturedVar<IList<int>> capturedVarTyped = CaptureVar((IList<int>) null);
            ICapturedVar capturedVar = capturedVarTyped;
            Console.WriteLine($"Captured var, actual type:       {capturedVar.ValueTypeString}");
            Console.WriteLine($"Captured var, declared type:     {capturedVar.TypeString}");
            Console.WriteLine($"Captured var, value:             {capturedVar.ValueObject.ToReadableString()}\n");
            declaredVariableType.Should().NotBeNull(because: "PRECOND: declared value type should not be null for this test to be relevant.");
            actualVariableType.Should().BeNull(because: "PRECOND: actual value type should not be null for this test to be relevant.");
            // Assert:
            capturedVar.Should().NotBeNull(because: "CaptureVar(variable) should produce a valid captured variable object.");
            capturedVar.ValueObject.Should().BeNull(because: "captured variable's value is null");

            capturedVar.Type.Should().Be(declaredVariableType, because: "Type property of captured variable should contain the declared variable type.");
            capturedVar.ValueType.Should().Be(actualVariableType, because: "ValueType property of captured variable should contain the actual type of variable value.");
        }




        // Direectly working with CapturedVar<T>, rather than static methods:

        [Fact]
        protected void CapturedVar_WorksCorrectlyWithoutStatingVariableTye()
        {
            // Arrange:
            IList<int> variable = new List<int>() { 4, 5, 6, 7 };
            Type actualVariableType = variable?.GetType();
            Type declaredVariableType = typeof(IList<int>);
            Console.WriteLine($"Test: capturing variable with static method CaptureVar(variable)...\n");
            Console.WriteLine($"Variable: actual type = {variable?.GetType()?.ToString()}, value = \n  {variable.ToReadableString()}\n");
            Console.WriteLine($"Actual type of variable's value: {actualVariableType?.ToString()}");
            Console.WriteLine($"Declared type of the variable:   {declaredVariableType.ToString()}\n");
            // Act:
            ICapturedVar<IList<int>> capturedVarTyped = new CapturedVar<IList<int>>(variable);
            ICapturedVar capturedVar = capturedVarTyped;
            Console.WriteLine($"Captured var, actual type:       {capturedVar.ValueTypeString}");
            Console.WriteLine($"Captured var, declared type:     {capturedVar.TypeString}");
            Console.WriteLine($"Captured var, value:             {capturedVar.ValueObject.ToReadableString()}\n");
            declaredVariableType.Should().NotBeNull(because: "PRECOND: declared value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBeNull(because: "PRECOND: actual value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBe(declaredVariableType,
                because: "PRECOND: Declared variable type should be different than actual type of its value for this test to be relevant.");
            variable.Should().NotBeNull(because: "PRECOND: value of captured variable shoulld not be null for this test to be relevant.");
            // Assert:
            capturedVar.Should().NotBeNull(because: "CaptureVar(variable) should produce a valid captured variable object.");
            capturedVar.ValueObject.Should().NotBeNull(because: "captured variable's value is not null");
            ((object)capturedVarTyped.Value).Should().Be(variable, because: "value of the captured variable should be correctly captured");
            capturedVar.ValueObject.Should().Be(variable, because: "value of the captured variable should be correctly captured and reflected even in non-generic ICapturedVar interface");

            capturedVar.Type.Should().Be(declaredVariableType, because: "Type property of captured variable should contain the declared variable type.");
            capturedVar.ValueType.Should().Be(actualVariableType, because: "ValueType property of captured variable should contain the actual type of variable value.");
        }

        [Fact]
        protected void CapturedVar_PropertiesShouldBeCorrect()
        {
            // Arrange:
            IList<int> variable = new List<int>() { 5, 6, 7 };
            Type actualVariableType = variable?.GetType();
            Type declaredVariableType = typeof(IList<int>);
            Console.WriteLine($"Test: capturing variable with static method CaptureVar(variable) and checking properties...\n");
            Console.WriteLine($"Variable: actual type = {variable?.GetType()?.ToString()}, value = \n  {variable.ToReadableString()}\n");
            Console.WriteLine($"Actual type of variable's value: {actualVariableType?.ToString()}");
            Console.WriteLine($"Declared type of the variable:   {declaredVariableType.ToString()}\n");

            // Act:
            ICapturedVar<IList<int>> capturedVarTyped = new CapturedVar<IList<int>>(variable);
            ICapturedVar capturedVar = capturedVarTyped;
            Console.WriteLine($"Captured var, actual type:       {capturedVar.ValueTypeString}");
            Console.WriteLine($"Captured var, declared type:     {capturedVar.TypeString}");
            Console.WriteLine($"Captured var, value:             {capturedVar.ValueObject.ToReadableString()}\n");
            Console.WriteLine($"Full captured variable object:\n{capturedVar?.ToStringLong("    ")}");

            declaredVariableType.Should().NotBeNull(because: "PRECOND: declared value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBeNull(because: "PRECOND: actual value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBe(declaredVariableType,
                because: "PRECOND: Declared variable type should be different than actual type of its value for this test to be relevant.");
            variable.Should().NotBeNull(because: "PRECOND: value of captured variable shoulld not be null for this test to be relevant.");
            // Assert:

            capturedVar.Type.Should().Be(declaredVariableType,
                because: "Type property of captured variable should contain the declared variable type.");
            capturedVar.TypeName.Should().Be(declaredVariableType.Name,
                because: "TypeName property of captured variable should contain name of the declared variable type.");
            capturedVar.TypeFullName.Should().Be(declaredVariableType.FullName,
                because: "TypeFullName property of captured variable should contain full name of the declared variable type.");
            capturedVar.TypeString.Should().Be(declaredVariableType.ToString(),
                because: "TypeString property of captured variable should contain result of ToString() of the declared variable type.");

            capturedVar.ValueType.Should().Be(actualVariableType,
                because: "ValueType property of captured variable should contain the actual type of variable's value.");
            capturedVar.ValueTypeName.Should().Be(actualVariableType.Name,
                because: "ValueTypeName property of captured variable should contain name of the actual variable value's type.");
            capturedVar.ValueTypeFullName.Should().Be(actualVariableType.FullName,
                because: "ValueTypeFullName property of captured variable should contain full name of the actual variable value's type.");
            capturedVar.ValueTypeString.Should().Be(actualVariableType?.ToString(),
                because: "ValueTypeString property of captured variable should contain result of ToString() of the actual variable value's type.");
        }


        [Fact]
        protected void CapturedVar_WorksCorrectlyWithCastValue()
        {
            // Arrange:
            IList<int> variable = new List<int>() { 5, 6, 7, 8 };
            Type actualVariableType = variable?.GetType();
            Type declaredVariableType = typeof(IList<int>);
            Console.WriteLine($"Test: capturing variable with static method CaptureVar(variable)...\n");
            Console.WriteLine($"Variable: actual type = {variable?.GetType()?.ToString()}, value = \n  {variable.ToReadableString()}\n");
            Console.WriteLine($"Actual type of variable's value: {actualVariableType?.ToString()}");
            Console.WriteLine($"Declared type of the variable:   {declaredVariableType.ToString()}\n");
            // Act:
            ICapturedVar<IList<int>> capturedVarTyped = new CapturedVar<IList<int>>((IList<int>)new List<int>() { 5, 6, 7, 8 });
            ICapturedVar capturedVar = capturedVarTyped;
            Console.WriteLine($"Captured var, actual type:       {capturedVar.ValueTypeString}");
            Console.WriteLine($"Captured var, declared type:     {capturedVar.TypeString}");
            Console.WriteLine($"Captured var, value:             {capturedVar.ValueObject.ToReadableString()}\n");
            declaredVariableType.Should().NotBeNull(because: "PRECOND: declared value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBeNull(because: "PRECOND: actual value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBe(declaredVariableType,
                because: "PRECOND: Declared variable type should be different than actual type of its value for this test to be relevant.");
            variable.Should().NotBeNull(because: "PRECOND: value of captured variable shoulld not be null for this test to be relevant.");
            // Assert:
            capturedVar.Should().NotBeNull(because: "CaptureVar(variable) should produce a valid captured variable object.");
            capturedVar.ValueObject.Should().NotBeNull(because: "captured variable's value is not null");

            capturedVar.Type.Should().Be(declaredVariableType, because: "Type property of captured variable should contain the declared variable type.");
            capturedVar.ValueType.Should().Be(actualVariableType, because: "ValueType property of captured variable should contain the actual type of variable value.");
        }

        [Fact]
        protected void CapturedVar_WorksCorrectlyWithCastValueNull()
        {
            // Arrange:
            IList<int> variable = null;
            Type actualVariableType = variable?.GetType();
            Type declaredVariableType = typeof(IList<int>);
            Console.WriteLine($"Test: capturing variable with static method CaptureVar(variable)...\n");
            Console.WriteLine($"Variable: actual type = {actualVariableType?.ToString()}, value = \n  {variable.ToReadableString()}\n");
            Console.WriteLine($"Actual type of variable's value: {actualVariableType?.ToString()}");
            Console.WriteLine($"Declared type of the variable:   {declaredVariableType.ToString()}\n");
            // Act:
            ICapturedVar<IList<int>> capturedVarTyped = new CapturedVar<IList<int>>((IList<int>)null);
            ICapturedVar capturedVar = capturedVarTyped;
            Console.WriteLine($"Captured var, actual type:       {capturedVar.ValueTypeString}");
            Console.WriteLine($"Captured var, declared type:     {capturedVar.TypeString}");
            Console.WriteLine($"Captured var, value:             {capturedVar.ValueObject.ToReadableString()}\n");
            declaredVariableType.Should().NotBeNull(because: "PRECOND: declared value type should not be null for this test to be relevant.");
            actualVariableType.Should().BeNull(because: "PRECOND: actual value type should not be null for this test to be relevant.");
            // Assert:
            capturedVar.Should().NotBeNull(because: "CaptureVar(variable) should produce a valid captured variable object.");
            capturedVar.ValueObject.Should().BeNull(because: "captured variable's value is null");

            capturedVar.Type.Should().Be(declaredVariableType, because: "Type property of captured variable should contain the declared variable type.");
            capturedVar.ValueType.Should().Be(actualVariableType, because: "ValueType property of captured variable should contain the actual type of variable value.");
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


