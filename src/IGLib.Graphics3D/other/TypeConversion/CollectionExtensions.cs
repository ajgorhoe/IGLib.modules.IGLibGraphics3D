using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;


// WARNING: 
// This works while the newer version has produces compiler errors.
// Keep this version until errors are resolved.


namespace IGLib.Core.CollectionExtensions
{

    /// <summary>Various extension methods for collections. In particular, conversion for readable strings,
    /// which produce readable outputs in application or assist in testing.
    /// <para>ToReadableString (<see cref="ToReadableString(object, string, string, string, string)"/> and similar):</para>\
    /// <para>Conversions of various array-like objects to readable strings. Various types of collections
    /// have their own overloads, which are statically dispatched (e.g. <see cref="IList{T}"/>, <see cref="IEnumerable{T}"/>,
    /// T[], rectangular arrays T[,], T[,,], jagged arrays T[][], T[][][]).</para>
    /// <para>There is also the method that detects the type of array-like objects dynamically, <see cref="ToReadableString(object, string, string, string, string)"/>,
    /// and produces, according to the actual type of the argument, a similar output for array-like type of objects than the
    /// corresponding statically typed counterparts.</para>
    /// <para>The ToreadableString(...) type of methods are meant to be used as static extension methods, but certain formatting
    /// aspects are parameterized by static properties <see cref="ArrayBracketOpen"/>, <see cref="ArrayBracketClosed"/>,
    /// <see cref="ArraySeparator"/> and <see cref="ArrayIndentation"/>. Each of these methods has a set of optional 
    /// parameters where these formatting parameters can be customized at will, and the mentioned properties represent
    /// default values for these parameters. This increases reusibility, and if necessary, the equivalent instance classes
    /// whose instances are injectable could easily be created from the current class.</para>
    /// </summary>
    public static class CollectionExtensions
    {

        /// <summary>Open bracket used in the string representation of array-like objects.</summary>
        public const string ArrayBracketOpen = "{";    // alternative: "[";

        /// <summary>Closed bracket used in the string representation of array-like objects.</summary>
        public const string ArrayBracketClosed = "}";    // alternative: "]";

        /// <summary>Array element separator used in the string representation of array-like objects.</summary>
        public const string ArraySeparator = ",";    // alternative: " |";

        /// <summary>Default indentation used in the string representation of array-like objects.</summary>
        public const string ArrayIndentation = "    ";    // alternative: "··";

        /// <summary>String that is used to output null objects.</summary>
        public const string NullString = "null";



        /// <summary>Converts the specified object <paramref name="o"/> to a readable string.
        /// <para>This method dynamically checks the actual type of <paramref name="o"/>. If the object
        /// is an array, 2 or 3 dimensional rectangular array, 2 or 3 dimensional ljagged array, a
        /// list (it implements the <see cref="IList"/> interface) or enumerable (it implements the
        /// <see cref="IEnumerable"/> interface, then the appropriate method is called to produce
        /// the string representation suitable for array-like objects.</para></summary>
        /// <param name="o">Object to be converted to a readable string.</param>
        /// <returns>Ths string containing a representation of the <paramref name="o"/>,
        /// <paramref name="indentation">String used for single indentation; default is <see cref="ArrayIndentation"/></paramref>
        /// <paramref name="openBracket">String used as open bracket in array representation; default is <see cref="ArrayBracketOpen"/></paramref>
        /// <paramref name="closedBracket">String used as closed bracket in array representation; default is <see cref="ArrayBracketClosed"/></paramref>
        /// <paramref name="separator">String used as separator in array representation; default is <see cref="ArraySeparator"/></paramref>
        /// which is also nicely structured and readable for objects of array-like types.</returns>
        public static string ToReadableString(this object o, string indentation = ArrayIndentation,
            string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed, 
            string separator = ArraySeparator)
        {
            // Null check
            if (o == null)
                return "null";

            // Handle jagged arrays (1D array of arrays or deeper)
            if (o is Array jaggedArray && jaggedArray.GetType().GetElementType()?.IsArray == true)
            {
                return HandleJaggedArray(jaggedArray, 0, indentation, openBracket, closedBracket, separator);
            }

            // Handle 1D flat arrays
            if (o is Array array && array.Rank == 1)
            {
                return array.Cast<object>().ToArray().ToReadableString(indentation, openBracket, closedBracket, separator);
            }

            // Handle 2D rectangular arrays
            if (o is Array array2D && array2D.Rank == 2)
            {
                return CastAndCallToReadableString(array2D, indentation, openBracket, closedBracket, separator);
            }

            // Handle 3D rectangular arrays
            if (o is Array array3D && array3D.Rank == 3)
            {
                return CastAndCallToReadableString(array3D, indentation, openBracket, closedBracket, separator);
            }

            // Handle IList
            if (o is IList list)
            {
                return list.Cast<object>().ToReadableString(indentation, openBracket, closedBracket, separator);
            }

            // Handle IEnumerable
            if (o is IEnumerable enumerable)
            {
                return enumerable.Cast<object>().ToReadableString(indentation, openBracket, closedBracket, separator);
            }

            // Fallback for non-collection types
            return o.ToString();
        }

