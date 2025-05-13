using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using IGLib.Tests.Base;
using System.Linq;
using System.Collections;
using IGLib.Core;
using IGLib.Core.CollectionExtensions;

namespace IGLib.Core.Tests
{

    /// <summary>Base class for type converter testing classes such as <see cref="BasicTypeConverterTests"/>.
    /// <para>Provides generic implementation of typical test methods for testing round-trip or one 
    /// direction conversions, and helper stuff like example classes on which conversions can be
    /// tested, with prescribed relations (inherits from, implements implicit / explicit conversion,
    /// etc.).</para></summary>
    /// <typeparam name="TestClass"></typeparam>
    public class TypeConverterTestsBase<TestClass> : TestBase<TestClass>
    {

        /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
        /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
        /// <param name="output">This parameter will be provided to constructor (injected via 
        /// constructor) by the test framework. I is also stored to Console property, such that
        /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
        /// method to generate test output.</param>
        public TypeConverterTestsBase(ITestOutputHelper output) : base(output)
        { }



        #region GenericConversionTests


        /// <summary>Like <see cref="TypeConverter_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        /// but with target type and the type of restored variable both equal to type of the original variable, and also
        /// the expected assigned object value and the expected restored value being equal to the original value.</summary>
        protected (CommonType Coverted, CommonType Restored) TypeConverter_ConversionToObjectAndBackTest
            <CommonType>(ITypeConverter typeConverter,
            CommonType original, bool restoreObjectBackToValue = true, bool doOutput = true)
        {
            return TypeConverter_ConversionToObjectAndBackTest<CommonType, CommonType, CommonType>(typeConverter,
                original, original, original, restoreObjectBackToValue, doOutput);
        }

