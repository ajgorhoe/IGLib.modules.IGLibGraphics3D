using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IGLib.Core
{


    /// <summary>
    /// A type converter that supports conversion of basic and collection types,
    /// including arrays, lists, rectangular (multidimensional) arrays, and jagged arrays.
    /// </summary>
    public class CollectionTypeConverter_BeforeFinalizing : BasicTypeConverter
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
            Type sourceType = value.GetType();

            if (actualTargetType.IsInstanceOfType(value))
                return value;

            if (IsJaggedArray(sourceType))
            {
                try
                {
                    return ConvertFromJaggedArray((Array)value, actualTargetType);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert jagged array from {sourceType} to {targetType}.", ex);
                }
            }

            if (IsRectangularArray(sourceType))
            {
                try
                {
                    return ConvertFromRectangularArray((Array)value, actualTargetType);
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


        // Auxiliary methods for array-like conversions:

        /// <summary>
        /// Converts an enumerable collection to the specified target collection type.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <param name="targetCollectionType">The target collection type.</param>
        /// <returns>The converted collection.</returns>
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

            if (IsAssignableToGenericIList(targetCollectionType, targetElementType) ||
                IsAssignableToGenericIEnumerable(targetCollectionType, targetElementType))
            {
                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(targetElementType));
                foreach (var item in convertedItems)
                    list.Add(item);
                return list;
            }

            throw new InvalidOperationException($"Unsupported collection target type: {targetCollectionType.FullName}");
        }

        /// <summary>
        /// Converts a rectangular array to another rectangular or one-dimensional collection.
        /// </summary>
        /// <param name="sourceArray">The source rectangular array.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>The converted object.</returns>
        private object ConvertFromRectangularArray(Array sourceArray, Type targetType)
        {
            Type targetElementType = GetElementType(targetType) ?? typeof(object);

            if (targetType.IsArray)
            {
                int targetRank = targetType.GetArrayRank();

                if (targetRank == 1)
                {
                    // Convert to 1D array
                    var flatSource = FlattenRectangularArray(sourceArray).ToList();
                    Array targetArray = Array.CreateInstance(targetElementType, flatSource.Count);
                    for (int i = 0; i < flatSource.Count; i++)
                    {
                        targetArray.SetValue(ConvertToType(flatSource[i], targetElementType), i);
                    }
                    return targetArray;
                }
                else if (sourceArray.Rank == targetRank)
                {
                    // Convert to same-shape rectangular array
                    int[] dims = Enumerable.Range(0, sourceArray.Rank)
                                           .Select(sourceArray.GetLength)
                                           .ToArray();
                    int total = dims.Aggregate(1, (a, b) => a * b);
                    var flatSource = FlattenRectangularArray(sourceArray).ToList();

                    if (flatSource.Count != total)
                        throw new InvalidOperationException("Element count mismatch during array reshaping.");

                    Array targetArray = Array.CreateInstance(targetElementType, dims);
                    var indexGen = new MultiDimensionalIndexGenerator_BeforeFinalizing(dims);

                    int counter = 0;
                    foreach (int[] index in indexGen)
                    {
                        var valueToSet = ConvertToType(flatSource[counter++], targetElementType);
                        targetArray.SetValue(valueToSet, index);
                    }

                    return targetArray;
                }
                else
                {
                    throw new InvalidOperationException("Target and source array ranks do not match for rectangular conversion.");
                }
            }
            else if (IsAssignableToGenericIList(targetType, targetElementType) ||
                     IsAssignableToGenericIEnumerable(targetType, targetElementType))
            {
                // Flatten to List<T>
                var flatSource = FlattenRectangularArray(sourceArray).ToList();
                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(targetElementType));
                foreach (var item in flatSource)
                {
                    list.Add(ConvertToType(item, targetElementType));
                }
                return list;
            }

            throw new InvalidOperationException($"Unsupported rectangular array target type: {targetType.FullName}");
        }

        /// <summary>
        /// Flattens a rectangular array into a sequence of objects.
        /// </summary>
        /// <param name="array">The rectangular array to flatten.</param>
        /// <returns>An IEnumerable of the array elements.</returns>
        private IEnumerable<object> FlattenRectangularArray(Array array)
        {
            int[] dims = Enumerable.Range(0, array.Rank)
                                   .Select(array.GetLength)
                                   .ToArray();
            var indexGen = new MultiDimensionalIndexGenerator_BeforeFinalizing(dims);

            foreach (int[] index in indexGen)
                yield return array.GetValue(index);
        }

        /// <summary>
        /// Checks if the type is a rectangular (multidimensional) array.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if rectangular array, false otherwise.</returns>
        private static bool IsRectangularArray(Type type) =>
            type.IsArray && type.GetArrayRank() > 1;

        /// <summary>
        /// Checks if a type is enumerable but not a string.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if type is enumerable but not string.</returns>
        private static bool IsEnumerableButNotString(Type type) =>
            typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);

        /// <summary>
        /// Checks if a type is assignable to IList&lt;T&gt;.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="elementType">The element type.</param>
        /// <returns>True if assignable to IList&lt;T&gt;.</returns>
        private static bool IsAssignableToGenericIList(Type type, Type elementType) =>
            type.IsGenericType &&
            typeof(IList<>).MakeGenericType(elementType).IsAssignableFrom(type);

        /// <summary>
        /// Checks if a type is assignable to IEnumerable&lt;T&gt;.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="elementType">The element type.</param>
        /// <returns>True if assignable to IEnumerable&lt;T&gt;.</returns>
        private static bool IsAssignableToGenericIEnumerable(Type type, Type elementType) =>
            type.IsGenericType &&
            typeof(IEnumerable<>).MakeGenericType(elementType).IsAssignableFrom(type);

        /// <summary>
        /// Gets the element type of a collection type.
        /// </summary>
        /// <param name="type">The collection type.</param>
        /// <returns>The element type if available, otherwise null.</returns>
        private static Type GetElementType(Type type)
        {
            if (type.IsArray) return type.GetElementType();

            var enumerableInterface = type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return enumerableInterface?.GetGenericArguments()[0];
        }


        // Auxiliary methods for jagged arrays:

        private object ConvertFromJaggedArray(Array sourceArray, Type targetType)
        {
            var flatSource = FlattenJaggedArray(sourceArray).ToList();
            Type targetElementType = GetElementType(targetType) ?? typeof(object);

            if (targetType.IsArray && targetType.GetArrayRank() == 1)
            {
                Array targetArray = Array.CreateInstance(targetElementType, flatSource.Count);
                for (int i = 0; i < flatSource.Count; i++)
                {
                    targetArray.SetValue(ConvertToType(flatSource[i], targetElementType), i);
                }
                return targetArray;
            }

            if (IsAssignableToGenericIList(targetType, targetElementType) ||
                IsAssignableToGenericIEnumerable(targetType, targetElementType))
            {
                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(targetElementType));
                foreach (var item in flatSource)
                    list.Add(ConvertToType(item, targetElementType));
                return list;
            }

            if (targetType.IsArray && targetType.GetElementType()?.IsArray == false)
            {
                var shape = GetJaggedArrayShape(sourceArray);
                if (shape == null)
                    throw new InvalidOperationException("Jagged array cannot be reshaped to a rectangular array due to inconsistent dimensions.");

                int[] dims = shape;
                Array targetArray = Array.CreateInstance(targetElementType, dims);

                var indexGen = new MultiDimensionalIndexGenerator_BeforeFinalizing(dims);
                int counter = 0;
                foreach (int[] index in indexGen)
                {
                    var val = ConvertToType(flatSource[counter++], targetElementType);
                    targetArray.SetValue(val, index);
                }
                return targetArray;
            }

            throw new InvalidOperationException($"Unsupported jagged array target type: {targetType.FullName}");
        }

        private IEnumerable<object> FlattenJaggedArray(Array array)
        {
            foreach (var item in array)
            {
                if (item is Array subArray)
                {
                    foreach (var subItem in FlattenJaggedArray(subArray))
                        yield return subItem;
                }
                else
                {
                    yield return item;
                }
            }
        }

        private static bool IsJaggedArray(Type type) =>
            type.IsArray && type.GetElementType()?.IsArray == true;

        private static int[] GetJaggedArrayShape(Array array)
        {
            List<int> shape = new List<int>();
            while (array is Array current)
            {
                shape.Add(current.Length);
                if (current.Length == 0) break;
                object first = current.GetValue(0);
                if (first is Array next && AllSameLength(current))
                {
                    array = next;
                }
                else
                {
                    break;
                }
            }
            return shape.ToArray();
        }

        private static bool AllSameLength(Array array)
        {
            if (array.Length == 0) return true;
            int? len = null;
            foreach (var item in array)
            {
                if (item is Array subArray)
                {
                    if (len == null) len = subArray.Length;
                    else if (subArray.Length != len) return false;
                }
                else return false;
            }
            return true;
        }

    
    }


    /// <summary>
    /// Utility class for generating indices for multidimensional arrays.
    /// </summary>
    public class MultiDimensionalIndexGenerator_BeforeFinalizing : IEnumerable<int[]>
    {
        private readonly int[] _dimensions;

        /// <summary>
        /// Initializes a new instance with specified dimensions.
        /// </summary>
        /// <param name="dimensions">Array of dimension lengths.</param>
        public MultiDimensionalIndexGenerator_BeforeFinalizing(int[] dimensions)
        {
            _dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
        }

        /// <summary>
        /// Returns an enumerator that iterates over all index combinations.
        /// </summary>
        /// <returns>Enumerator of int[] index combinations.</returns>
        public IEnumerator<int[]> GetEnumerator()
        {
            int rank = _dimensions.Length;
            int[] indices = new int[rank];

            while (true)
            {
                yield return (int[])indices.Clone();

                int k = rank - 1;
                while (k >= 0)
                {
                    indices[k]++;
                    if (indices[k] < _dimensions[k]) break;
                    indices[k] = 0;
                    k--;
                }

                if (k < 0) break;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Gets a flat index for the given multidimensional index.
        /// </summary>
        /// <param name="index">Multidimensional index array.</param>
        /// <returns>Corresponding flat index.</returns>
        public int FlatIndex(int[] index)
        {
            int flatIndex = 0;
            int stride = 1;

            for (int i = _dimensions.Length - 1; i >= 0; i--)
            {
                flatIndex += index[i] * stride;
                stride *= _dimensions[i];
            }

            return flatIndex;
        }
    }


}
