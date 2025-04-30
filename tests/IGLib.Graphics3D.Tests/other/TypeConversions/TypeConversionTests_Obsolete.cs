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

    /// <summary>Tests for type conversion utilities.</summary>
    public class TypeConversionTestsObsolete : TestBase<TypeConverterTests>
    {

        /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
        /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
        /// <param name="output">This parameter will be provided to constructor (injected via 
        /// constructor) by the test framework. I is also stored to Console property, such that
        /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
        /// method to generate test output.</param>
        public TypeConversionTestsObsolete(ITestOutputHelper output) : base(output)
        {  }


        #region TypeConversionHelper



        /// <summary>Like <see cref="TypeConversionHelper_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        /// but with target type and the type of restored variable both equal to type of the original variable, and also
        /// the expected restored value being equal to the original value.</summary>
        protected void TypeConversionHelper_ConversionToObjectAndBackTest<OriginalType>(
            OriginalType original, bool restoreObjectBackToValue = true)
        {
            TypeConversionHelper_ConversionToObjectAndBackTest<OriginalType, OriginalType, OriginalType>(
                original, original, restoreObjectBackToValue);
        }

        /// <summary>Like <see cref="TypeConversionHelper_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        /// but with the type of restored variable equal to type of the original variable.</summary>
        protected void TypeConversionHelper_ConversionToObjectAndBackTest<OriginalType, TargetType>(
            OriginalType original, OriginalType expectedRestoredValue, bool restoreObjectBackToValue = true)
        {
            TypeConversionHelper_ConversionToObjectAndBackTest<OriginalType, TargetType, OriginalType>(
                original, expectedRestoredValue, restoreObjectBackToValue);
        }


        /// <summary>Like <see cref="TypeConversionHelper_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        /// but with the type of restored variable equal to type of the original variable, and also with expected
        /// restored value equal to the original value.</summary>
        protected void TypeConversionHelper_ConversionToObjectAndBackTest<OriginalType, TargetType>(
            OriginalType original, bool restoreObjectBackToValue = true)
        {
            TypeConversionHelper_ConversionToObjectAndBackTest<OriginalType, TargetType, OriginalType>(
                original, original, restoreObjectBackToValue);
        }


        /// <summary>Performs test of conversion via <see cref="TypeConversionHelper"/> from a value of type
        /// <typeparamref name="OriginalType"/> to an object variable of target type <typeparamref name="TargetType"/>
        /// and back to value of type <typeparamref name="RestoredType"/>.</summary>
        /// <param name="originalValue">Original value that is converted to object.</param>
        /// <param name="expectedRestoredValue">Expected restored value after conversion of original to object and restoring back to original.</param>
        /// <param name="restoreObjectBackToValue">If true (which is default) then object is also restored back to a value of type <typeparamref name="RestoredType"/>.</param>
        protected void TypeConversionHelper_ConversionToObjectAndBackTest<OriginalType, TargetType, RestoredType>(
            OriginalType originalValue, RestoredType expectedRestoredValue, bool restoreObjectBackToValue = true)
        {
            // Arrange
            Type targetType = typeof(TargetType);
            // int original = 4353;
            RestoredType restored;
            TypeConversionHelper typeConverter = new TypeConversionHelper();
            Console.WriteLine($"Converting value of type {originalValue.GetType().Name}, value = {originalValue}. to object, and storing the object.");
            // Act
            object assignedObject = typeConverter.ConvertToType(originalValue, targetType);
            if (assignedObject == null)
            {
                Console.WriteLine("Warning: Converted object is null.");
            }
            else
            {
                Console.WriteLine($"Converted object is of type {assignedObject.GetType().Name}, value: {assignedObject}");
            }
            // Assert
            assignedObject.Should().NotBeNull(because: $"Value of type {originalValue.GetType().Name} value can be convertet to object of type {targetType.Name}.");
            assignedObject.GetType().Should().Be(targetType, because: $"Type of the assigned object should mach the target typ {targetType.Name}.");
            if (restoreObjectBackToValue)
            {
                // restored = (RestoredType)assignedObject;
                restored = (RestoredType)typeConverter.ConvertToType(assignedObject, typeof(RestoredType));
                if (restored == null)
                {
                    Console.WriteLine("WARNING: Restored value is null.");
                }
                else
                {
                    Console.WriteLine($"Value of type {restored.GetType().Name} restored from the object: {restored}");
                }
                restored.Should().Be(expectedRestoredValue, because: $"Restoring object that hods {targetType.Name} should correctly reproduce the original value of type {originalValue.GetType().Name}.");
            }
        }

        // Conversons from int:

        [Fact]
        public void TypeConversionHelper_RoundTripConversion_IntToIntObjectToInt_IsCorrect()
        {
            TypeConversionHelper_ConversionToObjectAndBackTest<int>(45);
        }

        [Fact]
        public void TypeConversionHelper_RoundTripConversion_IntToDoubleObjectDouble_IsCorrect()
        {
            TypeConversionHelper_ConversionToObjectAndBackTest<int, double, double>(45, 45.0);
        }

        [Fact]
        public void TypeConversionHelper_RoundTripConversion_IntToDoubleObjectToInt_IsCorrect()
        {
            TypeConversionHelper_ConversionToObjectAndBackTest<int, double>(45);
        }

        // Conversion from double:

        [Fact]
        public void TypeConversionHelper_RoundTripConversion_DoubleToDoubleObjectToDouble_IsCorrect()
        {
            TypeConversionHelper_ConversionToObjectAndBackTest<double>(6.4);
        }

        [Fact]
        public void TypeConversionHelper_RoundTripConversion_IntegerDoubleToIntObjectToInt_IsCorrect()
        {
            TypeConversionHelper_ConversionToObjectAndBackTest<double, int, int>(6.0, 6);
        }

        [Fact]
        public void TypeConversionHelper_RoundTripConversion_NonintegerDoubleToIntObjectToInt_IsCorrect()
        {
            TypeConversionHelper_ConversionToObjectAndBackTest<double, int, int>(6.1, 6);
            TypeConversionHelper_ConversionToObjectAndBackTest<double, int, int>(6.9, 7);
            TypeConversionHelper_ConversionToObjectAndBackTest<double, int, int>(6.5, 6);
        }

        [Fact]
        public void TypeConversionHelper_RoundTripConversion_OwerflowDoubleToIntObjectToInt_IsCorrect()
        {
            try
            {
                Exception exception = Assert.Throws<InvalidOperationException>(() =>
                    TypeConversionHelper_ConversionToObjectAndBackTest<double, int, int>(1.0e22, 6)
                    );
                Console.WriteLine($"Exception type: {exception.GetType().Name}, message: {exception.Message}");
                if (exception.InnerException != null)
                {
                    Console.WriteLine("Inner exception is null.");
                }
                else
                {
                    Console.WriteLine($"Inner exception type: {exception.InnerException.GetType().Name}, message: {exception.InnerException.Message}");
                }
                // exception.InnerException.Should().NotBeNull();
                //exception.InnerException.GetType().Should().Be(typeof());
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Assert.Throws has thrown an exception, type = {ex.GetType().Name}, message: {ex.Message}.");
                throw;
            }
        }

        [Fact]
        public void TypeConversionHelper_RoundTripConversion_NonintegerDoubleToIntObjectToDouble_IsCorrect()
        {
            TypeConversionHelper_ConversionToObjectAndBackTest<double, int, double>(6.1, 6.0);
            TypeConversionHelper_ConversionToObjectAndBackTest<double, int, double>(6.9, 7.0);
            TypeConversionHelper_ConversionToObjectAndBackTest<double, int, double>(6.5, 6.0);
        }








        #endregion TypeConversionHelper




        #region Examples



        #endregion Examples


    }

}


