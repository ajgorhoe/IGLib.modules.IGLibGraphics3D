using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using IGLib.Tests.Base;
using System.Linq;
using IGLib.Core;


namespace IGLib.Core.Tests
{

    /// <summary>This class contain tests for model parameter objects (which implement the
    /// <see cref="IModelParameter"/> interface).</summary>
    public class ModelParameterTests : TestBase<ModelParameterTests>
    {

        /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
        /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
        /// <param name="output">This parameter will be provided to constructor (injected via 
        /// constructor) by the test framework. I is also stored to Console property, such that
        /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
        /// method to generate test output.</param>
        public ModelParameterTests(ITestOutputHelper output) : base(output)
        {  }


        #region ModelParameter

        #region ModelParameter.Creation

        [Fact]
        public void ModelParameter_CreationWithNameAndType_SuccessfullyCreated()
        {
            // Arrange
            string name = "TestParameter";
            Type type = typeof(double);
            // Act
            ModelParameter parameterObject = new(name, type);
            // Assert
            parameterObject.Should().NotBeNull();
        }

        [Fact]
        public void ModelParameter_CreationWithNameAndType_NameAndTypeAreCorrect()
        {
            // Arrange
            string name = "TestParameter";
            Type type = typeof(double);
            // Act
            ModelParameter parameterObject = new(name, type);
            // Assert
            parameterObject.Should().NotBeNull(because: "Precond: object was created.");
            parameterObject.Name.Should().Be(name);
            parameterObject.Type.Should().Be(type);
        }

        [Fact]
        public void ModelParameter_CreationWithNameAndType_ShouldNotBeConstant()
        {
            // Arrange
            string name = "TestParameter";
            Type type = typeof(double);
            // Act
            ModelParameter parameterObject = new(name, type);
            // Assert
            parameterObject.Should().NotBeNull(because: "Precond: object was created.");
            parameterObject.IsConstant.Should().BeFalse();
        }

        [Fact]
        public void ModelParameter_CreationWithNameAndType_DefaultValueShouldNotBeDefined()
        {
            // Arrange
            string name = "TestParameter";
            Type type = typeof(double);
            // Act
            ModelParameter parameterObject = new(name, type);
            // Assert
            parameterObject.Should().NotBeNull(because: "Precond: object was created.");
            parameterObject.IsDefaultValueDefined.Should().BeFalse();
        }

        [Fact]
        public void ModelParameter_CreationWithNameAndType_ValueShouldNotBeDefined()
        {
            // Arrange
            string name = "TestParameter";
            Type type = typeof(double);
            // Act
            ModelParameter parameterObject = new(name, type);
            // Assert
            parameterObject.Should().NotBeNull(because: "Precond: object was created.");
            parameterObject.IsValueDefined.Should().BeFalse();
        }

        [Fact]
        public void ModelParameter_CreationWithNameAndType_ValueObjectThrowsCorrectException()
        {
            // Arrange
            string name = "TestParameter";
            Type type = typeof(double);
            ModelParameter parameterObject = new(name, type);
            // Act & Assert
            parameterObject.Should().NotBeNull(because: "Precond: object was created.");
            parameterObject.IsValueDefined.Should().BeFalse(because: "Precond: since value was not set, IsValueDefined should be false.");
            var exception = Assert.Throws<InvalidOperationException>(() => parameterObject.ValueObject);            
            exception.Message.Should().Contain(name, because: "Exception message should contain parameter name.");
        }

        [Fact]
        public void ModelParameter_CreationWithNameAndType_DefaultValueObjectThrowsCorrectException()
        {
            // Arrange
            string name = "TestParameter";
            Type type = typeof(double);
            ModelParameter parameterObject = new(name, type);
            // Act & Assert
            parameterObject.Should().NotBeNull(because: "Precond: object was created.");
            parameterObject.IsValueDefined.Should().BeFalse(because: "Precond: since value was not set, IsValueDefined should be false.");
            var exception = Assert.Throws<InvalidOperationException>(() => parameterObject.DefaultValueObject);
            exception.Message.Should().Contain(name, because: "Exception message should contain parameter name.");
        }

        [Fact]
        public void ModelParameter_CreationWithTitle_TitleIsDefinedAndContinsName()
        {
            // Arrange
            string name = "TestParameter";
            Type type = typeof(double);
            ModelParameter parameterObject = new(name, type);
            // Act & Assert
            parameterObject.Should().NotBeNull(because: "Precond: object was created.");
            parameterObject.Title.Should().NotBeNull(because: "When title was not specified, default should be set.");
            parameterObject.Title.Should().Contain(name, because: "When title was not specified, default title should be set and should contain parameter name.");
        }

        [Fact]
        public void ModelParameter_CreationWithDescruotion_DescriptionIsDefinedAndContinsNameAndType()
        {
            // Arrange
            string name = "TestParameter";
            Type type = typeof(double);
            ModelParameter parameterObject = new(name, type);
            // Act & Assert
            parameterObject.Should().NotBeNull(because: "Precond: object was created.");
            parameterObject.Description.Should().NotBeNull(because: "When title was not specified, default should be set.");
            parameterObject.Description.Should().Contain(name, because: "When title was not specified, default title should be set and should contain parameter name.");
            parameterObject.Description.Should().Contain(type.Name, because: "When title was not specified, default title should be set and should also contain parameter type.");
        }

        [Fact]
        public void ModelParameter_CreationWithTitleSpecified_TitleIsCorrect()
        {
            // Arrange
            string name = "TestParameter";
            Type type = typeof(double);
            string parameterTitle = "Parameter Title";
            ModelParameter parameterObject = new(name, type, title: parameterTitle);
            // Act & Assert
            parameterObject.Should().NotBeNull(because: "Precond: object was created.");
            parameterObject.Title.Should().Be(parameterTitle, because: "Parameter's title should be as specified when creating the object.");
        }

        [Fact]
        public void ModelParameter_CreationWithDescriptionSpecified_DescriptionIsCorrect()
        {
            // Arrange
            string name = "TestParameter";
            Type type = typeof(double);
            string parameterDescription = "This is a test parameter.";
            // Act
            ModelParameter parameterObject = new(name, type, title: null, description: parameterDescription);
            // Assert
            parameterObject.Should().NotBeNull(because: "Precond: object was created.");
            parameterObject.Description.Should().Be(parameterDescription, because: "Parameter's description should be as specified when creating the object.");
        }

        #endregion ModelParameter.Creation

        

        #endregion ModelParameter




        #region ModelParameterTyped



        #endregion ModelParameterTyped



        #region Examples



        #endregion Examples


    }

}


