using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGLib.Core.CollectionExtensions_OLD
{

    public static class CollectionExtensions_OLD
    {

        public const string NullString = "null";


        /// <summary>Converts the specified object <paramref name="o"/> to a readable string.
        /// <para>This method dynamically checks the actual type of <paramref name="o"/>. If the object
        /// is an array, 2 or 3 dimensional rectangular array, 2 or 3 dimensional ljagged array, a
        /// list (it implwments the <see cref="IList"/> interface) or enumerable (it implements the
        /// <see cref="IEnumerable"/> interface, then the appropriate method is called to produce
        /// the string representation suitable for array-like objectrs.</para></summary>
        /// <param name="o">Object to be converted to a readable string.</param>
        /// <returns>Ths string containing a representation of the <paramref name="o"/>,
        /// which is also nicely structured and readable for objects of array-like types.</returns>
        public static string ToReadableString(this object o)
        {
            // Null check
            if (o == null)
                return "null";

            // Handle jagged arrays (1D array of arrays or deeper)
            if (o is Array jaggedArray && jaggedArray.GetType().GetElementType()?.IsArray == true)
            {
                return HandleJaggedArray(jaggedArray);
            }

            // Handle 1D flat arrays
            if (o is Array array && array.Rank == 1)
            {
                return array.Cast<object>().ToArray().ToReadableString();
            }

            // Handle 2D rectangular arrays
            if (o is Array array2D && array2D.Rank == 2)
            {
                return CastAndCallToReadableString(array2D);
            }

            // Handle 3D rectangular arrays
            if (o is Array array3D && array3D.Rank == 3)
            {
                return CastAndCallToReadableString(array3D);
            }

            // Handle IList
            if (o is IList list)
            {
                return list.Cast<object>().ToReadableString();
            }

            // Handle IEnumerable
            if (o is IEnumerable enumerable)
            {
                return enumerable.Cast<object>().ToReadableString();
            }

            // Fallback for non-collection types
            return o.ToString();
        }

        private static string CastAndCallToReadableString(Array array)
        {
            if (array is int[,] int2DArray)
            {
                return int2DArray.ToReadableString();
            }
            if (array is int[,,] int3DArray)
            {
                return int3DArray.ToReadableString();
            }
            if (array is string[,] string2DArray)
            {
                return string2DArray.ToReadableString();
            }
            if (array is string[,,] string3DArray)
            {
                return string3DArray.ToReadableString();
            }
            // Add more types as needed
            return array.ToString(); // Fallback for unsupported types
        }

        /// <summary>A helper method to handle jagged arrays dynamically.</summary>
        /// <param name="jaggedArray">The jagged array whose string representation should be returned.</param>
        /// <returns>String representation of the specified jagged array.</returns>
        private static string HandleJaggedArray(Array jaggedArray, int indentLevel = 0)
        {
            var sb = new StringBuilder();
            string indent = new string(' ', indentLevel * 4); // Indentation string based on the current level

            sb.Append(indent + "{\n");
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                var element = jaggedArray.GetValue(i);

                if (element is Array innerArray && innerArray.GetType().GetElementType()?.IsArray == true)
                {
                    // Recursively handle nested jagged arrays
                    sb.Append(HandleJaggedArray((Array)element, indentLevel + 1));
                }
                else if (element is Array inner1DArray)
                {
                    // Handle 1D arrays
                    sb.Append(new string(' ', (indentLevel + 1) * 4));
                    sb.Append(inner1DArray.Cast<object>().ToArray().ToReadableString());
                }

                // Append a comma unless it's the last element
                if (i < jaggedArray.Length - 1)
                {
                    sb.Append(",");
                }

                sb.Append("\n");
            }
            sb.Append(indent + "}");
            return sb.ToString();
        }


        /// <summary>Converts 1D  array to a readable string.</summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="array">Array to be converted.</param>
        public static string ToReadableString<T>(this T[] array)
        {
            if (array == null)
            {
                return NullString;
            }
            return $"{{{string.Join(", ", array)}}}";
        }


        /// <summary>Converts an <see cref="IList{T}"/> object to a readable string.</summary>
        /// <typeparam name="T">Type of list elements.</typeparam>
        /// <param name="list"><see cref="IList{T}"/> object to be converted.</param>
        public static string ToReadableString<T>(this IList<T> list)
        {
            return $"{{{string.Join(", ", list)}}}";
        }


        // Extension method for generic IEnumerable<T>
        /// <summary>Converts an <see cref="IEnumerable{T}"/> object to a readable string.</summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="enumerable"><see cref="IEnumerable{T}"/> object to be converted.</param>
        public static string ToReadableString<T>(this IEnumerable<T> enumerable)
        {
            return $"{{{string.Join(", ", enumerable)}}}";
        }


        /// <summary>Converts a 2D rectangular array to a readable string.</summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="array">Array to be converted.</param>
        /// <returns>Readable string representation of the 2D rectangular array.</returns>
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
                sb.Append("    {");
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    sb.Append(array[i, j]);
                    if (j < array.GetLength(1) - 1) // Avoid trailing comma
                        sb.Append(", ");
                }
                sb.Append("}");
                if (i < array.GetLength(0) - 1) // Avoid trailing comma
                    sb.Append(",");
                sb.Append("\n");
            }
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>Converts a 3D rectangular array to a readable string.</summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="array">Array to be converted.</param>
        /// <returns>String representation of the 3D rectangular array.</returns>
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
                    sb.Append("        {");
                    for (int k = 0; k < array.GetLength(2); k++)
                    {
                        sb.Append(array[i, j, k]);
                        if (k < array.GetLength(2) - 1) // Avoid trailing comma
                            sb.Append(", ");
                    }
                    sb.Append("}");
                    if (j < array.GetLength(1) - 1) // Avoid trailing comma
                        sb.Append(",");
                    sb.Append("\n");
                }
                sb.Append("    }");
                if (i < array.GetLength(0) - 1) // Avoid trailing comma
                    sb.Append(",");
                sb.Append("\n");
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
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                sb.Append("    ");
                sb.Append(jaggedArray[i].ToReadableString()); // Reuse 1D array method
                if (i < jaggedArray.Length - 1) // Avoid trailing comma
                    sb.Append(",");
                sb.Append("\n");
            }
            sb.Append("}");
            return sb.ToString();
        }

        public static string ToReadableString<T>(this T[][][] jaggedArray)
        {
            if (jaggedArray == null)
            {
                return NullString;
            }
            var sb = new StringBuilder();
            sb.Append("{\n");
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                sb.Append("    {\n");
                for (int j = 0; j < jaggedArray[i].Length; j++)
                {
                    sb.Append("        ");
                    sb.Append(jaggedArray[i][j].ToReadableString()); // Reuse 1D array method
                    if (j < jaggedArray[i].Length - 1) // Avoid trailing comma
                        sb.Append(",");
                    sb.Append("\n");
                }
                sb.Append("    }");
                if (i < jaggedArray.Length - 1) // Avoid trailing comma
                    sb.Append(",");
                sb.Append("\n");
            }
            sb.Append("}");
            return sb.ToString();
        }


    }

}