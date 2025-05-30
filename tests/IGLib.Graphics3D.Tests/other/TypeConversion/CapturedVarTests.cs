// Uncomment the definition below in order to include tests that fail at this hierarchy level by design!
// #define IncludeFailedTestsByDesign

using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using IGLib.Tests.Base;
using IGLib.Core;
using IGLib.Core.CollectionExtensions;
using static IGLib.Core.CapturedVar;


namespace IGLib.Core.Tests
{



    /// <summary>Tests of the <see cref="CapturedVar"/> class and related types and utiliities.</summary>
    public class CapturedVarTests : TypeConverterTestsBase<CapturedVarTests>
    {

        /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
        /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
        /// <param name="output">This parameter will be provided to constructor (injected via 
        /// constructor) by the test framework. I is also stored to Console property, such that
        /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
        /// method to generate test output.</param>
        public CapturedVarTests(ITestOutputHelper output) : base(output)
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
            ICapturedVar<IList<int>> capturedVarTyped = Capture(variable);
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
            ICapturedVar<IList<int>> capturedVarTyped = Capture(variable);
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
            ICapturedVar<IList<int>> capturedVarTyped = Capture((IList<int>)new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 });
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
            ICapturedVar<IList<int>> capturedVarTyped = Capture((IList<int>) null);
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
        protected void CapturedVarCreation_WorksCorrectlyWithoutStatingVariableTye()
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
        protected void CapturedVarCreation_PropertiesShouldBeCorrect()
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
        protected void CapturedVarCreation_WorksCorrectlyWithCastValue()
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
        protected void CapturedVarCreation_WorksCorrectlyWithCastValueNull()
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

