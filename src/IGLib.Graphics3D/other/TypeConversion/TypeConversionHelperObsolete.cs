using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IGLib.Core
{

    [Obsolete("Teplaced by TypeConverter classes  implementing ITypeConverter.")]
    public class TypeConversionHelper // : TypeConversionHelperBase
    {

        private static readonly Lazy<TypeConversionHelper> _instance = new(() => new TypeConversionHelper());
        public static TypeConversionHelper Instance => _instance.Value;

        // Allows "using static TypeConversionHelper;" for convenient access
        public static TypeConversionHelper TypeConversionHelperInstance => Instance;


        /// <summary>Used to cache <see cref="MethodInfo"/> objects used for type conversions.</summary>
        protected readonly ConcurrentDictionary<(Type SourceType, Type TargetType), MethodInfo> _castMethodCache = new();

        public TypeConversionHelper() { }


        public object ConvertToType(
            object value,
            Type targetType,
            bool enableNullableHandling = true,
            bool enableCollectionConversion = false)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            if (value == null)
            {
                if (targetType.IsValueType && Nullable.GetUnderlyingType(targetType) == null)
                {
                    throw new InvalidOperationException($"Cannot assign null to non-nullable type {targetType.FullName}.");
                }
                return null;
            }

            Type actualTargetType = targetType;
            if (enableNullableHandling)
            {
                actualTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            }

            if (actualTargetType.IsInstanceOfType(value))
            {
                return value; // Already correct type
            }

            Type sourceType = value.GetType();

            // Handle collection conversion if enabled
            if (enableCollectionConversion &&
                IsEnumerableButNotString(sourceType) &&
                IsEnumerableButNotString(actualTargetType))
            {
                return ConvertCollections(value, sourceType, actualTargetType, enableNullableHandling, enableCollectionConversion);
            }

            try
            {
                if (value is IConvertible && typeof(IConvertible).IsAssignableFrom(actualTargetType))
                {
                    return Convert.ChangeType(value, actualTargetType);
                }

                var castMethod = _castMethodCache.GetOrAdd((sourceType, actualTargetType), key =>
                {
                    var (srcType, tgtType) = key;
                    return srcType
                        .GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .FirstOrDefault(m =>
                            (m.Name == "op_Implicit" || m.Name == "op_Explicit") &&
                            m.ReturnType == tgtType);
                });

                if (castMethod != null)
                {
                    return castMethod.Invoke(null, new[] { value });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to convert value of type {sourceType.FullName} to type {targetType.FullName}.", ex);
            }

            throw new InvalidOperationException($"Cannot assign value of type {sourceType.FullName} to type {targetType.FullName}.");
        }

        private static bool IsEnumerableButNotString(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
        }

        private object ConvertCollections(
            object value,
            Type sourceType,
            Type targetType,
            bool enableNullableHandling,
            bool enableCollectionConversion)
        {
            Type sourceElementType = GetEnumerableElementType(sourceType) ?? typeof(object);
            Type targetElementType = GetEnumerableElementType(targetType) ?? typeof(object);

            var enumerable = ((IEnumerable)value).Cast<object>();

            var convertedList = enumerable
                .Select(item => ConvertToType(item, targetElementType, enableNullableHandling, enableCollectionConversion))
                .ToList();

            if (targetType.IsArray)
            {
                Array array = Array.CreateInstance(targetElementType, convertedList.Count);
                for (int i = 0; i < convertedList.Count; i++)
                {
                    array.SetValue(convertedList[i], i);
                }
                return array;
            }

            if (typeof(IList).IsAssignableFrom(targetType))
            {
                IList list = (IList)Activator.CreateInstance(targetType);
                foreach (var item in convertedList)
                {
                    list.Add(item);
                }
                return list;
            }

            throw new InvalidOperationException($"Unsupported collection conversion from {sourceType.FullName} to {targetType.FullName}.");
        }

        private static Type GetEnumerableElementType(Type enumerableType)
        {
            if (enumerableType.IsArray)
                return enumerableType.GetElementType();

            var iface = enumerableType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return iface?.GetGenericArguments()[0];
        }
    }
}
