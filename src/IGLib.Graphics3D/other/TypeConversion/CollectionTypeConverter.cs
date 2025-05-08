using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IGLib.Core
{

    public class CollectionTypeConverter : BasicTypeConverter
    {
        public override object ConvertToType(object value, Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            if (value == null)
            {
                if (IsNullableType(targetType)) return null;
                throw new InvalidOperationException($"Cannot assign null to non-nullable type {targetType.FullName}.");
            }

            Type actualTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            // Handle collection conversion
            if (IsEnumerableButNotString(value.GetType()) && IsEnumerableButNotString(actualTargetType))
            {
                try
                {
                    return ConvertCollection(value, actualTargetType);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert collection of type {value.GetType()} to {targetType}.", ex);
                }
            }

            // Fallback to base converter
            return base.ConvertToType(value, targetType);
        }

        private object ConvertCollection(object value, Type targetCollectionType)
        {
            Type sourceElementType = GetElementType(value.GetType()) ?? typeof(object);
            Type targetElementType = GetElementType(targetCollectionType) ?? typeof(object);

            var sourceEnumerable = ((IEnumerable)value).Cast<object>();

            // Recursively convert each element
            var convertedItems = sourceEnumerable
                .Select(item => ConvertToType(item, targetElementType))
                .ToList();

            // Convert to array
            if (targetCollectionType.IsArray)
            {
                var array = Array.CreateInstance(targetElementType, convertedItems.Count);
                for (int i = 0; i < convertedItems.Count; i++)
                {
                    array.SetValue(convertedItems[i], i);
                }
                return array;
            }

            // Convert to List<T>
            if (IsGenericList(targetCollectionType, targetElementType))
            {
                var list = (IList)Activator.CreateInstance(targetCollectionType);
                foreach (var item in convertedItems)
                {
                    list.Add(item);
                }
                return list;
            }

            throw new InvalidOperationException($"Unsupported collection target type: {targetCollectionType.FullName}");
        }

        private static bool IsEnumerableButNotString(Type type) =>
            typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);

        private static bool IsGenericList(Type type, Type elementType) =>
            type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(List<>) &&
            type.GetGenericArguments()[0].IsAssignableFrom(elementType);

        private static Type GetElementType(Type type)
        {
            if (type.IsArray) return type.GetElementType();

            var enumerableInterface = type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return enumerableInterface?.GetGenericArguments()[0];
        }
    }

}