        /// <summary>Performs test of conversion via <see cref="ITypeConverter"/> from a value of type
        /// <typeparamref name="OriginalType"/> to a value of the target type <typeparamref name="TargetType"/>
        /// to be assigned to a variable of type object, and then converts this value to type <typeparamref name="RestoredType"/>
        /// and copies (restores) it to a variable of that type.</summary>
        /// <param name="typeConverter">Converter used for type conversion.</param>
        /// <param name="originalValue">Original value that is converted converted to an object of type <see cref="TargetType"/>.</param>
        /// <param name="expectedAssignedObjectValue">The expected value after conversion of the <paramref name="originalValue"/> to
        /// a value of type <typeparamref name="TargetType"/> and assignment to a variable of type <see cref="object"/></param>
        /// <param name="expectedRestoredValue">Expected restored value after conversion of the original value and storing it in 
        /// a variable of type object and restoring it from the object variable to a variable of type <typeparamref name="RestoredType"/>.</param>
        /// <param name="restoreObjectBackToValue">If true (which is default) then object is also restored back to a value of type <typeparamref name="RestoredType"/>.</param>
        /// <typeparam name="OriginalType">Type of the origial value to be stored as object.</typeparam>
        /// <typeparam name="TargetType">Type to which the <paramref name="originalValue"/> will be converted when being stored in an object variable.</typeparam>
        /// <typeparam name="RestoredType">Type of variable to which the value will be restored from the variable of type object.</typeparam>
        protected (TargetType Converted, RestoredType Restored) TypeConverter_ConversionToObjectAndBackTest
            <OriginalType, TargetType, RestoredType>(
            ITypeConverter typeConverter,
            OriginalType originalValue, TargetType expectedAssignedObjectValue,
            RestoredType expectedRestoredValue, bool restoreObjectBackToValue = true, bool doDetailedOutput = true)
        {
            // Arrange
            Type declaredOriginalType = typeof(OriginalType);
            Type requestedTargetType = typeof(TargetType);
            Type requestedRestoredType = typeof(RestoredType);
            object convertedObject = default;
            RestoredType restoredValue = default;
            if (doDetailedOutput)
            {
                Console.WriteLine($"Converting value of type {typeof(OriginalType).Name} to object of type {typeof(TargetType)}");
                if (restoreObjectBackToValue)
                {
                    Console.WriteLine($"  and restoring the object to a value of type {typeof(RestoredType).Name}.");
                }
                Console.WriteLine($"Original object:");
                Console.WriteLine($"  Declared Type: {typeof(OriginalType).Name}");
                Console.WriteLine($"  Actual Type:   {originalValue?.GetType().Name ?? "unknown"}");
                Console.WriteLine($"  Value: <{originalValue}>; using ToReadableString():");
                Console.WriteLine($"{originalValue.ToReadableString()}");
                Console.WriteLine("");
            }
            // Act
            convertedObject = typeConverter.ConvertToType(originalValue, requestedTargetType);
            if (doDetailedOutput)
            {
                Console.WriteLine($"Converted object:");
                Console.WriteLine($"  Declared Type: {typeof(TargetType).Name}");
                Console.WriteLine($"  Actual Type:   {convertedObject?.GetType().Name ?? "unknown"}");
                Console.WriteLine($"  Value: <{convertedObject}>; using ToReadableString():");
                Console.WriteLine($"{convertedObject.ToReadableString()}");
                Console.WriteLine("");
                if (convertedObject == null)
                {
                    Console.WriteLine("Warning: Converted object is null.");
                }
                else
                {
                    Console.WriteLine($"Converted object is of type {convertedObject.GetType().Name}, value: {convertedObject}");
                }
            }
            // Assert
            if (originalValue == null)
            {
                if (convertedObject != null)
                {
                    Console.WriteLine($"Warning: the original value is null but the restored value is not null.");
                }
                convertedObject.Should().BeNull(because: "null original should produce null when converted to object.");
            }
            else
            {
                // originalValue != null
                if (convertedObject == null)
                {
                    Console.WriteLine("WARNING: the original value is not null but the converted object is null.");
                }
                convertedObject.Should().NotBeNull(because: $"Value of type {originalValue.GetType().Name} should be convertet to object of type {requestedTargetType.Name}.");
                Type actualTargetType = convertedObject.GetType();
                if (requestedTargetType.IsClass)
                {
                    requestedTargetType.IsAssignableFrom(actualTargetType).Should().Be(true,
                        because: "The requested target type should be assignable from the actual type of the converted object.");
                }
                else
                {
                    convertedObject.GetType().Should().Be(requestedTargetType, because: $"Type of the converted object should mach the target type {requestedTargetType.Name}.");
                }
                if (requestedTargetType == typeof(string) || 
                    (!(requestedTargetType.IsClass || convertedObject is IList || convertedObject is IEnumerable || convertedObject is Array)))
                {
                    Console.WriteLine("Comparisson to the expected converted object will be applied.");
                    convertedObject.Should().Be(expectedAssignedObjectValue);
                }
            }
            if (restoreObjectBackToValue)
            {
                // Q: Should we do it like this in some cases?: restored = (RestoredType)assignedObject;
                restoredValue = (RestoredType)typeConverter.ConvertToType(convertedObject, typeof(RestoredType));

                convertedObject = typeConverter.ConvertToType(originalValue, requestedTargetType);
                if (doDetailedOutput)
                {
                    Console.WriteLine($"Restored value (round-trip conversion):");
                    Console.WriteLine($"  Declared Type: {typeof(RestoredType).Name}");
                    Console.WriteLine($"  Actual Type:   {restoredValue?.GetType().Name ?? "unknown"}");
                    Console.WriteLine($"  Value: <{restoredValue}>; using ToReadableString():");
                    Console.WriteLine($"{restoredValue.ToReadableString()}");
                    Console.WriteLine("");
                    if (restoredValue == null)
                    {
                        Console.WriteLine("Restored value is null.");
                    }
                }
                if (convertedObject == null)
                {
                    if (restoredValue != null)
                    {
                        Console.WriteLine($"Warning: restored object is null but the restored value is not null.");
                    }
                    restoredValue.Should().BeNull(because: "null rstored object should result in null restored value.");
                }
                else
                {
                    // restoredValue is NOT null
                    if (restoredValue == null)
                    {
                        Console.WriteLine("WARNING: Restored value is null but assignd object from which value was resttored is not.");
                    }
                    restoredValue.Should().NotBeNull(because: "The converted object is not null, therefore the restored object should also not be null.");
                    Type actualRestoredType = restoredValue.GetType();
                    //if (doOutput)
                    //{
                    //    Console.WriteLine($"Value of type {actualRestoredType.Name} restored from the object: {restoredValue}");
                    //}
                    if (requestedRestoredType.IsClass)
                    {
                        requestedRestoredType.IsAssignableFrom(actualRestoredType).Should().Be(true,
                            because: "The requested target type should be assignable from the actual type of the restored object.");
                    }
                    else
                    {
                        restoredValue.GetType().Should().Be(requestedRestoredType, because: $"Type of the restored object should mach the target type {requestedTargetType.Name}.");
                    }
                    if (requestedRestoredType == typeof(string) ||
                        !(requestedRestoredType.IsClass || restoredValue is IList || restoredValue is IEnumerable || restoredValue is Array))
                    {
                        Console.WriteLine("Comparisson to the expected restored value will be applied.");
                        restoredValue.Should().Be(expectedRestoredValue, because: $"Restoring object that hods {requestedTargetType.Name} should correctly reproduce the original value of type {originalValue.GetType().Name}.");
                    }
                }
            }
            return (Converted: (TargetType)convertedObject, Restored: restoredValue);
        }


