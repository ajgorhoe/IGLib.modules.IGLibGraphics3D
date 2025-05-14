using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IGLib.Core
{




    /// <summary>
    /// Provides conversion logic for a wide range of collection types, including:
    /// jagged arrays, rectangular arrays, generic lists, and enumerable sequences.
    /// Supports element-wise conversion and flattening where appropriate.
    /// </summary>
    public class CollectionTypeConverter : BasicTypeConverter
    {
        /// <summary>
        /// Main entry point that dispatches conversion of arrays, lists, and jagged/rectangular collections.
        /// </summary>
        /// <param name="value">The input object to be converted.</param>
        /// <param name="targetType">The desired target type.</param>
        /// <returns>The converted object compatible with the target type.</returns>
        public override object ConvertToType(object value, Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            if (value == null)
            {
                if (targetType.IsValueType && Nullable.GetUnderlyingType(targetType) == null)
                    throw new InvalidOperationException($"Cannot assign null to non-nullable type {targetType.FullName}.");
                return null;
            }

            var sourceType = value.GetType();
            var actualTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (actualTargetType.IsInstanceOfType(value))
                return value;

            // Rectangular array to rectangular array or flatten
            if (sourceType.IsArray && sourceType.GetElementType() != null && sourceType.GetArrayRank() > 1)
            {
                return ConvertRectangularArray(value, actualTargetType);
            }

            // Jagged array handling (T[]...[])
            if (IsJaggedArray(sourceType))
            {
                if (actualTargetType.IsArray && actualTargetType.GetArrayRank() > 1)
                    return ConvertJaggedToRectangular((Array)value, actualTargetType);

                if (actualTargetType.IsArray || ImplementsGenericInterface(actualTargetType, typeof(IEnumerable<>)))
                    return ConvertFromJaggedArray((Array)value, actualTargetType);
            }

            // Rectangular array to jagged array
            if (sourceType.IsArray && sourceType.GetArrayRank() > 1 &&
                actualTargetType.IsArray && IsJaggedArray(actualTargetType))
            {
                return ConvertRectangularToJagged((Array)value, actualTargetType);
            }

            // Generic enumerable conversions
            if (typeof(IEnumerable).IsAssignableFrom(sourceType) &&
                (actualTargetType.IsArray || ImplementsGenericInterface(actualTargetType, typeof(IEnumerable<>))))
            {
                return ConvertEnumerable(value, actualTargetType);
            }

            // Fallback to base logic
            return base.ConvertToType(value, targetType);
        }

        /// <summary>
        /// Checks if a type is or implements a specific generic interface (e.g., IEnumerable&lt;&gt;).
        /// </summary>
        /// <param name="candidateType">The type to check.</param>
        /// <param name="genericInterface">The generic interface definition, e.g., typeof(IEnumerable&lt;&gt;).</param>
        /// <returns>True if the type is or implements the given generic interface.</returns>
        private static bool ImplementsGenericInterface(Type candidateType, Type genericInterface)
        {
            return (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition() == genericInterface)
                || candidateType.GetInterfaces().Any(i =>
                       i.IsGenericType && i.GetGenericTypeDefinition() == genericInterface);
        }







        // Jagged array flattening and Conversion:

        /// <summary>
        /// Converts a jagged array to a 1D array, List&lt;T&gt;, or other flat collection.
        /// </summary>
        /// <param name="sourceArray">The jagged array to flatten and convert.</param>
        /// <param name="targetType">The type of collection to convert to.</param>
        /// <returns>The flattened collection converted to the target type.</returns>
        private object ConvertFromJaggedArray(Array sourceArray, Type targetType)
        {
            Type targetElementType = GetElementType(targetType) ?? typeof(object);
            var flatSource = FlattenJaggedArray(sourceArray).ToList();

            // Early return if target type is 1D array
            if (targetType.IsArray && targetType.GetArrayRank() == 1)
            {
                var resultArray = Array.CreateInstance(targetElementType, flatSource.Count);
                for (int i = 0; i < flatSource.Count; i++)
                {
                    resultArray.SetValue(ConvertToType(flatSource[i], targetElementType), i);
                }
                return resultArray;
            }

            // Generic collection types (List<T>, IEnumerable<T>, IList<T>)
            if (ImplementsGenericInterface(targetType, typeof(IEnumerable<>)))
            {
                var listType = typeof(List<>).MakeGenericType(targetElementType);
                var list = (IList)Activator.CreateInstance(listType);
                foreach (var item in flatSource)
                {
                    list.Add(ConvertToType(item, targetElementType));
                }

                if (targetType.IsAssignableFrom(listType))
                    return list;

                throw new InvalidOperationException(
                    $"Cannot convert jagged array to target type {targetType.FullName}.");
            }

            throw new InvalidOperationException(
                $"Unsupported target type for jagged array conversion: {targetType.FullName}.");
        }

        /// <summary>
        /// Recursively flattens a jagged array into a linear sequence.
        /// </summary>
        /// <param name="array">The jagged array to flatten.</param>
        /// <returns>A flattened sequence of all inner elements.</returns>
        private IEnumerable<object> FlattenJaggedArray(Array array)
        {
            foreach (var item in array)
            {
                if (item is Array nestedArray)
                {
                    foreach (var subItem in FlattenJaggedArray(nestedArray))
                        yield return subItem;
                }
                else
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Determines if the array is jagged (array of arrays).
        /// </summary>
        /// <param name="type">The array type to inspect.</param>
        /// <returns>True if jagged; otherwise false.</returns>
        private bool IsJaggedArray(Type type)
        {
            return type.IsArray && type.GetElementType()?.IsArray == true;
        }

        /// <summary>
        /// Gets the element type of a collection or array.
        /// </summary>
        /// <param name="type">The type to examine.</param>
        /// <returns>The element type, or null if not found.</returns>
        private Type GetElementType(Type type)
        {
            if (type.IsArray)
                return type.GetElementType();

            if (type.IsGenericType &&
                type.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>)))
                return type.GetGenericArguments()[0];

            var enumType = type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return enumType?.GetGenericArguments()[0];
        }







        // Jagged → rectangular conversion

        /// <summary>
        /// Converts a uniform jagged array to a rectangular array if shapes match.
        /// </summary>
        /// <param name="sourceArray">The jagged array with uniform shape.</param>
        /// <param name="targetType">The rectangular array type to convert to.</param>
        /// <returns>A rectangular array with converted elements.</returns>
        private object ConvertJaggedToRectangular(Array sourceArray, Type targetType)
        {
            if (!IsUniformJaggedArray(sourceArray))
                throw new InvalidOperationException("Jagged array does not have uniform shape.");

            Type targetElementType = targetType.GetElementType();
            int rank = targetType.GetArrayRank();
            int[] shape = GetJaggedArrayShape(sourceArray).ToArray();

            if (shape.Length != rank)
                throw new InvalidOperationException("Jagged array rank does not match target rectangular array rank.");

            Array result = Array.CreateInstance(targetElementType, shape);
            var flatValues = FlattenJaggedArray(sourceArray).ToArray();

            if (flatValues.Length != result.Length)
                throw new InvalidOperationException("Mismatch in element count during jagged to rectangular conversion.");

            int index = 0;
            var indexGen = new MultiDimensionalIndexGenerator(shape);

            foreach (var indices in indexGen)
            {
                var convertedValue = ConvertToType(flatValues[index++], targetElementType);
                result.SetValue(convertedValue, indices);
            }

            return result;
        }

        /// <summary>
        /// Checks if all subarrays at each level of a jagged array have equal lengths.
        /// </summary>
        /// <param name="array">The jagged array to validate.</param>
        /// <returns>True if the shape is uniform; otherwise false.</returns>
        private bool IsUniformJaggedArray(Array array)
        {
            return CheckUniform(array, out _);

            bool CheckUniform(Array arr, out int length)
            {
                length = arr.Length;

                if (arr.Length == 0)
                    return true;

                object first = arr.GetValue(0);
                if (first is not Array firstSub) return true;

                int subLength;
                if (!CheckUniform(firstSub, out subLength))
                    return false;

                for (int i = 1; i < arr.Length; i++)
                {
                    object current = arr.GetValue(i);
                    if (current is not Array currArr || currArr.Length != subLength)
                        return false;

                    if (!CheckUniform(currArr, out int _))
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Infers the shape (lengths at each level) of a uniform jagged array.
        /// </summary>
        /// <param name="array">The jagged array.</param>
        /// <returns>A list of dimensions per level.</returns>
        private List<int> GetJaggedArrayShape(Array array)
        {
            var shape = new List<int>();
            object current = array;

            while (current is Array arr)
            {
                shape.Add(arr.Length);
                current = arr.Length > 0 ? arr.GetValue(0) : null;
            }

            return shape;
        }

        /// <summary>
        /// Converts a rectangular array to a jagged array with matching shape and converted values.
        /// </summary>
        /// <param name="sourceArray">The rectangular array to convert.</param>
        /// <param name="targetType">The target jagged array type.</param>
        /// <returns>The jagged array filled with values.</returns>
        private object ConvertRectangularToJagged(Array sourceArray, Type targetType)
        {
            Type targetElementType = GetElementType(targetType);
            int rank = sourceArray.Rank;
            int[] dims = Enumerable.Range(0, rank).Select(sourceArray.GetLength).ToArray();
            var flatValues = FlattenRectangularArray(sourceArray).ToList();

            Array jagged = CreateJaggedArray(targetElementType, dims, 0);
            PopulateJaggedArray(jagged, flatValues, targetElementType, 0, new int[0]);

            return jagged;
        }

        /// <summary>
        /// Creates an empty jagged array of the given shape and element type.
        /// </summary>
        /// <param name="elementType">The leaf type of the array.</param>
        /// <param name="dims">The shape (lengths) of the array at each level.</param>
        /// <param name="level">The current depth level.</param>
        /// <returns>A jagged array with the specified structure.</returns>
        private Array CreateJaggedArray(Type elementType, int[] dims, int level)
        {
            Type arrayType = elementType;
            for (int i = level; i < dims.Length - 1; i++)
                arrayType = arrayType.MakeArrayType();

            Array array = Array.CreateInstance(arrayType, dims[level]);

            if (level < dims.Length - 1)
            {
                for (int i = 0; i < dims[level]; i++)
                {
                    var subArray = CreateJaggedArray(elementType, dims, level + 1);
                    array.SetValue(subArray, i);
                }
            }

            return array;
        }

        /// <summary>
        /// Populates a jagged array with converted values from a flat list.
        /// </summary>
        /// <param name="target">The jagged array to fill.</param>
        /// <param name="values">The flat list of values.</param>
        /// <param name="elementType">The final element type (not intermediate array types).</param>
        /// <param name="level">Current recursion level.</param>
        /// <param name="indexPath">Index tracker for debugging (optional).</param>
        /// <returns>The count of consumed elements.</returns>
        private int PopulateJaggedArray(object target, List<object> values, Type elementType, int level, int[] indexPath)
        {
            int count = 0;
            Array array = (Array)target;
            Type currentElementType = array.GetType().GetElementType();

            bool isLeafLevel = currentElementType == elementType;

            for (int i = 0; i < array.Length; i++)
            {
                if (isLeafLevel)
                {
                    array.SetValue(ConvertToType(values[count++], elementType), i);
                }
                else
                {
                    object sub = array.GetValue(i);
                    count += PopulateJaggedArray(sub, values.Skip(count).ToList(), elementType, level + 1, indexPath.Append(i).ToArray());
                }
            }

            return count;
        }






        // Rectangular array & enumerable

        /// <summary>
        /// Converts any enumerable collection to a target collection type.
        /// </summary>
        /// <param name="value">The source enumerable object.</param>
        /// <param name="targetType">The desired target collection type.</param>
        /// <returns>A new collection with converted elements.</returns>
        private object ConvertEnumerable(object value, Type targetType)
        {
            if (targetType.IsInstanceOfType(value))
                return value;

            Type targetElementType = GetElementType(targetType) ?? typeof(object);
            var enumerable = ((IEnumerable)value).Cast<object>();

            // 1D array
            if (targetType.IsArray && targetType.GetArrayRank() == 1)
            {
                var list = enumerable.Select(x => ConvertToType(x, targetElementType)).ToList();
                var array = Array.CreateInstance(targetElementType, list.Count);
                for (int i = 0; i < list.Count; i++)
                    array.SetValue(list[i], i);
                return array;
            }

            // List<T>, IList<T>, IEnumerable<T>
            if (ImplementsGenericInterface(targetType, typeof(IEnumerable<>)))
            {
                var listType = typeof(List<>).MakeGenericType(targetElementType);
                var list = (IList)Activator.CreateInstance(listType);

                foreach (var item in enumerable)
                {
                    list.Add(ConvertToType(item, targetElementType));
                }

                if (targetType.IsAssignableFrom(listType))
                    return list;

                throw new InvalidOperationException(
                    $"Cannot assign created list to target type {targetType.FullName}.");
            }

            throw new InvalidOperationException(
                $"Unsupported conversion from enumerable to {targetType.FullName}.");
        }

        /// <summary>
        /// Converts rectangular arrays to other arrays or flat collections.
        /// </summary>
        /// <param name="value">The rectangular source array.</param>
        /// <param name="targetArrayType">The target array or collection type.</param>
        /// <returns>The converted collection or array.</returns>
        private object ConvertRectangularArray(object value, Type targetArrayType)
        {
            Array sourceArray = (Array)value;
            var flatSource = FlattenRectangularArray(sourceArray).ToArray();

            Type targetElementType = GetElementType(targetArrayType) ?? typeof(object);

            // Flatten to 1D array
            if (targetArrayType.IsArray && targetArrayType.GetArrayRank() == 1)
            {
                var array = Array.CreateInstance(targetElementType, flatSource.Length);
                for (int i = 0; i < flatSource.Length; i++)
                    array.SetValue(ConvertToType(flatSource[i], targetElementType), i);
                return array;
            }

            // Rectangular to rectangular of same shape
            if (targetArrayType.IsArray && targetArrayType.GetArrayRank() == sourceArray.Rank)
            {
                int[] dims = Enumerable.Range(0, sourceArray.Rank)
                                       .Select(d => sourceArray.GetLength(d))
                                       .ToArray();

                int total = dims.Aggregate(1, (a, b) => a * b);
                if (flatSource.Length != total)
                    throw new InvalidOperationException("Element count mismatch during array reshaping.");

                var result = Array.CreateInstance(targetElementType, dims);
                var indexGen = new MultiDimensionalIndexGenerator(dims);
                int index = 0;

                foreach (var indices in indexGen)
                {
                    var converted = ConvertToType(flatSource[index++], targetElementType);
                    result.SetValue(converted, indices);
                }

                return result;
            }

            // Flatten to List<T>, IEnumerable<T>, IList<T>
            if (ImplementsGenericInterface(targetArrayType, typeof(IEnumerable<>)))
            {
                var listType = typeof(List<>).MakeGenericType(targetElementType);
                var list = (IList)Activator.CreateInstance(listType);

                foreach (var item in flatSource)
                {
                    list.Add(ConvertToType(item, targetElementType));
                }

                // $$ Original condition:
                // if (targetArrayType.IsAssignableFrom(listType))
                if (targetArrayType.IsAssignableFrom(list.GetType()))
                    return list;

                throw new InvalidOperationException(
                    $"Cannot assign list to target type {targetArrayType.FullName}.");
            }

            throw new InvalidOperationException(
                $"Unsupported conversion from rectangular array to {targetArrayType.FullName}.");
        }

        /// <summary>
        /// Flattens a rectangular array to an IEnumerable of its elements.
        /// </summary>
        /// <param name="sourceArray">Rectangular array to flatten.</param>
        /// <returns>Flattened sequence of array elements.</returns>
        private IEnumerable<object> FlattenRectangularArray(Array sourceArray)
        {
            foreach (var item in sourceArray)
                yield return item;
        }






    }



















/// <summary>
/// Utility class for generating indices for multidimensional arrays.
/// </summary>
public class MultiDimensionalIndexGenerator : IEnumerable<int[]>
    {
        private readonly int[] _dimensions;

        /// <summary>
        /// Initializes a new instance with specified dimensions.
        /// </summary>
        /// <param name="dimensions">Array of dimension lengths.</param>
        public MultiDimensionalIndexGenerator(int[] dimensions)
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
