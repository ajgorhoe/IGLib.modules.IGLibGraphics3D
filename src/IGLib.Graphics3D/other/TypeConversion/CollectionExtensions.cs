using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGLib.Core.CollectionExtensions
{

    public static class CollectionExtensions
    {

        public const string NullString = "null";

        /// <summary>Converts 1D  array to a readable string.</summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="array">Array to be converted.</param>
        public static string ToReadableString<T>(this T[] array)
        {
            if (array == null)
            {
                return NullString;
            }
            return $"[ {string.Join(", ", array)} ]";
        }


        // Extension method for generic IList<T>
        /// <summary>Converts an <see cref="IList{T}"/> object to a readable string.</summary>
        /// <typeparam name="T">Type of list elements.</typeparam>
        /// <param name="list"><see cref="IList{T}"/> object to be converted.</param>
        public static string ToReadableString<T>(this IList<T> list)
        {
            return $"[ {string.Join(", ", list)} ]";
        }


        // Extension method for generic IEnumerable<T>
        /// <summary>Converts an <see cref="IEnumerable{T}"/> object to a readable string.</summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="enumerable"><see cref="IEnumerable{T}"/> object to be converted.</param>
        public static string ToReadableString<T>(this IEnumerable<T> enumerable)
        {
            return $"[ {string.Join(", ", enumerable)} ]";
        }


        /// <summary>Converts a 2D rectangular array to a readable string.</summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="array">Array to be converted.</param>
        /// <returns></returns>
        public static string ToReadableString<T>(this T[,] array)
        {
            if (array == null)
            {
                return NullString;
            }
            var sb = new StringBuilder();
            sb.Append("{\n");
            for (int i = 0; i < array.GetLength(0); i++)
            {
                sb.Append("    { ");
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    sb.Append(array[i, j]);
                    if (j < array.GetLength(1) - 1)
                        sb.Append(", ");
                }
                sb.Append(" },\n");
            }
            sb.Append("}");
            return sb.ToString();
        }

        // Extension method for 3D rectangular array
        public static string ToReadableString<T>(this T[,,] array)
        {
            if (array == null)
            {
                return NullString;
            }
            var sb = new StringBuilder();
            sb.Append("{\n");
            for (int i = 0; i < array.GetLength(0); i++)
            {
                sb.Append("    {\n");
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    sb.Append("        { ");
                    for (int k = 0; k < array.GetLength(2); k++)
                    {
                        sb.Append(array[i, j, k]);
                        if (k < array.GetLength(2) - 1)
                            sb.Append(", ");
                    }
                    sb.Append(" },\n");
                }
                sb.Append("    },\n");
            }
            sb.Append("}");
            return sb.ToString();
        }

        // Extension method for jagged arrays
        public static string ToReadableString<T>(this T[][] jaggedArray)
        {
            if (jaggedArray == null)
            {
                return NullString;
            }
            var sb = new StringBuilder();
            sb.Append("{\n");
            foreach (var row in jaggedArray)
            {
                sb.Append("    ");
                sb.Append(row.ToReadableString()); // Reuse 1D array method
                sb.Append(",\n");
            }
            sb.Append("}");
            return sb.ToString();
        }

        public static string ToReadableString<T>(this T[][][] jaggedArray)
        {
            var sb = new StringBuilder();
            sb.Append("{\n");
            foreach (var plane in jaggedArray)
            {
                sb.Append("    {\n");
                foreach (var row in plane)
                {
                    sb.Append("        ");
                    sb.Append(row.ToReadableString()); // Reuse 1D array method
                    sb.Append(",\n");
                }
                sb.Append("    },\n");
            }
            sb.Append("}");
            return sb.ToString();
        }


    }

}