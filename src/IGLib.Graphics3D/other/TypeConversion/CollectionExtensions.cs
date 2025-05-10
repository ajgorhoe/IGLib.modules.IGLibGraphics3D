using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGLib.Core.CollectionExtensions
{
    /// <summary>
    /// Provides extension methods for converting arrays, lists, and enumerable types to readable string representations.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>Open bracket used in the string representation of array-like objects.</summary>
        public const string ArrayBracketOpen = "{";

        /// <summary>Closed bracket used in the string representation of array-like objects.</summary>
        public const string ArrayBracketClosed = "}";

        /// <summary>Array element separator used in the string representation of array-like objects.</summary>
        public const string ArraySeparator = ",";

        /// <summary>Default indentation used in the string representation of array-like objects.</summary>
        public const string ArrayIndentation = "    ";

        /// <summary>String representation for null values.</summary>
        public const string NullString = "null";

        /// <summary>
        /// Converts an object to a readable string by dynamically determining its type.
        /// Handles 1D, 2D, 3D arrays, jagged arrays, lists, and enumerables.
        /// </summary>
        /// <param name="o">The object to be converted.</param>
        /// <param name="indentation">The indentation to use for nested levels (default is "    ").</param>
        /// <param name="openBracket">The open bracket to use (default is "{").</param>
        /// <param name="closedBracket">The closed bracket to use (default is "}").</param>
        /// <param name="separator">The separator to use between elements (default is ",").</param>
        /// <returns>A formatted string representation of the object.</returns>
        public static string ToReadableString(this object o, string indentation = ArrayIndentation, string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed, string separator = ArraySeparator)
        {
            if (o == null)
                return NullString;

            // Handle arrays
            if (o is Array array)
            {
                if (array.Rank == 1)
                    return array.Cast<object>().ToArray().ToReadableString(openBracket, closedBracket, separator);

                if (array.Rank == 2)
                    return ((object[,])array).ToReadableString(indentation, openBracket, closedBracket, separator);

                if (array.Rank == 3)
                    return ((object[,,])array).ToReadableString(indentation, openBracket, closedBracket, separator);

                if (array.GetType().GetElementType()?.IsArray == true)
                    return HandleJaggedArray(array, 0, indentation, openBracket, closedBracket, separator);
            }

            // Handle lists
            if (o is IList list)
                return list.Cast<object>().ToReadableString(openBracket, closedBracket, separator);

            // Handle enumerables
            if (o is IEnumerable enumerable)
                return enumerable.Cast<object>().ToReadableString(openBracket, closedBracket, separator);

            // Handle other objects
            return o.ToString();
        }

        /// <summary>
        /// A helper method to handle jagged arrays dynamically and produce a readable string representation.
        /// </summary>
        /// <param name="jaggedArray">The jagged array whose string representation should be returned.</param>
        /// <param name="indentLevel">The current level of indentation for nested arrays (default is 0).</param>
        /// <param name="indentation">The indentation to use for nested levels (default is "    ").</param>
        /// <param name="openBracket">The open bracket to use (default is "{").</param>
        /// <param name="closedBracket">The closed bracket to use (default is "}").</param>
        /// <param name="separator">The separator to use between elements (default is ",").</param>
        /// <returns>String representation of the specified jagged array.</returns>
        private static string HandleJaggedArray(Array jaggedArray, int indentLevel = 0, string indentation = ArrayIndentation, string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed, string separator = ArraySeparator)
        {
            var sb = new StringBuilder();
            string indent = new string(' ', indentLevel * indentation.Length);

            sb.Append($"{indent}{openBracket}\n");
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                var element = jaggedArray.GetValue(i);

                if (element is Array innerArray && innerArray.GetType().GetElementType()?.IsArray == true)
                {
                    sb.Append(HandleJaggedArray((Array)element, indentLevel + 1, indentation, openBracket, closedBracket, separator));
                }
                else if (element is Array inner1DArray)
                {
                    sb.Append($"{new string(' ', (indentLevel + 1) * indentation.Length)}{inner1DArray.Cast<object>().ToArray().ToReadableString(openBracket, closedBracket, separator)}");
                }

                if (i < jaggedArray.Length - 1)
                    sb.Append($"{separator}\n");
            }
            sb.Append($"\n{indent}{closedBracket}");
            return sb.ToString();
        }

        /// <summary>
        /// Converts a 2D jagged array to a readable string.
        /// </summary>
        public static string ToReadableString<T>(this T[][] jaggedArray, string indentation = ArrayIndentation, string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed, string separator = ArraySeparator)
        {
            if (jaggedArray == null)
                return NullString;

            var sb = new StringBuilder();
            sb.Append($"{openBracket}\n");
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                sb.Append($"{indentation}{jaggedArray[i].ToReadableString(openBracket, closedBracket, separator)}");
                if (i < jaggedArray.Length - 1)
                    sb.Append($"{separator}\n");
            }
            sb.Append($"\n{closedBracket}");
            return sb.ToString();
        }

        /// <summary>
        /// Converts a 3D jagged array to a readable string.
        /// </summary>
        public static string ToReadableString<T>(this T[][][] jaggedArray, string indentation = ArrayIndentation, string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed, string separator = ArraySeparator)
        {
            if (jaggedArray == null)
                return NullString;

            var sb = new StringBuilder();
            sb.Append($"{openBracket}\n");
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                sb.Append($"{indentation}{jaggedArray[i].ToReadableString(indentation, openBracket, closedBracket, separator)}");
                if (i < jaggedArray.Length - 1)
                    sb.Append($"{separator}\n");
            }
            sb.Append($"\n{closedBracket}");
            return sb.ToString();
        }

        // Other methods (e.g., for lists, enumerables, 2D/3D arrays) are already parameterized in the previous version.
    }
}