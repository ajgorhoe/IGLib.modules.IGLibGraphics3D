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
    public class TypeConverterTestsBase<TestClass> : TestBase<TestClass>
    {

        /// <summary>Calling base constructor initializes things like <see cref="TestBase{TestClass}.Output"/> 
        /// and <see cref="TestBase{TestClass}.Output"/> properties that enable writing on tests' output.</summary>
        /// <param name="output">This parameter will be provided to constructor (injected via 
        /// constructor) by the test framework. I is also stored to Console property, such that
        /// test code can use <see cref="RoslynScriptingApiExamplesTests.Console.WriteLine(string)"/> 
        /// method to generate test output.</param>
        public TypeConverterTestsBase(ITestOutputHelper output) : base(output)
        {  }


        #region GenericConversionTests


        /// <summary>Like <see cref="TypeConverter_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        /// but with target type and the type of restored variable both equal to type of the original variable, and also
        /// the expected restored value being equal to the original value.</summary>
        protected void TypeConverter_ConversionToObjectAndBackTest<OriginalType>(ITypeConverter typeConverter,
            OriginalType original, bool restoreObjectBackToValue = true)
        {
            TypeConverter_ConversionToObjectAndBackTest<OriginalType, OriginalType, OriginalType>(typeConverter,
                original, original, restoreObjectBackToValue);
        }

        /// <summary>Like <see cref="TypeConverter_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        /// but with the type of restored variable equal to type of the original variable.</summary>
        protected void TypeConverter_ConversionToObjectAndBackTest<OriginalType, TargetType>(ITypeConverter typeConverter,
            OriginalType original, OriginalType expectedRestoredValue, bool restoreObjectBackToValue = true)
        {
            TypeConverter_ConversionToObjectAndBackTest<OriginalType, TargetType, OriginalType>(typeConverter,
                original, expectedRestoredValue, restoreObjectBackToValue);
        }


        /// <summary>Like <see cref="TypeConverter_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        /// but with the type of restored variable equal to type of the original variable, and also with expected
        /// restored value equal to the original value.</summary>
        protected void TypeConverter_ConversionToObjectAndBackTest<OriginalType, TargetType>(ITypeConverter typeConverter,
            OriginalType original, bool restoreObjectBackToValue = true)
        {
            TypeConverter_ConversionToObjectAndBackTest<OriginalType, TargetType, OriginalType>(typeConverter,
                original, original, restoreObjectBackToValue);
        }


        /// <summary>Performs test of conversion via <see cref="TypeConversionHelper"/> from a value of type
        /// <typeparamref name="OriginalType"/> to an object variable of target type <typeparamref name="TargetType"/>
        /// and back to value of type <typeparamref name="RestoredType"/>.</summary>
        /// <param name="originalValue">Original value that is converted to object.</param>
        /// <param name="expectedRestoredValue">Expected restored value after conversion of original to object and restoring back to original.</param>
        /// <param name="restoreObjectBackToValue">If true (which is default) then object is also restored back to a value of type <typeparamref name="RestoredType"/>.</param>
        protected void TypeConverter_ConversionToObjectAndBackTest<OriginalType, TargetType, RestoredType>(ITypeConverter typeConverter,
            OriginalType originalValue, RestoredType expectedRestoredValue, bool restoreObjectBackToValue = true)
        {
            // Arrange
            Type targetType = typeof(TargetType);
            RestoredType restored;
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


        #endregion GenericConversionTests




        #region GenericSpeedTests


        /// <summary>Like <see cref="TypeConverter_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        /// but with target type and the type of restored variable both equal to type of the original variable, and also
        /// the expected restored value being equal to the original value.</summary>
        protected void TypeConverter_Speed_ConversionToObjectAndBackTest<OriginalType>(
            ITypeConverter typeConverter, int numExecutions, double minExecutionsPerSecond,
            OriginalType original, bool restoreObjectBackToValue = true)
        {
            TypeConverter_Speed_ConversionToObjectAndBackTest<OriginalType, OriginalType, OriginalType>(
                typeConverter, numExecutions, minExecutionsPerSecond,
                original, original, restoreObjectBackToValue);
        }

        /// <summary>Like <see cref="TypeConverter_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        /// but with the type of restored variable equal to type of the original variable.</summary>
        protected void TypeConverter_Speed_ConversionToObjectAndBackTest<OriginalType, TargetType>(
            ITypeConverter typeConverter, int numExecutions, double minExecutionsPerSecond,
            OriginalType original, OriginalType expectedRestoredValue, bool restoreObjectBackToValue = true)
        {
            TypeConverter_Speed_ConversionToObjectAndBackTest<OriginalType, TargetType, OriginalType>(
                typeConverter, numExecutions, minExecutionsPerSecond,
                original, expectedRestoredValue, restoreObjectBackToValue);
        }


        /// <summary>Like <see cref="TypeConverter_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        /// but with the type of restored variable equal to type of the original variable, and also with expected
        /// restored value equal to the original value.</summary>
        protected void TypeConverter_Speed_ConversionToObjectAndBackTest<OriginalType, TargetType>(
            ITypeConverter typeConverter, int numExecutions, double minExecutionsPerSecond,
            OriginalType original, bool restoreObjectBackToValue = true)
        {
            TypeConverter_Speed_ConversionToObjectAndBackTest<OriginalType, TargetType, OriginalType>(
                typeConverter, numExecutions, minExecutionsPerSecond,
                original, original, restoreObjectBackToValue);
        }


        /// <summary>Performs test of conversion via <see cref="TypeConversionHelper"/> from a value of type
        /// <typeparamref name="OriginalType"/> to an object variable of target type <typeparamref name="TargetType"/>
        /// and back to value of type <typeparamref name="RestoredType"/>.</summary>
        /// <param name="originalValue">Original value that is converted to object.</param>
        /// <param name="expectedRestoredValue">Expected restored value after conversion of original to object and restoring back to original.</param>
        /// <param name="restoreObjectBackToValue">If true (which is default) then object is also restored back to a value of type <typeparamref name="RestoredType"/>.</param>
        protected void TypeConverter_Speed_ConversionToObjectAndBackTest<OriginalType, TargetType, RestoredType>(
            ITypeConverter typeConverter, int numExecutions, double minExecutionsPerSecond,
            OriginalType originalValue, RestoredType expectedRestoredValue, bool restoreObjectBackToValue = true)
        {
            // First, just perform the ordinary test, such that test vreaks if the case does not work correctly:
            // Arrange
            Type targetType = typeof(TargetType);
            RestoredType restored;
            Console.WriteLine("Conversion SPEED test:");
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
            assignedObject.Should().NotBeNull(because: $"Precond: Value of type {originalValue.GetType().Name} value can be convertet to object of type {targetType.Name}.");
            assignedObject.GetType().Should().Be(targetType, because: $"Precond: Type of the assigned object should mach the target typ {targetType.Name}.");
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
                restored.Should().Be(expectedRestoredValue, because: $"Precond: Restoring object that hods {targetType.Name} should correctly reproduce the original value of type {originalValue.GetType().Name}.");
            }
            // Then, do a similar thing in a loop, but without assertions:
            // Speifyinf the frequency of wtiring a dot:
            int frequency = 1;
            double numDots = (double)numExecutions / frequency;
            while ((int) numDots >=50)
            {
                frequency *= 10;
                numDots = (double)numExecutions / frequency;
            }
            Console.WriteLine("");
            Console.WriteLine($"Performing speed tests, ({numExecutions}) executions...");
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < numExecutions; ++i)
            {

                // Act
                assignedObject = typeConverter.ConvertToType(originalValue, targetType);
                if (restoreObjectBackToValue)
                {
                    restored = (RestoredType)typeConverter.ConvertToType(assignedObject, typeof(RestoredType));
                }
                if (i > 0 && i % frequency == 0)
                {
                    Console.WriteLine($". ({i})");
                }
            }
            sw.Stop();
            double totalTime = sw.Elapsed.TotalSeconds;
            double executionsPerSecond = (double)numExecutions / totalTime;
            Console.WriteLine($"Number of executions: {numExecutions} ({(double) numExecutions / 1_000.0} k).");
            Console.WriteLine($"Elapsed time: {totalTime} s");
            Console.WriteLine($"Number of executions per second: {executionsPerSecond}");
            Console.WriteLine($"         In millions per second: {executionsPerSecond / 1.0e6}");
            executionsPerSecond.Should().BeGreaterThanOrEqualTo(minExecutionsPerSecond);

        }

        #endregion GenercSpeedTests

    }

}


