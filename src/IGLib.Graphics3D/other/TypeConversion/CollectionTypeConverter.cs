using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IGLib.Core
{


    /// <summary>
    /// A type converter that supports conversion of basic and collection types,
    /// including arrays, lists, and rectangular (multidimensional) arrays.
    /// </summary>
    public class CollectionTypeConverter : BasicTypeConverter
    {
        /// <summary>
        /// Converts a value to the specified target type, supporting collections.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <returns>The converted object.</returns>
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
            Type sourceType = value.GetType();

            if (IsRectangularArray(sourceType) || IsRectangularArray(actualTargetType))
            {
                try
                {
                    return ConvertRectangularArray(value, actualTargetType);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert rectangular array from {sourceType} to {targetType}.", ex);
                }
            }

            if (IsEnumerableButNotString(sourceType) && IsEnumerableButNotString(actualTargetType))
            {
                try
                {
                    return ConvertEnumerable(value, actualTargetType);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert collection from {sourceType} to {targetType}.", ex);
                }
            }

            return base.ConvertToType(value, targetType);
        }

        private object ConvertEnumerable(object value, Type targetCollectionType)
        {
            Type targetElementType = GetElementType(targetCollectionType) ?? typeof(object);
            var sourceEnumerable = ((IEnumerable)value).Cast<object>();

            var convertedItems = sourceEnumerable
                .Select(item => ConvertToType(item, targetElementType))
                .ToList();

            if (targetCollectionType.IsArray)
            {
                var array = Array.CreateInstance(targetElementType, convertedItems.Count);
                for (int i = 0; i < convertedItems.Count; i++)
                    array.SetValue(convertedItems[i], i);
                return array;
            }

            if (IsAssignableToGenericIList(targetCollectionType, targetElementType))
            {
                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(targetElementType));
                foreach (var item in convertedItems)
                    list.Add(item);
                return list;
            }

            throw new InvalidOperationException($"Unsupported collection target type: {targetCollectionType.FullName}");
        }

        private object ConvertRectangularArray(object value, Type targetArrayType)
        {
            // Remark: condition below was previously "!targetArrayType.IsArray || !targetArrayType.IsSZArray && !targetArrayType.IsArray",
            // but IsSZArray check is redundant becaulse logic already filters arrays based on targetArrayType.GetArrayRank()
            // and element access. "!targetArrayType.IsArray" should be sufficient.
            if (!targetArrayType.IsArray)
                throw new InvalidOperationException("Target type is not a rectangular array.");

            Type targetElementType = targetArrayType.GetElementType();
            var sourceArray = value as Array;

            if (sourceArray == null)
                throw new InvalidOperationException("Source value is not an array.");

            if (IsRectangularArray(value.GetType()) && sourceArray.Rank == targetArrayType.GetArrayRank())
            {
                int[] dims = Enumerable.Range(0, sourceArray.Rank).Select(d => sourceArray.GetLength(d)).ToArray();
                var resultArray = Array.CreateInstance(targetElementType, dims);

                foreach (var index in MultiDimensionalIndices(dims))
                {
                    var val = ConvertToType(sourceArray.GetValue(index), targetElementType);
                    resultArray.SetValue(val, index);
                }

                return resultArray;
            }
            else
            {
                // Flatten source and populate multidimensional target
                var flatSource = ((IEnumerable)value).Cast<object>()
                    .Select(item => ConvertToType(item, targetElementType))
                    .ToArray();

                int[] dims = Enumerable.Range(0, targetArrayType.GetArrayRank())
                    .Select(i => (int)targetArrayType.GetMethod("GetLength")!.Invoke(value, new object[] { i })).ToArray();

                int total = dims.Aggregate(1, (a, b) => a * b);
                if (flatSource.Length != total)
                    throw new InvalidOperationException("Element count mismatch during array reshaping.");

                var resultArray = Array.CreateInstance(targetElementType, dims);

                int counter = 0;
                foreach (var index in MultiDimensionalIndices(dims))
                    resultArray.SetValue(flatSource[counter++], index);

                return resultArray;
            }
        }

        private static IEnumerable<int[]> MultiDimensionalIndices(int[] dims)
        {
            int rank = dims.Length;
            int[] indices = new int[rank];
            while (true)
            {
                yield return (int[])indices.Clone();

                int k = rank - 1;
                while (k >= 0)
                {
                    indices[k]++;
                    if (indices[k] < dims[k]) break;
                    indices[k] = 0;
                    k--;
                }

                if (k < 0) break;
            }
        }

        private static bool IsRectangularArray(Type type) =>
            type.IsArray && type.GetArrayRank() > 1;

        private static bool IsEnumerableButNotString(Type type) =>
            typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);

        private static bool IsAssignableToGenericIList(Type type, Type elementType) =>
            typeof(IList<>).MakeGenericType(elementType).IsAssignableFrom(type);

        private static Type GetElementType(Type type)
        {
            if (type.IsArray) return type.GetElementType();

            var enumerableInterface = type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return enumerableInterface?.GetGenericArguments()[0];
        }
    }


}
