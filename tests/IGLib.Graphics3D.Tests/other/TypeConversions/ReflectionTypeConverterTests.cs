//Remove the definition below when if directives are removed and the tests permanently remain here!
#define IncludeFailedTestsByDesign

using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IGLib.Tests.Base.SampleClasses;
using static IGLib.Tests.Base.SampleCollsctions.SampleCollections;
using IGLib.Core;
using IGLib.CoreExtended;
using IGLib.Core.CollectionExtensions;

namespace IGLib.Core.Tests
{


    /// <summary>Tests of the basic type converter (<see cref="BasicTypeConverter"/>,
    /// implementation of the <see cref="ITypeConverter"/> interface).</summary>
    public class ReflectionTypeConverterTests : CollectionTypeConverterTests
    {

        public ReflectionTypeConverterTests(ITestOutputHelper output) : base(output)
        { }


        /// <summary>The type converter that is under test.</summary>
        protected new ReflectionTypeConverter TypeConverter { get; } = new ReflectionTypeConverter();



        #region ReflectionTypeConverter.BasicTests

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_ImplicitlytoderivedToDerivedObjectToBase_IsCorrect()
        {
            ImplicitlyConvertibleToDerived originalObject = new();
            DerivedClass expectedAssignedObject = originalObject;  // implicit conversion
            DerivedClass expectedRestoredValue = expectedAssignedObject;
            TypeConverter_ConversionToObjectAndBackTest<
                ImplicitlyConvertibleToDerived, DerivedClass, DerivedClass>(
                TypeConverter, originalObject, expectedAssignedObject, expectedRestoredValue);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_RoundTripConversion_ExplicitlytoderivedToDerivedObjectToBase_IsCorrect()
        {
            ExplicitlyConvertibleToDerived originalObject = new();
            DerivedClass expectedAssignedObject = (DerivedClass)originalObject;   // explicit conversion (cast)
            DerivedClass expectedRestoredValue = expectedAssignedObject;
            TypeConverter_ConversionToObjectAndBackTest<
                ExplicitlyConvertibleToDerived, DerivedClass, DerivedClass>(
                TypeConverter, originalObject, expectedAssignedObject, expectedRestoredValue);
        }


        [Fact]
        protected virtual void SpecificTypeConverter_OneDirectionConversion_ImplicitlyfromderivedToDerivedObjectToBase_IsCorrect()
        {
            DerivedClass originalObject = new();
            ImplicitlyConvertibleFromDerived expectedAssignedObject = originalObject;  // implicit conversion
            object expectedRestoredValue = null;  // onedirectional!
            TypeConverter_ConversionToObjectAndBackTest<
                DerivedClass, ImplicitlyConvertibleFromDerived, object>(
                TypeConverter, originalObject, expectedAssignedObject, expectedRestoredValue,
                restoreObjectBackToValue: false);
        }

        [Fact]
        protected virtual void SpecificTypeConverter_OneDirectionConversion_ExplicitlyfromderivedToDerivedObjectToBase_IsCorrect()
        {
            DerivedClass originalObject = new();
            ExplicitlyConvertibleFromDerived expectedAssignedObject = (ExplicitlyConvertibleFromDerived)originalObject;  // explicit conversion (cast)
            object expectedRestoredValue = null;  // onedirectional!
            TypeConverter_ConversionToObjectAndBackTest<
                DerivedClass, ExplicitlyConvertibleFromDerived, object>(
                TypeConverter, originalObject, expectedAssignedObject, expectedRestoredValue,
                restoreObjectBackToValue: false);
        }

        #endregion ReflectionTypeConverter.BasicTests



        #region ReflectionTypeConverter.DifferentCasesTests



        [Fact]
        public void ImplicitConversion_FromCelsiusToDouble_Works()
        {
            var c = new Celsius(36.5);
            var result = TypeConverter.ConvertToType(c, typeof(double));
            result.Should().Be(36.5);
        }

        [Fact]
        public void ExplicitConversion_FromCelsiusToFahrenheit_Works()
        {
            var c = new Celsius(100);
            var result = TypeConverter.ConvertToType(c, typeof(Fahrenheit));
            result.Should().BeOfType<Fahrenheit>()
                  .Which.Degrees.Should().BeApproximately(212, 0.001);
        }

        [Fact]
        public void ExplicitConversion_FromFahrenheitToCelsius_Works()
        {
            var f = new Fahrenheit(32);
            var result = TypeConverter.ConvertToType(f, typeof(Celsius));
            result.Should().BeOfType<Celsius>()
                  .Which.Degrees.Should().BeApproximately(0, 0.001);
        }

        [Fact]
        public void Conversion_UsingBaseClassOperator_Works()
        {
            var derived = new DerivedWrapper(5);
            var result = TypeConverter.ConvertToType(derived, typeof(string));
            result.Should().Be("Wrapped:5");
        }

        [Fact]
        public void Conversion_UsingInterfaceOperator_Works_WhenAllowed()
        {
            var obj = new InterfaceImpl(42);
            var result = TypeConverter.ConvertToType(obj, typeof(int));
            result.Should().Be(42);
        }

        [Fact]
        public void Conversion_UsingInterfaceOperator_Ignored_WhenDisabled()
        {
            var obj = new InterfaceImpl(99);
            Action act = () => TypeConverter.ConvertToType(obj, typeof(int), allowInterfaceConversions: false);
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*No conversion found*");
        }

        [Fact]
        public void Conversion_Fails_WhenNoOperatorExists()
        {
            var source = new object();
            Action act = () => TypeConverter.ConvertToType(source, typeof(DateTime));
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*No conversion found*");
        }



        #endregion ReflectionTypeConverter.DifferentCasesTests



    }




    #region SampleClasses.ReflectionTypeConverterTests

    public class Celsius
    {
        public double Degrees { get; }

        public Celsius(double degrees) => Degrees = degrees;

        public static implicit operator double(Celsius c) => c.Degrees;
        public static explicit operator Fahrenheit(Celsius c) => new Fahrenheit(c.Degrees * 9 / 5 + 32);
    }

    public class Fahrenheit
    {
        public double Degrees { get; }

        public Fahrenheit(double degrees) => Degrees = degrees;

        public static explicit operator Celsius(Fahrenheit f) => new Celsius((f.Degrees - 32) * 5 / 9);
    }

    public class BaseWrapper
    {
        public int Value { get; }

        public BaseWrapper(int value) => Value = value;

        public static implicit operator string(BaseWrapper w) => $"Wrapped:{w.Value}";
    }

    public class DerivedWrapper : BaseWrapper
    {
        public DerivedWrapper(int value) : base(value) { }
    }

    public interface IConvertibleThing
    {
        int Code { get; }
    }

    public class InterfaceImpl : IConvertibleThing
    {
        public int Code { get; }

        public InterfaceImpl(int code) => Code = code;

        public static implicit operator int(InterfaceImpl impl) => impl.Code;
    }

    #endregion SampleClasses.ReflectionTypeConverterTests



}





