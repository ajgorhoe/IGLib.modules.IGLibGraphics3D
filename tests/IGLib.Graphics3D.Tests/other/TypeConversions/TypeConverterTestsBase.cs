using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using IGLib.Tests.Base;
using System.Linq;
using IGLib.Core;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;


namespace IGLib.Core.Tests
{

    /// <summary>Base class for type converter testing classes such as <see cref="BasicTypeConverterTests"/>.
    /// <para>Provides generic implementation of typical test methods for testing round-trip or one 
    /// direction conversions, and helper stubb like example classes on which conversions can be
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
        {  }


        #region GenericConversionTests


        /// <summary>Like <see cref="TypeConverter_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(OriginalType, RestoredType)"/>,
        /// but with target type and the type of restored variable both equal to type of the original variable, and also
        /// the expected assigned object value and the expected restored value being equal to the original value.</summary>
        protected void TypeConverter_ConversionToObjectAndBackTest<CommonType>(ITypeConverter typeConverter,
            CommonType original, bool restoreObjectBackToValue = true)
        {
            TypeConverter_ConversionToObjectAndBackTest<CommonType, CommonType, CommonType>(typeConverter,
                original, original, original, restoreObjectBackToValue);
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
        protected void TypeConverter_ConversionToObjectAndBackTest<OriginalType, TargetType, RestoredType>(ITypeConverter typeConverter,
            OriginalType originalValue, TargetType expectedAssignedObjectValue, RestoredType expectedRestoredValue, bool restoreObjectBackToValue = true)
        {
            // Arrange
            Type declaredOriginalType = typeof(OriginalType);
            Type requestedTargetType = typeof(TargetType);
            Type requestedRestoredType = typeof(RestoredType);
            RestoredType restoredValue;
            Console.WriteLine($"Converting value of type {originalValue.GetType().Name}, value = {originalValue}. to object, and storing the object.");
            // Act
            object assignedObject = typeConverter.ConvertToType(originalValue, requestedTargetType);
            Console.WriteLine($"Assigned object: type = {assignedObject.GetType().Name}, value: {assignedObject}");
            if (assignedObject == null)
            {
                Console.WriteLine("Warning: Converted object is null.");
            }
            else
            {
                Console.WriteLine($"Converted object is of type {assignedObject.GetType().Name}, value: {assignedObject}");
            }
            // Assert
            if (originalValue == null)
            {
                if (assignedObject != null)
                {
                    Console.WriteLine($"Warning: the original value is null but the restored value is not null.");
                }
                assignedObject.Should().BeNull(because: "null original should produce null when converted to object.");
            } else
            {
                // originalValue != null
                if (assignedObject == null)
                {
                    Console.WriteLine("WARNING: the original value is not null but the assigned object is null.");
                }
                assignedObject.Should().NotBeNull(because: $"Value of type {originalValue.GetType().Name} should be convertet to object of type {requestedTargetType.Name}.");
                Type actualTargetType = assignedObject.GetType();
                if (requestedTargetType.IsClass)
                {
                    requestedTargetType.IsAssignableFrom(actualTargetType).Should().Be(true, 
                        because: "The requested target type should be assignable from the actual type of the assigned object.");
                }
                else
                {
                    assignedObject.GetType().Should().Be(requestedTargetType, because: $"Type of the assigned object should mach the target type {requestedTargetType.Name}.");
                }
            }
            if (restoreObjectBackToValue)
            {
                // Q: Should we do it like this in some cases?: restored = (RestoredType)assignedObject;
                restoredValue = (RestoredType)typeConverter.ConvertToType(assignedObject, typeof(RestoredType));
                if (restoredValue == null)
                {
                    Console.WriteLine("Restored value is null.");
                }
                Console.WriteLine($"Restored value: type = {restoredValue.GetType().Name}, value: {restoredValue}");
                if (assignedObject == null)
                {
                    if (restoredValue != null)
                    {
                        Console.WriteLine($"Warning: assigned object is null but the restored value is not null.");
                    }
                    restoredValue.Should().BeNull(because: "null assigned object should result in null restored value.");
                }
                else
                {
                    // assignedObject is NOT null
                    if (restoredValue == null)
                    {
                        Console.WriteLine("WARNING: Restored value is null but assignd object from which value was resttored is not.");
                    }
                    restoredValue.Should().NotBeNull(because: "The assigned object is not null, therefore the restored object should also not be null.");
                    Type actualRestoredType = restoredValue.GetType();
                    Console.WriteLine($"Value of type {actualRestoredType.Name} restored from the object: {restoredValue}");
                    if (requestedRestoredType.IsClass)
                    {
                        requestedRestoredType.IsAssignableFrom(actualRestoredType).Should().Be(true,
                            because: "The requested target type should be assignable from the actual type of the assigned object.");
                    }
                    else
                    {
                        assignedObject.GetType().Should().Be(requestedTargetType, because: $"Type of the assigned object should mach the target type {requestedTargetType.Name}.");
                    }
                    restoredValue.Should().Be(expectedRestoredValue, because: $"Restoring object that hods {requestedTargetType.Name} should correctly reproduce the original value of type {originalValue.GetType().Name}.");
                }
            }
        }


        #endregion GenericConversionTests




        #region GenericConversionSpeedTests





        /// <summary>Like <see cref="TypeConverter_Speed_ConversionToObjectAndBackTest{OriginalType, TargetType, RestoredType}(ITypeConverter, int, double, OriginalType, TargetType, RestoredType, bool)"/>,
        /// but with target type and the type of restored variable both equal to type of the original variable, and also
        /// the expected assiged object value and the restored value being equal to the original value.</summary>
        protected void TypeConverter_Speed_ConversionToObjectAndBackTest<OriginalType>(
            ITypeConverter typeConverter, int numExecutions, double minExecutionsPerSecond,
            OriginalType original, bool restoreObjectBackToValue = true)
        {
            TypeConverter_Speed_ConversionToObjectAndBackTest<OriginalType, OriginalType, OriginalType>(
                typeConverter, numExecutions, minExecutionsPerSecond,
                original, original, original, restoreObjectBackToValue);
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
        protected void TypeConverter_Speed_ConversionToObjectAndBackTest<OriginalType, TargetType, RestoredType>(
            ITypeConverter typeConverter, int numExecutions, double minExecutionsPerSecond,
            OriginalType originalValue, TargetType expectedAssignedObjectValue, RestoredType expectedRestoredValue, 
            bool restoreObjectBackToValue = true)
        {
            Console.WriteLine("Conversion SPEED test:");
            // First, just perform the ordinary test, such that test vreaks if the case does not work correctly:
            // Arrange
            Type declaredOriginalType = typeof(OriginalType);
            Type requestedTargetType = typeof(TargetType);
            Type requestedRestoredType = typeof(RestoredType);
            RestoredType restoredValue;
            Console.WriteLine($"Converting value of type {originalValue.GetType().Name}, value = {originalValue}. to object, and storing the object.");
            // Act
            object assignedObject = typeConverter.ConvertToType(originalValue, requestedTargetType);
            Console.WriteLine($"Assigned object: type = {assignedObject.GetType().Name}, value: {assignedObject}");
            if (assignedObject == null)
            {
                Console.WriteLine("Warning: Converted object is null.");
            }
            else
            {
                Console.WriteLine($"Converted object is of type {assignedObject.GetType().Name}, value: {assignedObject}");
            }
            // Assert
            if (originalValue == null)
            {
                if (assignedObject != null)
                {
                    Console.WriteLine($"Warning: the original value is null but the restored value is not null.");
                }
                assignedObject.Should().BeNull(because: "null original should produce null when converted to object.");
            }
            else
            {
                // originalValue != null
                if (assignedObject == null)
                {
                    Console.WriteLine("WARNING: the original value is not null but the assigned object is null.");
                }
                assignedObject.Should().NotBeNull(because: $"Value of type {originalValue.GetType().Name} should be convertet to object of type {requestedTargetType.Name}.");
                Type actualTargetType = assignedObject.GetType();
                if (requestedTargetType.IsClass)
                {
                    requestedTargetType.IsAssignableFrom(actualTargetType).Should().Be(true,
                        because: "The requested target type should be assignable from the actual type of the assigned object.");
                }
                else
                {
                    assignedObject.GetType().Should().Be(requestedTargetType, because: $"Type of the assigned object should mach the target type {requestedTargetType.Name}.");
                }
            }
            if (restoreObjectBackToValue)
            {
                // Q: Should we do it like this in some cases?: restored = (RestoredType)assignedObject;
                restoredValue = (RestoredType)typeConverter.ConvertToType(assignedObject, typeof(RestoredType));
                if (restoredValue == null)
                {
                    Console.WriteLine("Restored value is null.");
                }
                Console.WriteLine($"Restored value: type = {restoredValue.GetType().Name}, value: {restoredValue}");
                if (assignedObject == null)
                {
                    if (restoredValue != null)
                    {
                        Console.WriteLine($"Warning: assigned object is null but the restored value is not null.");
                    }
                    restoredValue.Should().BeNull(because: "null assigned object should result in null restored value.");
                }
                else
                {
                    // assignedObject is NOT null
                    if (restoredValue == null)
                    {
                        Console.WriteLine("WARNING: Restored value is null but assignd object from which value was resttored is not.");
                    }
                    restoredValue.Should().NotBeNull(because: "The assigned object is not null, therefore the restored object should also not be null.");
                    Type actualRestoredType = restoredValue.GetType();
                    Console.WriteLine($"Value of type {actualRestoredType.Name} restored from the object: {restoredValue}");
                    if (requestedRestoredType.IsClass)
                    {
                        requestedRestoredType.IsAssignableFrom(actualRestoredType).Should().Be(true,
                            because: "The requested target type should be assignable from the actual type of the assigned object.");
                    }
                    else
                    {
                        assignedObject.GetType().Should().Be(requestedTargetType, because: $"Type of the assigned object should mach the target type {requestedTargetType.Name}.");
                    }
                    restoredValue.Should().Be(expectedRestoredValue, because: $"Restoring object that hods {requestedTargetType.Name} should correctly reproduce the original value of type {originalValue.GetType().Name}.");
                }
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
                assignedObject = typeConverter.ConvertToType(originalValue, requestedTargetType);
                if (restoreObjectBackToValue)
                {
                    restoredValue = (RestoredType)typeConverter.ConvertToType(assignedObject, typeof(RestoredType));
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
            Console.WriteLine($"Number of executions: {numExecutions} ({(double) numExecutions / 1_000.0} k).");
            Console.WriteLine($"Elapsed time: {totalTime} s");
            Console.WriteLine($"Number of executions per second: {executionsPerSecond}");
            Console.WriteLine($"         In millions per second: {executionsPerSecond / 1.0e6}");
            executionsPerSecond.Should().BeGreaterThanOrEqualTo(minExecutionsPerSecond);
        }

        #endregion GenericConversionSpeedTests



        #region ClassesForTests

        protected class BaseClass
        {
            public BaseClass()
            {
                ID = 98;
                Name = "BaseClass object";
            }
            public override string ToString()
            {
                return $"{GetType().Name}: ID = {ID}, Name = \"{Name}\".";
            }
            public int ID { get; set; }
            public string Name { get; set; }

            // Implementations for equality comparisson:
            public override bool Equals(object obj) => this.Equals(obj as BaseClass);
            public bool Equals(BaseClass compared)
            {
                if (compared is null)
                {
                    return false;
                }
                // Optimization for a common success case.
                if (Object.ReferenceEquals(this, compared))
                {
                    return true;
                }
                // If run-time types are not exactly the same, return false.
                if (this.GetType() != compared.GetType())
                {
                    return false;
                }
                // Return true if the fields match.
                // Note that the base class is not invoked because it is
                // System.Object, which defines Equals as reference equality.
                return (ID == compared.ID) && (Name == compared.Name);
            }
            public override int GetHashCode() => (ID, Name).GetHashCode();
            public static bool operator ==(BaseClass lhs, BaseClass rhs)
            {
                if (lhs is null)
                {
                    if (rhs is null)
                    {
                        return true;
                    }
                    // Only the left side is null.
                    return false;
                }
                // Equals handles case of null on right side.
                return lhs.Equals(rhs);
            }
            public static bool operator !=(BaseClass lhs, BaseClass rhs) => !(lhs == rhs);
        }


        protected class DerivedClass : BaseClass
        {
            public DerivedClass()
            {
                ID = 466;
                Name = $"{GetType().Name} object";
                Description = $"This is an instance of {GetType().Name}.";
            }
            public override string ToString()
            {
                return $"{GetType().Name}: ID = {ID}, Name = \"{Name}\", Description = \"{Description}\".";
            }
            public string Description { get; set; }
            public static implicit operator DerivedClass(ImplicitlyConvertibleToDerived source)
            {
                if (source == null) return null;
                return new DerivedClass
                {
                    ID = source.MyId2,
                    Name = source.MyName2,
                    Description = source.MyDescription2,
                };
            }
            public const string DescriptionWhenConvertedFromIncompatibleClass =
                "Converted from a class that do not have Description equivalent.";
            public static explicit operator DerivedClass(ExplicitlyConvertibleToDerived source)
            {
                if (source == null) return null;
                return new DerivedClass
                {
                    ID = source.MyId1,
                    Name = source.MyName1,
                    Description = DescriptionWhenConvertedFromIncompatibleClass
                };
            }

            // Implementations for equality comparisson:
            public override bool Equals(object obj) => this.Equals(obj as DerivedClass);
            public bool Equals(DerivedClass compared)
            {
                if (compared is null)
                {
                    return false;
                }
                // Optimization for a common success case.
                if (Object.ReferenceEquals(this, compared))
                {
                    return true;
                }
                // If run-time types are not exactly the same, return false.
                if (this.GetType() != compared.GetType())
                {
                    return false;
                }
                // Return true if the fields match.
                // Note that the base class is not invoked because it is
                // System.Object, which defines Equals as reference equality.
                return (ID == compared.ID) && (Name == compared.Name && Description == compared.Description);
            }
            public override int GetHashCode() => (ID, Name, Description).GetHashCode();
            public static bool operator ==(DerivedClass lhs, DerivedClass rhs)
            {
                if (lhs is null)
                {
                    if (rhs is null)
                    {
                        return true;
                    }
                    // Only the left side is null.
                    return false;
                }
                // Equals handles case of null on right side.
                return lhs.Equals(rhs);
            }
            public static bool operator !=(DerivedClass lhs, DerivedClass rhs) => !(lhs == rhs);
        }


        protected class ExplicitlyConvertibleToDerived
        {
            public ExplicitlyConvertibleToDerived()
            {
                MyId1 = 538;
                MyName1 = $"{GetType().Name} object";
            }
            public override string ToString()
            {
                return $"{GetType().Name}: ID = {MyId1}, Name = \"{MyName1}\".";
            }
            public int MyId1 { get; set; }
            public string MyName1 { get; set; }

            // Implementations for equality comparisson:
            public override bool Equals(object obj) => this.Equals(obj as ExplicitlyConvertibleToDerived);
            public bool Equals(ExplicitlyConvertibleToDerived compared)
            {
                if (compared is null)
                {
                    return false;
                }
                // Optimization for a common success case.
                if (Object.ReferenceEquals(this, compared))
                {
                    return true;
                }
                // If run-time types are not exactly the same, return false.
                if (this.GetType() != compared.GetType())
                {
                    return false;
                }
                // Return true if the fields match.
                // Note that the base class is not invoked because it is
                // System.Object, which defines Equals as reference equality.
                return (MyId1 == compared.MyId1) && (MyName1 == compared.MyName1);
            }
            public override int GetHashCode() => (MyId1, MyName1).GetHashCode();
            public static bool operator ==(ExplicitlyConvertibleToDerived lhs, ExplicitlyConvertibleToDerived rhs)
            {
                if (lhs is null)
                {
                    if (rhs is null)
                    {
                        return true;
                    }
                    // Only the left side is null.
                    return false;
                }
                // Equals handles case of null on right side.
                return lhs.Equals(rhs);
            }
            public static bool operator !=(ExplicitlyConvertibleToDerived lhs, ExplicitlyConvertibleToDerived rhs) => !(lhs == rhs);
        }

        protected class ImplicitlyConvertibleToDerived
        {
            public ImplicitlyConvertibleToDerived()
            {
                MyId2 = 825;
                MyName2 = $"{GetType().Name} object";
                MyDescription2 = $"This is an instance of {GetType().Name}.";
            }
            public override string ToString()
            {
                return $"{GetType().Name}: MyId2 = {MyId2}, MyName2 = \"{MyName2}\", MyDescription2 \"{MyDescription2}\".";
            }
            public int MyId2 { get; set; }
            public string MyName2 { get; set; }
            public string MyDescription2 { get; set; }

            // Implementations for equality comparisson:
            public override bool Equals(object obj) => this.Equals(obj as ImplicitlyConvertibleToDerived);
            public bool Equals(ImplicitlyConvertibleToDerived compared)
            {
                if (compared is null)
                {
                    return false;
                }
                // Optimization for a common success case.
                if (Object.ReferenceEquals(this, compared))
                {
                    return true;
                }
                // If run-time types are not exactly the same, return false.
                if (this.GetType() != compared.GetType())
                {
                    return false;
                }
                // Return true if the fields match.
                // Note that the base class is not invoked because it is
                // System.Object, which defines Equals as reference equality.
                return (MyId2 == compared.MyId2) && (MyName2 == compared.MyName2) && (MyDescription2 == compared.MyDescription2);
            }
            public override int GetHashCode() => (MyId2, MyName2, MyDescription2).GetHashCode();
            public static bool operator ==(ImplicitlyConvertibleToDerived lhs, ImplicitlyConvertibleToDerived rhs)
            {
                if (lhs is null)
                {
                    if (rhs is null)
                    {
                        return true;
                    }
                    // Only the left side is null.
                    return false;
                }
                // Equals handles case of null on right side.
                return lhs.Equals(rhs);
            }
            public static bool operator !=(ImplicitlyConvertibleToDerived lhs, ImplicitlyConvertibleToDerived rhs) => !(lhs == rhs);
        }



        protected class ExplicitlyConvertibleFromDerived
        {
            public ExplicitlyConvertibleFromDerived()
            {
                MyId3 = 235;
                MyName3 = $"{GetType().Name} object";
            }
            public override string ToString()
            {
                return $"{GetType().Name}: ID = {MyId3}, Name = \"{MyName3}\".";
            }
            public int MyId3 { get; set; }
            public string MyName3 { get; set; }
            public static explicit operator ExplicitlyConvertibleFromDerived(DerivedClass source)
            {
                return new ExplicitlyConvertibleFromDerived
                {
                    MyId3 = source.ID,
                    MyName3 = source.Name,
                };
            }

            // Implementations for equality comparisson:
            public override bool Equals(object obj) => this.Equals(obj as ExplicitlyConvertibleFromDerived);
            public bool Equals(ExplicitlyConvertibleFromDerived compared)
            {
                if (compared is null)
                {
                    return false;
                }
                // Optimization for a common success case.
                if (Object.ReferenceEquals(this, compared))
                {
                    return true;
                }
                // If run-time types are not exactly the same, return false.
                if (this.GetType() != compared.GetType())
                {
                    return false;
                }
                // Return true if the fields match.
                // Note that the base class is not invoked because it is
                // System.Object, which defines Equals as reference equality.
                return (MyId3 == compared.MyId3) && (MyName3 == compared.MyName3);
            }
            public override int GetHashCode() => (MyId3, MyName3).GetHashCode();
            public static bool operator ==(ExplicitlyConvertibleFromDerived lhs, ExplicitlyConvertibleFromDerived rhs)
            {
                if (lhs is null)
                {
                    if (rhs is null)
                    {
                        return true;
                    }
                    // Only the left side is null.
                    return false;
                }
                // Equals handles case of null on right side.
                return lhs.Equals(rhs);
            }
            public static bool operator !=(ExplicitlyConvertibleFromDerived lhs, ExplicitlyConvertibleFromDerived rhs) => !(lhs == rhs);
        }



        protected class ImplicitlyConvertibleFromDerived
        {
            public ImplicitlyConvertibleFromDerived()
            {
                MyId4 = 816;
                MyName4 = $"{GetType().Name} object";
                MyDescription4 = $"This is an instance of {GetType().Name}.";
            }
            public override string ToString()
            {
                return $"{GetType().Name}: MyId4 = {MyId4}, MyName4 = \"{MyName4}\", MyDescription4 \"{MyDescription4}\".";
            }
            public int MyId4 { get; set; }
            public string MyName4 { get; set; }
            public string MyDescription4 { get; set; }
            public static implicit operator ImplicitlyConvertibleFromDerived(DerivedClass source)
            {
                return new ImplicitlyConvertibleFromDerived
                {
                    MyId4 = source.ID,
                    MyName4 = source.Name,
                    MyDescription4 = source.Description,
                };
            }

            // Implementations for equality comparisson:
            public override bool Equals(object obj) => this.Equals(obj as ImplicitlyConvertibleFromDerived);
            public bool Equals(ImplicitlyConvertibleFromDerived compared)
            {
                if (compared is null)
                {
                    return false;
                }
                // Optimization for a common success case.
                if (Object.ReferenceEquals(this, compared))
                {
                    return true;
                }
                // If run-time types are not exactly the same, return false.
                if (this.GetType() != compared.GetType())
                {
                    return false;
                }
                // Return true if the fields match.
                // Note that the base class is not invoked because it is
                // System.Object, which defines Equals as reference equality.
                return (MyId4 == compared.MyId4) && (MyName4 == compared.MyName4) && (MyDescription4 == compared.MyDescription4);
            }
            public override int GetHashCode() => (MyId4, MyName4, MyDescription4).GetHashCode();
            public static bool operator ==(ImplicitlyConvertibleFromDerived lhs, ImplicitlyConvertibleFromDerived rhs)
            {
                if (lhs is null)
                {
                    if (rhs is null)
                    {
                        return true;
                    }
                    // Only the left side is null.
                    return false;
                }
                // Equals handles case of null on right side.
                return lhs.Equals(rhs);
            }
            public static bool operator !=(ImplicitlyConvertibleFromDerived lhs, ImplicitlyConvertibleFromDerived rhs) => !(lhs == rhs);
        }





        #endregion ClassesForTests


    }

}


