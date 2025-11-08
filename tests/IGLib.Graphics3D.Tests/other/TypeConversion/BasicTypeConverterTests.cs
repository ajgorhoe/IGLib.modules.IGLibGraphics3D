
#nullable disable

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
using IGLib.Tests.Base.SampleClasses;

namespace IGLib.Core.Tests
{

    /// <summary>Tests of the basic type converter (<see cref="BasicTypeConverter"/>,
    /// implementation of the <see cref="ITypeConverter"/> interface).</summary>
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


        /// <summary>The type converter that is under test.</summary>
        protected virtual ITypeConverter TypeConverter { get; } = new BasicTypeConverter();

        /// <summary>Number of executions in speed tests.</summary>
        protected virtual int NumExecutions { get; } = 1_000;


        #region BasicTypeConverterTests

        /// <summary>Minimum required number of executions per second in speed tests.</summary>
        protected virtual double MinPerSecond { get; } = 10_000;

        // Conversons from int:

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_IntToIntObjectToInt_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<int>(TypeConverter, 45);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_IntToDoubleObjectToDouble_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<int, double, double>(TypeConverter, 45, 45.0, 45.0);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_IntToDoubleObjectToInt_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<int, double, int>(TypeConverter, 45, 45.0, 45);
        }

        // Conversion from double:

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_DoubleToDoubleObjectToDouble_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<double>(TypeConverter, 6.4);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_IntegerDoubleToIntObjectToInt_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<double, int, int>(TypeConverter, 6.0, 6, 6);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_NonintegerDoubleToIntObjectToInt_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<double, int, int>(TypeConverter, 6.1, 6, 6);
            TypeConverter_ConversionToObjectAndBackTest<double, int, int>(TypeConverter, 6.9, 7, 7);
            TypeConverter_ConversionToObjectAndBackTest<double, int, int>(TypeConverter, 6.5, 6, 6);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_OwerflowDoubleToIntObjectToInt_IsCorrect()
        {
            try
            {
                Exception exception = Assert.Throws<InvalidOperationException>(() =>
                    TypeConverter_ConversionToObjectAndBackTest<double, int, int>(TypeConverter, 1.0e22, 6, 6)
                    );
                Console.WriteLine($"Exception type: {exception.GetType().Name}, message: {exception.Message}");
                if (exception.InnerException == null)
                {
                    Console.WriteLine("Inner exception is null.");
                }
                else
                {
                    Console.WriteLine($"Inner exception type: {exception.InnerException.GetType().Name}, message: {exception.InnerException.Message}");
                }
                exception.InnerException.Should().NotBeNull();
                exception.InnerException.GetType().Should().Be(typeof(OverflowException));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Assert.Throws has thrown an exception, type = {ex.GetType().Name}, message: {ex.Message}.");
                throw;
            }
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_NonintegerDoubleToIntObjectToDouble_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<double, int, double>(TypeConverter, 6.1, 6, 6.0);
            TypeConverter_ConversionToObjectAndBackTest<double, int, double>(TypeConverter, 6.9, 7, 7.0);
            TypeConverter_ConversionToObjectAndBackTest<double, int, double>(TypeConverter, 6.5, 6,6.0);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_DoubleToStringObjectToDouble_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<double, string, double>(TypeConverter, 45.6, "45.6", 45.6);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_StringToDoubleObjectToString_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<string, double, string>(TypeConverter, 
                "24.6", 24.6, "24.6");
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_StringWithUnderscoresToDoubleObjectToDouble_IsCorrect()
        {

            try
            {
                Exception exception = Assert.Throws<InvalidOperationException>(() =>
                    TypeConverter_ConversionToObjectAndBackTest<string, double, double>(TypeConverter,
                        "123_456.55e-16", 123_456.55e-16, 123_456.55e-16)
                    );
                Console.WriteLine($"Exception type: {exception.GetType().Name}, message: {exception.Message}");
                if (exception.InnerException == null)
                {
                    Console.WriteLine("Inner exception is null.");
                }
                else
                {
                    Console.WriteLine($"Inner exception type: {exception.InnerException.GetType().Name}, message: {exception.InnerException.Message}");
                }
                exception.InnerException.Should().NotBeNull();
                exception.InnerException.GetType().Should().Be(typeof(FormatException));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Assert.Throws has thrown an exception, type = {ex.GetType().Name}, message: {ex.Message}.");
                throw;
            }
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_DoubleStringToDoubleObjectToInt_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<string, double, int>(TypeConverter, "12.21", 12.21, 12);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_DoubleToStringObjectToString_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<double, string, string>(TypeConverter, 18.66, "18.66", "18.66");
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_ClassToClassObjectToClass_IsCorrect()
        {
            TypeConverter_ConversionToObjectAndBackTest<DerivedClass>(TypeConverter, 
                new DerivedClass());
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_DerivedClassToBaseClassObjectToBaseClass_IsCorrect()
        {
            DerivedClass originalObject = new DerivedClass();
            BaseClass expectedAssignedObject = originalObject;
            BaseClass expectedRestoredValue = originalObject;
            TypeConverter_ConversionToObjectAndBackTest<DerivedClass, BaseClass, BaseClass>(TypeConverter, 
                originalObject, expectedAssignedObject, expectedRestoredValue);
        }


        #endregion BasicTypeConverterTests




        #region SpeedTests.TypeConversion

        [Fact]
        protected virtual void SpecificTypeConverter_Speed_RoundTripConversion_IntToIntObjectToInt_IsCorrect()
        {
            TypeConverter_Speed_ConversionToObjectAndBackTest<int>(TypeConverter, NumExecutions, MinPerSecond,
                45, doOutputInReferenceTest: false);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_Speed_RoundTripConversion_IntToDoubleObjectToInt_IsCorrect()
        {
            TypeConverter_Speed_ConversionToObjectAndBackTest<int, double, int>(TypeConverter, NumExecutions, MinPerSecond, 
                45, 45.0, 45, doOutputInReferenceTest: false);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_Speed_RoundTripConversion_DoubleToIntObjectToDouble_IsCorrect()
        {
            TypeConverter_Speed_ConversionToObjectAndBackTest<double, int, double>(TypeConverter, NumExecutions, MinPerSecond, 
                26.9, 27, 27.0, doOutputInReferenceTest: false);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_Speed_RoundTripConversion_DoubleToStringObjectToDouble_IsCorrect()
        {
            TypeConverter_Speed_ConversionToObjectAndBackTest<double, string, double>(TypeConverter, NumExecutions, MinPerSecond, 
                98.4, "98.4", 98.4, doOutputInReferenceTest: false);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_Speed_RoundTripConversion_DerivedClassToBaseClassObjectToBaseClass_IsCorrect()
        {
            DerivedClass originalObject = new DerivedClass();
            BaseClass expectedAssignedObject = originalObject;
            BaseClass expectedRestoredValue = originalObject;
            TypeConverter_Speed_ConversionToObjectAndBackTest<DerivedClass, BaseClass, BaseClass>(
                TypeConverter, NumExecutions, MinPerSecond,
                originalObject, expectedAssignedObject, expectedRestoredValue, doOutputInReferenceTest: false);
        }


        #endregion SpeedTests.TypeConversion



        #region BasicTypeConverterSpeedTests




        #endregion BasicTypeConverterSpeedTests



        #region Examples



        #endregion Examples


    }

}


