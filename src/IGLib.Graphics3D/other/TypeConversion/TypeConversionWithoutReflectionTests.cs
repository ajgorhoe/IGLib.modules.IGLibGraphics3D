using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
// using System.Reflection;


namespace IGLib.Core
{

    public class TypeConversionWithoutReflectionTests
    {

        public object ConvertToType(
            object value,
            Type targetType,
            bool enableNullableHandling = true,
            bool enableCollectionConversion = false)
        {
            object result = null;
            result = value;

            if (value != null)
            {
                Type resultType = result.GetType();

                if (resultType != targetType)
                {
                    Console.WriteLine($"Type of the assigned object ({resultType.Name}) is diffrent than required ({targetType.Name})");
                }
                if (! targetType.IsAssignableFrom(resultType))
                {
                    throw new InvalidCastException($"Cannot assign object of type {resultType.Name} to object that should be of type {targetType.Name}.");
                }


            }

            return result;
        }

    }

}