        [Fact]
        protected void CapturedVarCreation_ToStringLong_WorksCorrectly()
        {
            // Arrange:
            IList<int> variable = new List<int>() { 1, 2, 5, 6, 7 };
            Type actualVariableType = variable?.GetType();
            Type declaredVariableType = typeof(IList<int>);
            string indentationString = "\t···";  // 5 middle dots
            Console.WriteLine($"Test: checking {nameof(CapturedVar)}.{nameof(ICapturedVar.ToStringLong)}...\n");
            Console.WriteLine($"Variable: actual type = {variable?.GetType()?.ToString()}, value = \n  {variable.ToReadableString()}\n");
            Console.WriteLine($"Actual type of variable's value: {actualVariableType?.ToString()}");
            Console.WriteLine($"Declared type of the variable:   {declaredVariableType.ToString()}\n");
            // Act:
            ICapturedVar<IList<int>> capturedVarTyped = new CapturedVar<IList<int>>(variable);
            ICapturedVar capturedVar = capturedVarTyped;
            string capturedVarString = capturedVar.ToStringLong(indentationString);
            Console.WriteLine($"Captured var, actual type:       {capturedVar.ValueTypeString}");
            Console.WriteLine($"Captured var, declared type:     {capturedVar.TypeString}");
            Console.WriteLine($"Captured var, value:             {capturedVar.ValueObject.ToReadableString()}\n");
            Console.WriteLine($"\nIndentation string used: \n\"{indentationString}\"");
            Console.WriteLine($"String representation produced by {nameof(ICapturedVar.ToStringLong)}():\n{capturedVarString}\n");

            declaredVariableType.Should().NotBeNull(because: "PRECOND: declared value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBeNull(because: "PRECOND: actual value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBe(declaredVariableType,
                because: "PRECOND: Declared variable type should be different than actual type of its value for this test to be relevant.");
            variable.Should().NotBeNull(because: "PRECOND: value of captured variable shoulld not be null for this test to be relevant.");
            capturedVar.Type.Should().Be(declaredVariableType,
                because: "PRECOND: Type property of captured variable should contain the declared variable type.");
            capturedVar.ValueType.Should().Be(actualVariableType,
                because: "PRECOND: ValueType property of captured variable should contain the actual type of variable's value.");

            // Assert:
            capturedVarString.Should().NotBeNullOrWhiteSpace(because: "long string representation must have some contentt as the object is not null");
            capturedVarString.Should().Contain(indentationString, because: "since indentation string was specified, it should appear in string form");

            // Declared type of the captured entity:
            capturedVarString.Should().Contain(nameof(CapturedVarObject.Type), 
                because: "produced string must contain the proper label for the declared type of the captured variable");
            
            capturedVarString.Should().Contain(indentationString + nameof(CapturedVarObject.TypeName), 
                because: "produced string must contain the proper label for short name of the declared type of the captured variable");
            capturedVarString.Should().Contain(declaredVariableType.Name,
                because: $"produced string must contain short name of declared type of captured variable produced by {nameof(Type)}.{nameof(Type.Name)}");
            
            capturedVarString.Should().Contain(indentationString + nameof(CapturedVarObject.TypeFullName), 
                because: "produced string must contain the proper label for full name of the declared type of the captured variable");
            capturedVarString.Should().Contain(declaredVariableType.FullName,
                because: $"produced string must contain full name of declared type of captured variable produced by {nameof(Type)}.{nameof(Type.FullName)}");
            
            capturedVarString.Should().Contain(indentationString + nameof(CapturedVarObject.TypeString), 
                because: "produced string must contain the proper label for human readable name of the declared type of the captured variable");
            capturedVarString.Should().Contain(declaredVariableType.ToString(),
                because: $"produced string must contain declared type of captured variable produced by {nameof(Type)}.{nameof(Type.ToString)}()");

            // Actual type of the captured entity's value:
            capturedVarString.Should().Contain(nameof(CapturedVarObject.ValueType), 
                because: "produced string must contain the proper label for the actual type of the captured value");
            
            capturedVarString.Should().Contain(indentationString + nameof(CapturedVarObject.ValueTypeName), 
                because: "produced string must contain the proper label for short name of the actual type of the captured value");
            capturedVarString.Should().Contain(actualVariableType.Name,
                because: $"produced string must contain short name of actual type of captured value produced by {nameof(Type)}.{nameof(Type.Name)}");
            
            capturedVarString.Should().Contain(indentationString + nameof(CapturedVarObject.ValueTypeFullName), 
                because: "produced string must contain the proper label for full name of the actual type of the captured value");
            capturedVarString.Should().Contain(actualVariableType.FullName,
                because: $"produced string must contain full name of actual type of captured valur produced by {nameof(Type)}.{nameof(Type.FullName)}");
            
            capturedVarString.Should().Contain(indentationString + nameof(CapturedVarObject.ValueTypeString), 
                because: "produced string must contain the proper label for human readable name of the actual type of the captured value");
            capturedVarString.Should().Contain(actualVariableType.ToString(),
                because: $"produced string must contain actual type of captured value produced by {nameof(Type)}.{nameof(Type.ToString)}()");

            // Captured value (value of the captred entity at the time of capture):
            capturedVarString.Should().Contain(nameof(CapturedVarObject.Value),
                because: "produced string must contain the proper label for the captured value of the entity");
            // remark:
            // expected value below should strictly be capturedVarTyped.ToReadableString(), but this shoulf be equivalent
            // to variable.ToReadableString()
            capturedVarString.Should().Contain(variable.ToReadableString(),
                because: $"produced string must contain a readable string describing the captured value");
            // Also verify that the string contains all elements of the list that was captured:
            for (int i = 0; i < variable.Count; ++i)
            {
                capturedVarString.Should().Contain(variable[i].ToString(),
                    because: "in case of list of integers, produced string should contin string representations of all elements");
            }
        }


        [Fact]
        protected void CapturedVarCreation_ToString_WorksCorrectly()
        {
            // Arrange:
            IList<int> variable = new List<int>() { 1, 2, 3, 6, 7, 8 };
            Type actualVariableType = variable?.GetType();
            Type declaredVariableType = typeof(IList<int>);
            Console.WriteLine($"Test: checking {nameof(CapturedVar)}.{nameof(ICapturedVar.ToString)}()...\n");
            Console.WriteLine($"Variable: actual type = {variable?.GetType()?.ToString()}, value = \n  {variable.ToReadableString()}\n");
            Console.WriteLine($"Actual type of variable's value: {actualVariableType?.ToString()}");
            Console.WriteLine($"Declared type of the variable:   {declaredVariableType.ToString()}\n");
            // Act:
            ICapturedVar<IList<int>> capturedVarTyped = new CapturedVar<IList<int>>(variable);
            ICapturedVar capturedVar = capturedVarTyped;
            string capturedVarString = capturedVar.ToString();
            Console.WriteLine($"Captured var, actual type:       {capturedVar.ValueTypeString}");
            Console.WriteLine($"Captured var, declared type:     {capturedVar.TypeString}");
            Console.WriteLine($"Captured var, value:             {capturedVar.ValueObject.ToReadableString()}\n");
            Console.WriteLine($"Captured var, full representation:\n{capturedVar.ToStringLong()}");
            Console.WriteLine($"\nString representation produced by {nameof(ICapturedVar.ToString)}():\n{capturedVarString}\n");

            declaredVariableType.Should().NotBeNull(because: "PRECOND: declared value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBeNull(because: "PRECOND: actual value type should not be null for this test to be relevant.");
            actualVariableType.Should().NotBe(declaredVariableType,
                because: "PRECOND: Declared variable type should be different than actual type of its value for this test to be relevant.");
            variable.Should().NotBeNull(because: "PRECOND: value of captured variable shoulld not be null for this test to be relevant.");
            capturedVar.Type.Should().Be(declaredVariableType,
                because: "PRECOND: Type property of captured variable should contain the declared variable type.");
            capturedVar.ValueType.Should().Be(actualVariableType,
                because: "PRECOND: ValueType property of captured variable should contain the actual type of variable's value.");

            // Assert:
            capturedVarString.Should().NotBeNullOrWhiteSpace(because: "long string representation must have some contentt as the object is not null");

            capturedVarString.Should().StartWith("Captured entity: ",
                because: "string representation should begin with agreed label");

            capturedVarString.Should().Contain("declared type = ",
                because: $"string representation should contain the declared type of the captured entity introduced by the agreed label");
            capturedVarString.Should().Contain(declaredVariableType.ToString(),
                because: $"string representation should contain correct declared type of the captred entity, produced by {nameof(Type)}.{nameof(Type.ToString)}()");

            capturedVarString.Should().Contain("actual type = ",
                because: $"string representation should contain the actual type of the captured entity;s value introduced by the agreed label");
            capturedVarString.Should().Contain(actualVariableType.ToString(),
                because: $"string representation should contain correct actual type of the captred value, produced by {nameof(Type)}.{nameof(Type.ToString)}()");

            // Captured value (value of the captred entity at the time of capture):
            capturedVarString.Should().Contain("value = ",
                because: "produced string should contain the agreed label for the captured value of the entity");
            // remark:
            // expected value below should strictly be capturedVarTyped.ToReadableString(), but this shoulf be equivalent
            // to variable.ToReadableString()
            // Verify that the string contains all elements of the list that was captured:
            for (int i = 0; i < variable.Count; ++i)
            {
                capturedVarString.Should().Contain(variable[i].ToString(),
                    because: "in case of list of integers, produced string should contin string representations of all elements");
            }
            // Also verify precise expected representation of the value (if this changes intentionally, the check below may 
            // need to be corrected):
            capturedVarString.Should().Contain(variable.ToReadableString(),
                because: $"produced string must contain a readable string describing the captured value");
        }


        #endregion CapturedVarTests

    }

}