        #endregion GenericConversionTests




        #region GenericConversionSpeedTests





        /// <summary>Like <see cref="TypeConverter_Speed_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(ITypeConverter, int, double, OriginalType, TargetType, RestoredType, bool)"/>,
        /// but with target type and the type of restored variable both equal to type of the original variable, and also
        /// the expected assiged object value and the restored value being equal to the original value.</summary>
        protected void TypeConverter_Speed_ConversionToObjectAndBackTest<OriginalType>(
            ITypeConverter typeConverter, int numExecutions, double minExecutionsPerSecond,
            OriginalType original, bool restoreObjectBackToValue = true, bool doOutputInReferenceTest = false)
        {
            TypeConverter_Speed_ConversionToObjectAndBackTest<OriginalType, OriginalType, OriginalType>(
                typeConverter, numExecutions, minExecutionsPerSecond,
                original, original, original, restoreObjectBackToValue, doOutputInReferenceTest);
        }



        /// <summary>Performs a speed test of conversion via <see cref="ITypeConverter"/> from a value of type
        /// <typeparamref name="OriginalType"/> to an object variable of target type <typeparamref name="TargetType"/>
        /// and back to value of type <typeparamref name="RestoredType"/>.
        /// <para>Expect for parameters related to measurement of the speed of conversions, parameters have the same meaning
        /// as in the <see cref="TypeConverter_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(ITypeConverter, 
        /// OriginalType, TargetType, RestoredType, bool)"/> method. Type parameters also have the same meaning as in
        /// that method.</para></summary>
        /// <param name="numExecutions">Number of executinos for speed measurements.</param>
        /// <param name="minExecutionsPerSecond">The expected minimal speed, in number of executions of type conversions 
        /// per second.</param>
        /// <param name="restoreObjectBackToValue">If true (which is default) then object is also restored back to a value of type <typeparamref name="RestoredType"/>.</param>
        protected (TargetType Converted, RestoredType Restored) TypeConverter_Speed_ConversionToObjectAndBackTest<
            OriginalType, TargetType, RestoredType>(
            ITypeConverter typeConverter, int numExecutions, double minExecutionsPerSecond,
            OriginalType originalValue, TargetType expectedAssignedObjectValue, RestoredType expectedRestoredValue,
            bool restoreObjectBackToValue = true, bool doOutputInReferenceTest = false)
        {
            Console.WriteLine("Conversion SPEED test:");
            TargetType converted;
            RestoredType restored;
            Type requestedTargetType = typeof(TargetType);
            Type requestedRestoredType = typeof(RestoredType);
            (converted, restored) = TypeConverter_ConversionToObjectAndBackTest<OriginalType, TargetType, RestoredType>(
            typeConverter, originalValue, expectedAssignedObjectValue, expectedRestoredValue, restoreObjectBackToValue, doOutputInReferenceTest);



            // Then, do a similar thing in a loop, but without assertions:
            // Speifyinf the frequency of wtiring a dot:
            int frequency = 1;
            double numDots = (double)numExecutions / frequency;
            while ((int)numDots >= 50)
            {
                frequency *= 10;
                numDots = (double)numExecutions / frequency;
            }
            Console.WriteLine("");
            Console.WriteLine($"Performing SPEED TEST, ({numExecutions}) executions...");
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < numExecutions; ++i)
            {
                // Act
                object convertedObject = (TargetType) typeConverter.ConvertToType(originalValue, requestedTargetType);
                if (restoreObjectBackToValue)
                {
                    object restoredObject = (RestoredType)typeConverter.ConvertToType(convertedObject, requestedRestoredType);
                }
                if (frequency < 0 && /* Exclude printing dots! */
                    i > 0 && i % frequency == 0)
                {
                    Console.WriteLine($". ({i})");
                }
            }
            sw.Stop();
            double totalTime = sw.Elapsed.TotalSeconds;
            double executionsPerSecond = (double)numExecutions / totalTime;
            Console.WriteLine($"Number of executions: {numExecutions} ({(double)numExecutions / 1_000.0} k).");
            Console.WriteLine($"Elapsed time: {totalTime} s");
            Console.WriteLine($"Number of executions per second: {executionsPerSecond}");
            Console.WriteLine($"         In millions per second: {executionsPerSecond / 1.0e6}");
            executionsPerSecond.Should().BeGreaterThanOrEqualTo(minExecutionsPerSecond);
            return (Converted: converted, Restored: restored);
        }