        private static string CastAndCallToReadableString(Array array, string indentation = ArrayIndentation,
            string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed,
            string separator = ArraySeparator)
        {
            if (array is int[,] int2DArray)
            {
                return int2DArray.ToReadableString(indentation, openBracket, closedBracket, separator);
            }
            if (array is int[,,] int3DArray)
            {
                return int3DArray.ToReadableString(indentation, openBracket, closedBracket, separator);
            }
            if (array is string[,] string2DArray)
            {
                return string2DArray.ToReadableString(indentation, openBracket, closedBracket, separator);
            }
            if (array is string[,,] string3DArray)
            {
                return string3DArray.ToReadableString(indentation, openBracket, closedBracket, separator);
            }
            // Add more types as needed
            return array.ToString(); // Fallback for unsupported types
        }

        /// <summary>A helper method to handle jagged arrays dynamically.</summary>
        /// <param name="jaggedArray">The jagged array whose string representation should be returned.</param>
        /// <returns>String representation of the specified jagged array.</returns>
        private static string HandleJaggedArray(Array jaggedArray, int indentLevel = 0, string indentation = ArrayIndentation,
            string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed,
            string separator = ArraySeparator)
        {
            var sb = new StringBuilder();
            // This has been replaced: string indent = new string(' ', indentLevel * 4); // Indentation string based on the current level
            string indent = string.Concat(Enumerable.Repeat(indentation, indentLevel));
            sb.Append(indent + $"{openBracket}\n");
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                var element = jaggedArray.GetValue(i);

                if (element is Array innerArray && innerArray.GetType().GetElementType()?.IsArray == true)
                {
                    // Recursively handle nested jagged arrays
                    sb.Append(HandleJaggedArray((Array)element, indentLevel + 1,
                        indentation, openBracket, closedBracket, separator));
                }
                else if (element is Array inner1DArray)
                {
                    // Handle 1D arrays
                    // This is replaced below: sb.Append(new string(' ', (indentLevel + 1) * 4));
                    sb.Append(string.Concat(Enumerable.Repeat(indentation, indentLevel + 1)));
                    sb.Append(inner1DArray.Cast<object>().ToArray().ToReadableString(indentation, openBracket, closedBracket, separator));
                }

                // Append a comma unless it's the last element
                if (i < jaggedArray.Length - 1)
                {
                    sb.Append($"{separator}");
                }

                sb.Append($"\n");
            }
            sb.Append(indent + $"{closedBracket}");
            return sb.ToString();
        }


        /// <summary>Converts 1D  array to a readable string.</summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="array">Array to be converted.</param>
        public static string ToReadableString<T>(this T[] array, string indentation = ArrayIndentation,
            string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed,
            string separator = ArraySeparator)
        {
            if (array == null)
            {
                return NullString;
            }
            return $"{openBracket}{string.Join($"{separator} ", array)}{closedBracket}";
        }


        /// <summary>Converts an <see cref="IList{T}"/> object to a readable string.</summary>
        /// <typeparam name="T">Type of list elements.</typeparam>
        /// <param name="list"><see cref="IList{T}"/> object to be converted.</param>
        public static string ToReadableString<T>(this IList<T> list, string indentation = ArrayIndentation,
            string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed,
            string separator = ArraySeparator)
        {
            return $"{openBracket}{string.Join($"{separator} ", list)}{closedBracket}";
        }


