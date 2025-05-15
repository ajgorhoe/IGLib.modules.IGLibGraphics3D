using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


    // In the beginning of private object ConvertRectangularArray(object value, Type targetArrayType):
    // Remark: condition below was previously "!targetArrayType.IsArray || !targetArrayType.IsSZArray && !targetArrayType.IsArray",
    // but IsSZArray check is redundant becaulse logic already filters arrays based on targetArrayType.GetArrayRank()
    // and element access. "!targetArrayType.IsArray" should be sufficient.
    //if (!targetArrayType.IsArray)
    //    throw new InvalidOperationException("Target type is not a rectangular array.");



namespace IGLib.Core
{


    /// <remarks>This class was in place before a deeper analysis, and it does not correctly convert rectangular arrays
    /// to List{T}>.</remarks>
    /// 
    /// <summary>
    /// A type converter that supports conversion of basic and collection types,
    /// including arrays, lists, and rectangular (multidimensional) arrays.
    /// <para>Takes a value of type <see cref="object"/>, identifies its type, converts it to the type specified target type,
    /// and returns the converted value as <see cref="object"/>.</para>
    /// This class extends capabilities of <see cref="BasicTypeConverter"/> with additional capabilities for array-like objects.
    /// <para>Capabilities:</para>
    /// <para>* All capabilities of <see cref="BasicTypeConverter"/> (e.g. conversion of types that implement IConvertible interface),
    /// like int ↔ double, etc., double ↔ string, etc., derived class → base class.</para>
    /// <para>* Various array-like conversios, with conversion of element type supported by <see cref="BasicTypeConverter"/>, if necessary:</para>
    /// <para>* 1D ↔ 1D (TSource[], List<TSource> ↔ TTarget[], List<TTarget>)</para>
    /// <para>* Multidimensional → 1D conversions (TSource[,,...] ↔ TTarget[] or List<TTarget>)</para>
    /// <para>* Multidimensional ↔ Multidimensional (only if rank and dimensions match exactly)</para>
    /// </summary>
    [Obsolete("Will be removed, kept as reference for some time (e.g. to transer useful comments and for testing).")]
    public class CollectionTypeConverter_OLD_ToRemoveLater : BasicTypeConverter
    {
        /// <summary>
        /// Converts a value to the specified target type, supporting collections. Conversion is bsed on the actual
        /// type of <paramref name="value"/> and the specified target type (<paramref name="targetType"/>). See the
        /// class description for more details.
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

            if (IsAssignableToGenericIList(targetCollectionType, targetElementType))
            {
                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(targetElementType));
                foreach (var item in convertedItems)
                    list.Add(item);
                return list;
            }

            throw new InvalidOperationException($"Unsupported collection target type: {targetCollectionType.FullName}");
        }

        /// <summary>
        /// Converts a rectangular array to another rectangular or 1D collection.
        /// </summary>
        /// <param name="value">The source array value.</param>
        /// <param name="targetArrayType">The target array type.</param>
        /// <returns>The converted array.</returns>
        private object ConvertRectangularArray(object value, Type targetArrayType)
        {
            Array sourceArray = (Array)value;

            // Flatten the source array
            var flatSource = FlattenRectangularArray(sourceArray).ToArray();

            if (!targetArrayType.IsArray)
                throw new InvalidOperationException("Target type is not an array.");

            Type elementType = targetArrayType.GetElementType();
            int rank = targetArrayType.GetArrayRank();

            if (rank == 1)
            {
                Array targetArray = Array.CreateInstance(elementType, flatSource.Length);
                for (int i = 0; i < flatSource.Length; i++)
                {
                    targetArray.SetValue(ConvertToType(flatSource[i], elementType), i);
                }
                return targetArray;
            }
            else
            {
                int[] dims = Enumerable.Range(0, sourceArray.Rank)
                                       .Select(sourceArray.GetLength)
                                       .ToArray();
                int total = dims.Aggregate(1, (a, b) => a * b);

                if (flatSource.Length != total)
                    throw new InvalidOperationException("Element count mismatch during array reshaping.");

                Array targetArray = Array.CreateInstance(elementType, dims);
                var indexGen = new IncludeFailedTestsByDesignOld(dims);

                int counter = 0;
                foreach (int[] index in indexGen)
                {
                    var valueToSet = ConvertToType(flatSource[counter++], elementType);
                    targetArray.SetValue(valueToSet, index);
                }

                return targetArray;
            }
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
            var indexGen = new IncludeFailedTestsByDesignOld(dims);

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
            typeof(IList<>).MakeGenericType(elementType).IsAssignableFrom(type);

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
    }

    /// <summary>
    /// Utility class for generating indices for multidimensional arrays.
    /// </summary>
    public class IncludeFailedTestsByDesignOld : IEnumerable<int[]>
    {
        private readonly int[] _dimensions;

        /// <summary>
        /// Initializes a new instance with specified dimensions.
        /// </summary>
        /// <param name="dimensions">Array of dimension lengths.</param>
        public IncludeFailedTestsByDesignOld(int[] dimensions)
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
