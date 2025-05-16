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
        protected override ITypeConverter TypeConverter { get; } = new ReflectionTypeConverter();





#if IncludeFailedTestsByDesign
        [Fact]
#endif
        protected virtual void SpecificTypeConverter_RoundTripConversion_ImplicitlytoderivedToDerivedObjectToBase_IsCorrect()
        {
            ImplicitlyConvertibleToDerived originalObject = new();
            DerivedClass expectedAssignedObject = originalObject;  // implicit conversion
            DerivedClass expectedRestoredValue = expectedAssignedObject;
            TypeConverter_ConversionToObjectAndBackTest<
                ImplicitlyConvertibleToDerived, DerivedClass, DerivedClass>(
                TypeConverter, originalObject, expectedAssignedObject, expectedRestoredValue);
        }

#if IncludeFailedTestsByDesign
        [Fact]
#endif        
        protected virtual void SpecificTypeConverter_RoundTripConversion_ExplicitlytoderivedToDerivedObjectToBase_IsCorrect()
        {
            ExplicitlyConvertibleToDerived originalObject = new();
            DerivedClass expectedAssignedObject = (DerivedClass)originalObject;   // explicit conversion (cast)
            DerivedClass expectedRestoredValue = expectedAssignedObject;
            TypeConverter_ConversionToObjectAndBackTest<
                ExplicitlyConvertibleToDerived, DerivedClass, DerivedClass>(
                TypeConverter, originalObject, expectedAssignedObject, expectedRestoredValue);
        }


#if IncludeFailedTestsByDesign
        [Fact]
#endif        
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

#if IncludeFailedTestsByDesign
        [Fact]
#endif        
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







    }


}


