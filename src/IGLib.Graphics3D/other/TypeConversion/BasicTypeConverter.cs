using System;

namespace IGLib.Core
{
    /// <summary>
    /// Provides basic type conversion functionality using IConvertible and nullable handling.
    /// </summary>
    public class BasicTypeConverter : ITypeConverter
    {
        /// <summary>
        /// Converts the specified value to the specified target type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public virtual object ConvertToType(object value, Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            try
            {
                if (value == null)
                {
                    if (IsNullableType(targetType))
                        return null;
                    else
                        throw new InvalidOperationException($"Cannot assign null to non-nullable type {targetType.FullName}.");
                }

                Type actualTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

                if (actualTargetType.IsInstanceOfType(value))
                {
                    return value;
                }

                if (value is IConvertible && typeof(IConvertible).IsAssignableFrom(actualTargetType))
                {
                    return Convert.ChangeType(value, actualTargetType);
                }

                throw new InvalidOperationException($"Cannot convert value of type {value.GetType().FullName} to type {targetType.FullName}.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to convert value of type {value?.GetType().FullName ?? "null"} to type {targetType.FullName}.", ex);
            }
        }

        /// <summary>
        /// Converts the specified value to the specified target type.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public T ConvertToType<T>(object value)
        {
            return (T)ConvertToType(value, typeof(T));
        }

        /// <summary>
        /// Converts the specified value to the specified target type.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="result">When this method returns, contains the converted value.</param>
        public void ConvertToType<T>(object value, out T result)
        {
            result = ConvertToType<T>(value);
        }

        /// <summary>
        /// Determines whether the specified type is nullable.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is nullable; otherwise, false.</returns>
        private static bool IsNullableType(Type type)
        {
            return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }
    }
}
