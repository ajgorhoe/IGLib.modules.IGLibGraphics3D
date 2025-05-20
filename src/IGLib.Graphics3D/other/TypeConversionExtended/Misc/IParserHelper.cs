using System;
using System.Globalization;

namespace IGLib.Core
{

    /// <summary>
    /// 
    /// </summary>
    public static class ParseHelper
    {



        /// <summary>Attempts to parse a string to a specific type with a format provider using reflection, with
        /// <see cref="CultureInfo.InvariantCulture"/> as format provider.
        /// </summary>
        public static bool TryParse<T>(string input, out T result)
        {
            return TryParse<T>(input, CultureInfo.InvariantCulture, out result);
        }


        /// <summary>Attempts to parse a string to a specific type with a format provider using reflection.
        /// If such method does not exist on the type, it tries to fall back to <see cref="TryParseDefault{T}(string, out T)"/>.
        /// </summary>
        public static bool TryParse<T>(string input, IFormatProvider formatProvider, out T result)
        {
            result = default;

            // Check for TryParse with format provider
            var tryParseMethod = typeof(T).GetMethod("TryParse", new[] { typeof(string), typeof(IFormatProvider), typeof(T).MakeByRefType() });
            if (tryParseMethod != null)
            {
                var parameters = new object[] { input, formatProvider, null };
                bool success = (bool)tryParseMethod.Invoke(null, parameters);
                if (success)
                {
                    result = (T)parameters[2];
                }
                return success;
            }

            // Fallback: If no TryParse with format provider is found, use default TryParse with two parameters
            return TryParseDefault(input, out result);
        }


        /// <summary>Tries to parse an object of the specified type <typeparamref name="T"/> and
        /// store it to the provided reference.
        /// <para>This method uses reflection to check whether the type <typeparamref name="T"/> contains the method
        /// TryParse. If it does, it executes that method via reflection, writes the result to <paramref name="result"/>,
        /// and returns true. If it does not, it just returns false.</para></summary>
        /// <typeparam name="T">Type of value / object to be parsed from <paramref name="inputString"/>.</typeparam>
        /// <param name="inputString">String that should hold the representation of the object that can be parsed.</param>
        /// <param name="result">Reference of the variable where the parsed object is stored when parsing is successful.</param>
        /// <returns>True if parsing was successful, false otherwise.</returns>
        public static bool TryParseDefault<T>(string inputString, out T result)
        {
            result = default;
            var tryParseMethod = typeof(T).GetMethod("TryParse", new[] { typeof(string), typeof(T).MakeByRefType() });
            if (tryParseMethod != null)
            {
                var parameters = new object[] { inputString, CultureInfo.InvariantCulture, null };
                bool success = (bool)tryParseMethod.Invoke(null, parameters);
                if (success)
                {
                    result = (T)parameters[2];
                }
                return success;
            }
            return false;
        }


        /// <summary>Demonstrates use of the method.</summary>
        public static void ExampleUsage()
        {
            if (ParseHelper.TryParse<int>("123", out int number))
            {
                Console.WriteLine($"Parsed number: {number}");
            }
        }

    }



}
