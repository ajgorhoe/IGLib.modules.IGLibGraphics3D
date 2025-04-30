using System;

namespace IGLib.Core
{
    /// <summary>
    /// Defines methods for converting objects to a specified type.
    /// </summary>
    public interface ITypeConverter
    {
        /// <summary>
        /// Converts the specified value to the specified target type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        object ConvertToType(object value, Type targetType);

        /// <summary>
        /// Converts the specified value to the specified target type.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        T ConvertToType<T>(object value);

        /// <summary>
        /// Converts the specified value to the specified target type.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="result">When this method returns, contains the converted value.</param>
        void ConvertToType<T>(object value, out T result);
    }
}