        //// First, just perform the ordinary test, such that test vreaks if the case does not work correctly:
        //// Arrange
        //Type declaredOriginalType = typeof(OriginalType);
        //Type requestedTargetType = typeof(TargetType);
        //Type requestedRestoredType = typeof(RestoredType);
        //RestoredType restoredValue;
        //Console.WriteLine($"Converting value of type {originalValue.GetType().Name}, value = {originalValue}. to object, and storing the object.");
        //// Act
        //object assignedObject = typeConverter.ConvertToType(originalValue, requestedTargetType);
        //Console.WriteLine($"Assigned object: type = {assignedObject.GetType().Name}, value: {assignedObject}");
        //if (assignedObject == null)
        //{
        //    Console.WriteLine("Warning: Converted object is null.");
        //}
        //else
        //{
        //    Console.WriteLine($"Converted object is of type {assignedObject.GetType().Name}, value: {assignedObject}");
        //}
        //// Assert
        //if (originalValue == null)
        //{
        //    if (assignedObject != null)
        //    {
        //        Console.WriteLine($"Warning: the original value is null but the restored value is not null.");
        //    }
        //    assignedObject.Should().BeNull(because: "null original should produce null when converted to object.");
        //}
        //else
        //{
        //    // originalValue != null
        //    if (assignedObject == null)
        //    {
        //        Console.WriteLine("WARNING: the original value is not null but the assigned object is null.");
        //    }
        //    assignedObject.Should().NotBeNull(because: $"Value of type {originalValue.GetType().Name} should be convertet to object of type {requestedTargetType.Name}.");
        //    Type actualTargetType = assignedObject.GetType();
        //    if (requestedTargetType.IsClass)
        //    {
        //        requestedTargetType.IsAssignableFrom(actualTargetType).Should().Be(true,
        //            because: "The requested target type should be assignable from the actual type of the assigned object.");
        //    }
        //    else
        //    {
        //        assignedObject.GetType().Should().Be(requestedTargetType, because: $"Type of the assigned object should mach the target type {requestedTargetType.Name}.");
        //    }
        //}
        //if (restoreObjectBackToValue)
        //{
        //    // Q: Should we do it like this in some cases?: restored = (RestoredType)assignedObject;
        //    restoredValue = (RestoredType)typeConverter.ConvertToType(assignedObject, typeof(RestoredType));
        //    if (restoredValue == null)
        //    {
        //        Console.WriteLine("Restored value is null.");
        //    }
        //    Console.WriteLine($"Restored value: type = {restoredValue.GetType().Name}, value: {restoredValue}");
        //    if (assignedObject == null)
        //    {
        //        if (restoredValue != null)
        //        {
        //            Console.WriteLine($"Warning: assigned object is null but the restored value is not null.");
        //        }
        //        restoredValue.Should().BeNull(because: "null assigned object should result in null restored value.");
        //    }
        //    else
        //    {
        //        // assignedObject is NOT null
        //        if (restoredValue == null)
        //        {
        //            Console.WriteLine("WARNING: Restored value is null but assignd object from which value was resttored is not.");
        //        }
        //        restoredValue.Should().NotBeNull(because: "The assigned object is not null, therefore the restored object should also not be null.");
        //        Type actualRestoredType = restoredValue.GetType();
        //        Console.WriteLine($"Value of type {actualRestoredType.Name} restored from the object: {restoredValue}");
        //        if (requestedRestoredType.IsClass)
        //        {
        //            requestedRestoredType.IsAssignableFrom(actualRestoredType).Should().Be(true,
        //                because: "The requested target type should be assignable from the actual type of the assigned object.");
        //        }
        //        else
        //        {
        //            assignedObject.GetType().Should().Be(requestedTargetType, because: $"Type of the assigned object should mach the target type {requestedTargetType.Name}.");
        //        }
        //        restoredValue.Should().Be(expectedRestoredValue, because: $"Restoring object that hods {requestedTargetType.Name} should correctly reproduce the original value of type {originalValue.GetType().Name}.");
        //    }
        //}




        #endregion GenericConversionSpeedTests




    }

}


