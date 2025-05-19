using System;

namespace IGLib.Core.Extended
{

    /// <summary>
    /// 
    /// </summary>
    public static class ParseHelper
    {


        /// <summary>Tries to parse an object of the specified type <typeparamref name="T"/> and
        /// store it to the provided reference.
        /// <para>This method uses reflection to check whether the type <typeparamref name="T"/> contains the method
        /// TryParse. If it does, it executes that method via reflection, writes the result to <paramref name="result"/>,
        /// and returns true. If it does not, it just returns false.</para></summary>
        /// <typeparam name="T">Type of value / object to be parsed from <paramref name="inputString"/>.</typeparam>
        /// <param name="inputString">String that should hold the representation of the object that can be parsed.</param>
        /// <param name="result">Reference of the variable where the parsed object is stored when parsing is successful.</param>
        /// <returns>True if parsing was successful, false otherwise.</returns>
        public static bool TryParse<T>(string inputString, out T result)
        {
            result = default;
            var tryParseMethod = typeof(T).GetMethod("TryParse", new[] { typeof(string), typeof(T).MakeByRefType() });
            if (tryParseMethod != null)
            {
                var parameters = new object[] { inputString, null };
                bool success = (bool)tryParseMethod.Invoke(null, parameters);
                if (success)
                {
                    result = (T)parameters[1];
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