        // Extension method for generic IEnumerable<T>
        /// <summary>Converts an <see cref="IEnumerable{T}"/> object to a readable string.</summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="enumerable"><see cref="IEnumerable{T}"/> object to be converted.</param>
        /// <param name="indentation">This parameter is not relevent for the current method, but is kept
        /// in order to resolve ambiguity with metods that take a jagged array parameter.</param>
        public static string ToReadableString<T>(this IEnumerable<T> enumerable, string indentation = ArrayIndentation,
            string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed,
            string separator = ArraySeparator)
        {
            return $"{openBracket}{string.Join($"{separator} ", enumerable)}{closedBracket}";
        }


        /// <summary>Converts a 2D rectangular array to a readable string.</summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="array">Array to be converted.</param>
        /// <returns>Readable string representation of the 2D rectangular array.</returns>
        public static string ToReadableString<T>(this T[,] array, string indentation = ArrayIndentation,
            string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed,
            string separator = ArraySeparator)
        {
            if (array == null)
            {
                return NullString;
            }

            var sb = new StringBuilder();
            sb.Append($"{openBracket}\n");
            for (int i = 0; i < array.GetLength(0); i++)
            {
                sb.Append($"{indentation}{openBracket}");
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    sb.Append(array[i, j]);
                    if (j < array.GetLength(1) - 1) // Avoid trailing comma
                        sb.Append($"{separator} ");
                }
                sb.Append($"{closedBracket}");
                if (i < array.GetLength(0) - 1) // Avoid trailing comma
                    sb.Append($"{separator}");
                sb.Append($"\n");
            }
            sb.Append($"{closedBracket}");
            return sb.ToString();
        }

        /// <summary>Converts a 3D rectangular array to a readable string.</summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="array">Array to be converted.</param>
        /// <returns>String representation of the 3D rectangular array.</returns>
        public static string ToReadableString<T>(this T[,,] array, string indentation = ArrayIndentation,
            string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed,
            string separator = ArraySeparator)
        {
            if (array == null)
            {
                return NullString;
            }

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
                        if (k < array.GetLength(2) - 1) // Avoid trailing comma
                            sb.Append($"{separator} ");
                    }
                    sb.Append($"{closedBracket}");
                    if (j < array.GetLength(1) - 1) // Avoid trailing comma
                        sb.Append($"{separator}");
                    sb.Append($"\n");
                }
                sb.Append($"{indentation}{closedBracket}");
                if (i < array.GetLength(0) - 1) // Avoid trailing comma
                    sb.Append($"{separator}");
                sb.Append($"\n");
            }
            sb.Append($"{closedBracket}");
            return sb.ToString();
        }

        // Extension method for jagged arrays
        public static string ToReadableString<T>(this T[][] jaggedArray, string indentation = ArrayIndentation,
            string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed,
            string separator = ArraySeparator)
        {
            if (jaggedArray == null)
            {
                return NullString;
            }

            var sb = new StringBuilder();
            sb.Append($"{openBracket}\n");
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                sb.Append($"{indentation}");
                sb.Append(jaggedArray[i].ToReadableString(indentation, openBracket, closedBracket, separator)); // Reuse 1D array method
                if (i < jaggedArray.Length - 1) // Avoid trailing comma
                    sb.Append($"{separator}");
                sb.Append($"\n");
            }
            sb.Append($"{closedBracket}");
            return sb.ToString();
        }

        public static string ToReadableString<T>(this T[][][] jaggedArray, string indentation = ArrayIndentation,
            string openBracket = ArrayBracketOpen, string closedBracket = ArrayBracketClosed,
            string separator = ArraySeparator)
        {
            if (jaggedArray == null)
            {
                return NullString;
            }
            var sb = new StringBuilder();
            sb.Append($"{openBracket}\n");
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                sb.Append($"{indentation}{openBracket}\n");
                for (int j = 0; j < jaggedArray[i].Length; j++)
                {
                    sb.Append($"{indentation}{indentation}");
                    sb.Append(jaggedArray[i][j].ToReadableString(indentation, openBracket, closedBracket, separator)); // Reuse 1D array method
                    if (j < jaggedArray[i].Length - 1) // Avoid trailing comma
                        sb.Append($"{separator}");
                    sb.Append($"\n");
                }
                sb.Append($"{indentation}{closedBracket}");
                if (i < jaggedArray.Length - 1) // Avoid trailing comma
                    sb.Append($"{separator}");
                sb.Append($"\n");
            }
            sb.Append($"{closedBracket}");
            return sb.ToString();
        }


    }

}