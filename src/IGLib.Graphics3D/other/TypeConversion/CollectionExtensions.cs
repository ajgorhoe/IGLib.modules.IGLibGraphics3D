using IGLib.Core.CollectionExtensions_OLD;
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
        public const string ArrayBracketOpen = "{";
        public const string ArrayBracketClosed = "}";
        public const string ArraySeparator = ",";
        public const string ArrayIndentation = "    ";
        public const string NullString = "null";

        /// <summary>
        /// Converts an object to a readable string by dynamically determining its type.
        /// Handles 1D, 2D, 3D arrays, jagged arrays, lists, and enumerables.
        /// </summary>
        public static string ToReadableString(this object o, string indentation = ArrayIndentation, string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed, string separator = ArraySeparator)
        {
            if (o == null)
                return NullString;

            if (o is Array array)
            {
                if (array.Rank == 1)
                    return array.Cast<object>().ToArray().ToReadableString(indentation, openBracket, closedBracket, separator);

                if (array.Rank == 2)
                    return array.ToReadableString(indentation, openBracket, closedBracket, separator);

                if (array.Rank == 3)
                    return array.ToReadableString(indentation, openBracket, closedBracket, separator);

                if (array.GetType().GetElementType()?.IsArray == true)
                    return HandleJaggedArray(array, 0, indentation, openBracket, closedBracket, separator);
            }

            if (o is IList list)
                return list.Cast<object>().ToReadableString(indentation, openBracket, closedBracket, separator);

            if (o is IEnumerable enumerable)
                return enumerable.Cast<object>().ToReadableString(indentation, openBracket, closedBracket, separator);

            return o.ToString();
        }

        public static string ToReadableString<T>(this T[] array, string indentation = ArrayIndentation, string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed, string separator = ArraySeparator)
        {
            if (array == null)
                return NullString;

            return $"{openBracket}{string.Join(separator + " ", array)}{closedBracket}";
        }

        public static string ToReadableString2D<T>(this T[,] array, string indentation = ArrayIndentation, string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed, string separator = ArraySeparator)
        {
            if (array == null)
                return NullString;

            var sb = new StringBuilder();
            sb.Append($"{openBracket}\n");
            for (int i = 0; i < array.GetLength(0); i++)
            {
                sb.Append($"{indentation}{openBracket}");
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    sb.Append(array[i, j]);
                    if (j < array.GetLength(1) - 1)
                        sb.Append($"{separator} ");
                }
                sb.Append($"{closedBracket}");
                if (i < array.GetLength(0) - 1)
                    sb.Append($"{separator}");
                sb.Append("\n");
            }
            sb.Append($"{closedBracket}");
            return sb.ToString();
        }

        public static string ToReadableString3D<T>(this T[,,] array, string indentation = ArrayIndentation, string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed, string separator = ArraySeparator)
        {
            if (array == null)
                return NullString;

            var sb = new StringBuilder();
            sb.Append($"{openBracket}\n");
            for (int i = 0; i < array.GetLength(0); i++)
            {
                sb.Append($"{indentation}{openBracket}\n");
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    sb.Append($"{indentation}{indentation}{openBracket}");
                    for (int k = 0; k < array.GetLength(2); k++)
                    {
                        sb.Append(array[i, j, k]);
                        if (k < array.GetLength(2) - 1)
                            sb.Append($"{separator} ");
                    }
                    sb.Append($"{closedBracket}");
                    if (j < array.GetLength(1) - 1)
                        sb.Append($"{separator}");
                    sb.Append("\n");
                }
                sb.Append($"{indentation}{closedBracket}");
                if (i < array.GetLength(0) - 1)
                    sb.Append($"{separator}");
                sb.Append("\n");
            }
            sb.Append($"{closedBracket}");
            return sb.ToString();
        }

        public static string ToReadableString<T>(this T[][] jaggedArray, string indentation = ArrayIndentation, string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed, string separator = ArraySeparator)
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

        private static string HandleJaggedArray(Array jaggedArray, int indentLevel, string indentation, string openBracket, string closedBracket, string separator)
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
                    sb.Append($"{new string(' ', (indentLevel + 1) * indentation.Length)}{inner1DArray.Cast<object>().ToArray().ToReadableString(indentation, openBracket, closedBracket, separator)}");
                }

                if (i < jaggedArray.Length - 1)
                    sb.Append($"{separator}\n");
            }
            sb.Append($"\n{indent}{closedBracket}");
            return sb.ToString();
        }
    }
}