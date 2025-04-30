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
    public class BasicTypeConverterTests : TypeConverterTestsBase<BasicTypeConverterTests>
    {

        /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
        /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
        /// <param name="output">This parameter will be provided to constructor (injected via 
        /// constructor) by the test framework. I is also stored to Console property, such that
        /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
        /// method to generate test output.</param>
        public BasicTypeConverterTests(ITestOutputHelper output) : base(output)
        {  }


        //#region GenericConversionTests


        ///// <summary>Like <see cref="TypeConverter_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        ///// but with target type and the type of restored variable both equal to type of the original variable, and also
        ///// the expected restored value being equal to the original value.</summary>
        //protected void TypeConverter_ConversionToObjectAndBackTest<OriginalType>(ITypeConverter typeConverter,
        //    OriginalType original, bool restoreObjectBackToValue = true)
        //{
        //    TypeConverter_ConversionToObjectAndBackTest<OriginalType, OriginalType, OriginalType>(typeConverter,
        //        original, original, restoreObjectBackToValue);
        //}

        ///// <summary>Like <see cref="TypeConverter_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        ///// but with the type of restored variable equal to type of the original variable.</summary>
        //protected void TypeConverter_ConversionToObjectAndBackTest<OriginalType, TargetType>(ITypeConverter typeConverter,
        //    OriginalType original, OriginalType expectedRestoredValue, bool restoreObjectBackToValue = true)
        //{
        //    TypeConverter_ConversionToObjectAndBackTest<OriginalType, TargetType, OriginalType>(typeConverter,
        //        original, expectedRestoredValue, restoreObjectBackToValue);
        //}


        ///// <summary>Like <see cref="TypeConverter_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        ///// but with the type of restored variable equal to type of the original variable, and also with expected
        ///// restored value equal to the original value.</summary>
        //protected void TypeConverter_ConversionToObjectAndBackTest<OriginalType, TargetType>(ITypeConverter typeConverter,
        //    OriginalType original, bool restoreObjectBackToValue = true)
        //{
        //    TypeConverter_ConversionToObjectAndBackTest<OriginalType, TargetType, OriginalType>(typeConverter,
        //        original, original, restoreObjectBackToValue);
        //}


        ///// <summary>Performs test of conversion via <see cref="TypeConversionHelper"/> from a value of type
        ///// <typeparamref name="OriginalType"/> to an object variable of target type <typeparamref name="TargetType"/>
        ///// and back to value of type <typeparamref name="RestoredType"/>.</summary>
        ///// <param name="originalValue">Original value that is converted to object.</param>
        ///// <param name="expectedRestoredValue">Expected restored value after conversion of original to object and restoring back to original.</param>
        ///// <param name="restoreObjectBackToValue">If true (which is default) then object is also restored back to a value of type <typeparamref name="RestoredType"/>.</param>
        //protected void TypeConverter_ConversionToObjectAndBackTest<OriginalType, TargetType, RestoredType>(ITypeConverter typeConverter,
        //    OriginalType originalValue, RestoredType expectedRestoredValue, bool restoreObjectBackToValue = true)
        //{
        //    // Arrange
        //    Type targetType = typeof(TargetType);
        //    // int original = 4353;
        //    RestoredType restored;
        //    Console.WriteLine($"Converting value of type {originalValue.GetType().Name}, value = {originalValue}. to object, and storing the object.");
        //    // Act
        //    object assignedObject = typeConverter.ConvertToType(originalValue, targetType);
        //    if (assignedObject == null)
        //    {
        //        Console.WriteLine("Warning: Converted object is null.");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Converted object is of type {assignedObject.GetType().Name}, value: {assignedObject}");
        //    }
        //    // Assert
        //    assignedObject.Should().NotBeNull(because: $"Value of type {originalValue.GetType().Name} value can be convertet to object of type {targetType.Name}.");
        //    assignedObject.GetType().Should().Be(targetType, because: $"Type of the assigned object should mach the target typ {targetType.Name}.");
        //    if (restoreObjectBackToValue)
        //    {
        //        // restored = (RestoredType)assignedObject;
        //        restored = (RestoredType)typeConverter.ConvertToType(assignedObject, typeof(RestoredType));
        //        if (restored == null)
        //        {
        //            Console.WriteLine("WARNING: Restored value is null.");
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Value of type {restored.GetType().Name} restored from the object: {restored}");
        //        }
        //        restored.Should().Be(expectedRestoredValue, because: $"Restoring object that hods {targetType.Name} should correctly reproduce the original value of type {originalValue.GetType().Name}.");
        //    }
        //}


        //#endregion GenericConversionTests


        //#region GenericSpeedTests



        //#endregion GenercSpeedTests



        #region BasicTypeConverterTests

        ITypeConverter BasicTypeConverter { get; } = new BasicTypeConverter();



        // Conversons from int:

        [Fact]
        public void BasicTypeConverter_RoundTripConversion_IntToIntObjectToInt_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<int>(BasicTypeConverter, 45);
        }

        [Fact]
        public void TypeConverter_RoundTripConversion_IntToDoubleObjectDouble_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<int, double, double>(BasicTypeConverter, 45, 45.0);
        }

        [Fact]
        public void TypeConverter_RoundTripConversion_IntToDoubleObjectToInt_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<int, double>(BasicTypeConverter, 45);
        }

        // Conversion from double:

        [Fact]
        public void TypeConverter_RoundTripConversion_DoubleToDoubleObjectToDouble_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<double>(BasicTypeConverter, 6.4);
        }

        [Fact]
        public void TypeConverter_RoundTripConversion_IntegerDoubleToIntObjectToInt_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<double, int, int>(BasicTypeConverter, 6.0, 6);
        }

        [Fact]
        public void TypeConverter_RoundTripConversion_NonintegerDoubleToIntObjectToInt_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<double, int, int>(BasicTypeConverter, 6.1, 6);
            TypeConverter_ConversionToObjectAndBackTest<double, int, int>(BasicTypeConverter, 6.9, 7);
            TypeConverter_ConversionToObjectAndBackTest<double, int, int>(BasicTypeConverter, 6.5, 6);
        }

        [Fact]
        public void TypeConverter_RoundTripConversion_OwerflowDoubleToIntObjectToInt_IsCorrect()
        {
            try
            {
                Exception exception = Assert.Throws<InvalidOperationException>(() =>
                    TypeConverter_ConversionToObjectAndBackTest<double, int, int>(BasicTypeConverter, 1.0e22, 6)
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
        public void TypeConverter_RoundTripConversion_NonintegerDoubleToIntObjectToDouble_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<double, int, double>(BasicTypeConverter, 6.1, 6.0);
            TypeConverter_ConversionToObjectAndBackTest<double, int, double>(BasicTypeConverter, 6.9, 7.0);
            TypeConverter_ConversionToObjectAndBackTest<double, int, double>(BasicTypeConverter, 6.5, 6.0);
        }


        #endregion BasicTypeConverterTests




        #region BasicTypeConverterSpeedTests




        #endregion BasicTypeConverterSpeedTests



        #region Examples



        #endregion Examples


    }

}


